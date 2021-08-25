// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using GnuCashIntegration.Entities;
    using GnuCashIntegration.Scheduling;

    public static class Reports
    {
        private static readonly string[] formats = new string[] { "yyyyMMddHHmmss", "yyyyMMdd" };

        public static Dictionary<int, decimal> CashFlow(GnuCashDatabase db)
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddYears(12).Date;

            var balances = new Dictionary<DateTime, decimal>();

            var allAccounts = db.Accounts.ToList();
            var allTransactions = db.Transactions.ToList();
            var allSplits = db.Splits.ToList();
            var allScheduledTransactions = db.ScheduledTransactions.ToList();
            var allRecurrences = db.Recurrences.ToList();
            var allSlots = db.Slots.ToList();

            var intrust = (from a in allAccounts
                           where a.Name.Contains("Intrust")
                           select a).Single();

            var intrustSplits = from s in allSplits
                                where s.AccountGuid == intrust.Guid
                                select s;

            var intrustTx = from t in allTransactions
                            where intrustSplits.Any(s => s.TransactionGuid == t.Guid)
                            select t;

            var txList = intrustTx.ToList();

            var splitsList = (from s in intrustSplits.ToList()
                              select new
                              {
                                  Num = (decimal)s.QuantityNumerator,
                                  Denom = (decimal)s.QuantityDenominator,
                                  PostDate = ParseDate(txList.Single(t => t.Guid == s.TransactionGuid).PostDate).Date,
                              }).ToList();

            var startingBalance = (from s in splitsList
                                   where s.PostDate < startDate
                                   select s.Num / s.Denom).Sum();

            var balance = startingBalance;
            for (var d = startDate; d <= endDate; d = d.AddDays(1))
            {
                balances[d.Date] = (from s in splitsList
                                    where s.PostDate == d
                                    select s.Num / s.Denom).Sum() - 22;
            }

            ////Evaluator eval = new Evaluator();
            FakeEvaluator eval = null;

            foreach (var s in allScheduledTransactions)
            {
                var schedule = new Schedule(
                     s.Name,
                     ParseDate(s.StartDate),
                     TryParseDate(s.EndDate, out var scheduleEndDate) ? (DateTime?)scheduleEndDate : null,
                     TryParseDate(s.LastOccurence, out var lastOccurence) ? (DateTime?)lastOccurence : null,
                     (int)s.NumOccurences,
                     (int)s.RemainingOccurences,
                     from r in allRecurrences.ToList()
                     where r.ScheduledTransactionGuid == s.Guid
                     select GetRecurrenceBase(r));

                foreach (var d in schedule.GetDatesInRange(startDate, endDate))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        ["i"] = d.Key,
                    };

                    foreach (var sp in from sp in allSplits
                                       where sp.AccountGuid == s.TemplateAccountGuid
                                       let slots = from sl in allSlots
                                                   where sl.ObjGuid == sp.Guid
                                                   select sl
                                       let credit = slots.FirstOrDefault(sl => sl.Name == "sched-xaction/credit-formula")
                                       let debit = slots.FirstOrDefault(sl => sl.Name == "sched-xaction/debit-formula")
                                       let account = slots.FirstOrDefault(sl => sl.Name == "sched-xaction/account")
                                       select new
                                       {
                                           Split = sp,
                                           Credit = credit?.StringVal,
                                           Debit = debit?.StringVal,
                                           Account = account?.GuidVal,
                                       })
                    {
                        decimal credit = 0;
                        decimal debit = 0;
                        Account account = null;

                        if (!string.IsNullOrEmpty(sp.Credit))
                        {
                            var expr = sp.Credit.Replace(",", string.Empty).Replace(':', ',').Trim();
                            credit = decimal.Parse(eval.Evaluate(expr, parameters).ToString());
                        }

                        if (!string.IsNullOrEmpty(sp.Debit))
                        {
                            var expr = sp.Debit.Replace(",", string.Empty).Replace(':', ',').Trim();
                            debit = -decimal.Parse(eval.Evaluate(expr, parameters).ToString());
                        }

                        if (!string.IsNullOrEmpty(sp.Account))
                        {
                            account = (from a in allAccounts
                                       where a.Guid == sp.Account
                                       select a).SingleOrDefault();
                        }

                        if (account.Guid != intrust.Guid)
                        {
                            continue;
                        }

                        var amount = -(credit + debit);

                        if (!balances.ContainsKey(d.Value.Date))
                        {
                            balances[d.Value.Date] = 0;
                        }

                        balances[d.Value.Date] += amount;
                    }
                }
            }

            var result = new Dictionary<int, decimal>();

            var runningBalance = balance;
            foreach (var d in balances)
            {
                runningBalance += d.Value;
                result[(int)(d.Key - startDate).TotalDays] = runningBalance;
            }

            return result;
        }

        public static DateTime ParseDate(string s)
        {
            return DateTime.ParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static bool TryParseDate(string s, out DateTime result)
        {
            return DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        private static RecurrenceBase GetRecurrenceBase(Recurrence r)
        {
            RecurrenceBase b;
            switch (r.PeriodType)
            {
                case "month":
                    b = new MonthRecurrence(
                        ParseDate(r.PeriodStart),
                        (int)r.Multiplier);
                    break;

                case "end of month":
                    b = new MonthRecurrence(
                        ParseDate(r.PeriodStart),
                        (int)r.Multiplier);
                    break;

                case "week":
                    b = new WeekRecurrence(
                        ParseDate(r.PeriodStart),
                        (int)r.Multiplier);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return b;
        }

        private class FakeEvaluator
        {
            public object Evaluate(string expr, Dictionary<string, object> parameters)
            {
                return null;
            }
        }
    }
}

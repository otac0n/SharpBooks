using SharpCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SharpCash.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(90);

            Dictionary<DateTime, decimal> balances = new Dictionary<DateTime, decimal>();

            try
            {
                var db = new GnuCashDatabase();
                var book = db.Books.FirstOrDefault();
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
                                where intrustSplits.Where(s => s.TransactionGuid == t.Guid).Any()
                                select t;

                var txList = intrustTx.ToList();

                var splitsList = (from s in intrustSplits.ToList()
                                  select new
                                  {
                                      Num = (decimal)s.QuantityNumerator,
                                      Denom = (decimal)s.QuantityDenominator,
                                      PostDate = ParseDate(txList.Where(t => t.Guid == s.TransactionGuid).Single().PostDate).Date
                                  }).ToList();

                var startingBalance = (from s in splitsList
                               where s.PostDate < startDate
                               select s.Num / s.Denom).Sum();

                var balance = startingBalance;
                for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
                {
                    balances[d.Date] = (from s in splitsList
                                where s.PostDate == d
                                select s.Num / s.Denom).Sum();
                }


                Evaluator eval = new Evaluator();

                foreach (var s in allScheduledTransactions)
                {
                    DateTime temp;

                    var schedule = new Schedule(
                         s.Name,
                         ParseDate(s.StartDate),
                         TryParseDate(s.EndDate, out temp) ? (DateTime?)temp : null,
                         TryParseDate(s.LastOccurence, out temp) ? (DateTime?)temp : null,
                         (int)s.NumOccurences,
                         (int)s.RemainingOccurences,
                         from r in allRecurrences.ToList()
                         where r.ScheduledTransactionGuid == s.Guid
                         select GetRecurrenceBase(r));

                    foreach (var d in schedule.GetDatesInRange(startDate, endDate))
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters["i"] = d.Key;

                        foreach (var sp in from sp in allSplits
                                           where sp.AccountGuid == s.TemplateAccountGuid
                                           let slots = from sl in allSlots
                                                       where sl.ObjGuid == sp.Guid
                                                       select sl
                                           let credit = slots.Where(sl => sl.Name == "sched-xaction/credit-formula").FirstOrDefault()
                                           let debit = slots.Where(sl => sl.Name == "sched-xaction/debit-formula").FirstOrDefault()
                                           let account = slots.Where(sl => sl.Name == "sched-xaction/account").FirstOrDefault()
                                           select new
                                           {
                                               Split = sp,
                                               Credit = credit == null ? null : credit.StringVal,
                                               Debit = debit == null ? null : debit.StringVal,
                                               Account = account == null ? null : account.GuidVal,
                                           })
                        {
                            decimal credit = 0;
                            decimal debit = 0;
                            decimal amount = 0;
                            Account account = null;
                            
                            if (!string.IsNullOrEmpty(sp.Credit))
                            {
                                var expr = sp.Credit.Replace(",", "").Replace(':', ',').Trim();
                                credit = decimal.Parse(eval.Evaluate(expr, parameters).ToString());
                            }
                            
                            if (!string.IsNullOrEmpty(sp.Debit))
                            {
                                var expr = sp.Debit.Replace(",", "").Replace(':', ',').Trim();
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

                            amount = -(credit + debit);

                            if (!balances.ContainsKey(d.Value.Date))
                            {
                                balances[d.Value.Date] += 0;
                            }

                            balances[d.Value.Date] += amount;
                        }
                    }
                }

                var runningBalance = balance;
                foreach (var d in balances)
                {
                    runningBalance += d.Value;
                    Console.WriteLine("{0:yyyy-MM-dd}\t{1}", d.Key, runningBalance);
                }

            }
            finally
            {
                Console.ReadKey(true);
            }
        }

        private static RecurrenceBase GetRecurrenceBase(Recurrence r)
        {
            RecurrenceBase b = null;
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
                    throw new Exception();
            }

            return b;
        }

        private static string[] formats = new string[] { "yyyyMMddHHmmss", "yyyyMMdd" };

        public static DateTime ParseDate(string s)
        {
            return DateTime.ParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static bool TryParseDate(string s, out DateTime result)
        {
            return DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}

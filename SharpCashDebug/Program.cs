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

                var balance = (from s in splitsList
                               where s.PostDate < startDate
                               select s.Num / s.Denom).Sum();

                Console.WriteLine(balance);
                Console.WriteLine("------------------");

                for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
                {
                    balance += (from s in splitsList
                                where s.PostDate == d
                                select s.Num / s.Denom).Sum();
                    Console.WriteLine("{0:yyyy-MM-dd}\t{1}", d, balance);
                }
                Console.WriteLine("------------------");


                Evaluator eval = new Evaluator();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["i"] = 1;

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
                        Console.Write("{0:yyyy-MM-dd}\t", d);
                    }
                    Console.WriteLine();

                    foreach (var sp in from sp in allSplits
                                       where sp.AccountGuid == s.TemplateAccountGuid
                                       let slots = from sl in allSlots
                                                   where sl.ObjGuid == sp.Guid
                                                   select sl
                                       let credit = slots.Where(sl => sl.Name == "sched-xaction/credit-formula").FirstOrDefault()
                                       let debit = slots.Where(sl => sl.Name == "sched-xaction/debit-formula").FirstOrDefault()
                                       select new {
                                            Split = sp,
                                            Credit = credit == null ? null : credit.StringVal,
                                            Debit = debit == null ? null : debit.StringVal
                                       })
                    {
                        //Console.WriteLine("---");
                        object credit = null;
                        object debit = null;
                        if (!string.IsNullOrEmpty(sp.Credit))
                        {
                            var expr = sp.Credit.Replace(",", "").Replace(':', ',').Trim();
                            credit = eval.Evaluate(expr, parameters);
                        }
                        if (!string.IsNullOrEmpty(sp.Debit))
                        {
                            var expr = sp.Debit.Replace(",", "").Replace(':', ',').Trim();
                            debit = eval.Evaluate(expr, parameters);
                        }

                        Console.WriteLine("{0}\t{1}\t{2}", sp.Split.Memo, credit, debit);
                    }

                }
                Console.WriteLine("------------------");

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

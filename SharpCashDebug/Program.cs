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

                var intrust = (from a in db.Accounts
                               where a.Name.Contains("Intrust")
                               select a).FirstOrDefault();

                var intrustSplits = from s in db.Splits
                                    where s.AccountGuid == intrust.Guid
                                    select s;

                var intrustTx = from t in db.Transactions
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
                foreach (var s in db.ScheduledTransactions)
                {
                    Console.WriteLine("------");
                    foreach (var rec in from r in db.Recurrences.ToList()
                                        where r.ScheduledTransactionGuid == s.Guid
                                        select GetRecurrenceBase(r))
                    {


                        Console.WriteLine(rec.GetNextOccurence());
                    }
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

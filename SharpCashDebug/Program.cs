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
            var endDate = DateTime.Today.AddDays(20);

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
                    foreach (var rec in db.Recurrences)
                    {
                        RecurrenceBase r = null;
                        switch (rec.PeriodType)
                        {
                            case "month":
                                r = new MonthRecurrence(
                                    ParseDate(rec.PeriodStart),
                                    (int)rec.Multiplier);
                                break;
                            case "end of month":
                                r = new MonthRecurrence(
                                    ParseDate(rec.PeriodStart),
                                    (int)rec.Multiplier);
                                break;
                            case "week":
                                r = new WeekRecurrence(
                                    ParseDate(rec.PeriodStart),
                                    (int)rec.Multiplier);
                                break;
                            default:
                                throw new Exception();
                        }

                        Console.WriteLine("{0}  {1}  {2}", rec.PeriodStart, rec.PeriodType, rec.Multiplier);
                    }
                }
            }
            finally
            {
                Console.ReadKey(true);
            }
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

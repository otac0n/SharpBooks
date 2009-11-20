using SharpCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            var startDate = DateTime.Today.ToString("yyyyMMdHHmmss");
            var endDate = DateTime.Today.AddDays(90).ToString("yyyyMMdHHmmss");

            try
            {
                var db = new GnuCashDatabase();

                var book = db.Books.FirstOrDefault();

                var intrust = (from a in db.Accounts
                               where a.Name.Contains("Intrust")
                               select a).FirstOrDefault();

                var intrustSplits = from s in db.Splits
                                 where s.account_guid == intrust.Guid
                                 select s;

                var intrustTransactions = from t in db.Transactions
                                          where intrustSplits.Where(s => s.tx_guid == t.guid).Any()
                                          select t;

                var splitsList = intrustSplits.ToList();
                var txList = intrustTransactions.ToList();

                var balance = (from s in splitsList
                               where s.account_guid == intrust.Guid
                               where txList.Where(t => t.guid == s.tx_guid).Single().PostDate.CompareTo(DateTime.Today.ToString("yyyyMMddHHmmss")) >= 1
                               select (decimal)s.quantity_num / s.quantity_denom).Sum();
                
                Console.WriteLine(balance);
            }
            finally
            {
                Console.ReadKey(true);
            }
        }
    }
}

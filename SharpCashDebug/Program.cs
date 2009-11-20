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

                var allSplits = (from s in db.Splits
                                 where s.account_guid == intrust.Guid
                                 select s).ToList();

                var balance = (from s in allSplits
                               where s.account_guid == intrust.Guid
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

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

                var denom = (double)(from s in db.Splits
                                        where s.account_guid == intrust.Guid
                                        select s.quantity_denom).Max();

                var balance = (from s in db.Splits
                               where s.account_guid == intrust.Guid
                               select s.quantity_num * (denom / s.quantity_denom)).Sum();
                
                Console.WriteLine(balance / denom);
            }
            finally
            {
                Console.ReadKey(true);
            }
        }
    }
}

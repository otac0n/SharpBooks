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

                var intrustTx = from t in db.Transactions
                                          where intrustSplits.Where(s => s.tx_guid == t.guid).Any()
                                          select t;

                var txList = intrustTx.ToList();

                var splitsList = (from s in intrustSplits.ToList()
                                  select new
                                  {
                                      Num = (decimal)s.value_num,
                                      Denom = (decimal)s.value_denom,
                                      PostDate = DateTime.ParseExact(txList.Where(t => t.guid == s.tx_guid).Single().PostDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).Date
                                  }).ToList();

                var balance = (from s in splitsList
                               where s.PostDate <= DateTime.Today
                               select s.Num / s.Denom).Sum();
                
                Console.WriteLine(balance);
            }
            finally
            {
                Console.ReadKey(true);
            }
        }
    }
}

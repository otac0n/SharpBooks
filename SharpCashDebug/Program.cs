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
            try
            {
                var db = new GnuCashDatabase();

                var book = db.Books.FirstOrDefault();

                var intrust = (from a in db.Accounts
                               where a.Name.Contains("Intrust")
                               select a).FirstOrDefault();

                foreach (var s in from s in db.Splits
                                  select new { Split = s, Value = (double)s.quantity_num / s.quantity_denom })
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", s.Split.quantity_num, s.Split.quantity_denom, (decimal)s.Split.quantity_num / s.Split.quantity_denom, s.Value);
                }

                //Console.WriteLine(balance);
            }
            finally
            {
                Console.ReadKey(true);
            }
        }
    }
}

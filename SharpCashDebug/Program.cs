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
                var doc = Engine.Load(@"C:\Sync\data\finances.gnucash");
                var book = doc.First();

                var baseCommodity = book.CommodityDatabase.FindCommodity("ISO4217", "USD");

                var account = (from a in book.AccountDatabase.Accounts
                               where a.FullName.Contains("Vanguard 401(k):Van")
                               select a).FirstOrDefault();

                if (account == null)
                {
                    Console.WriteLine("Account could not be found.");
                    return;
                }

                Console.WriteLine("Found account: [" + account.FullName + "]");
                Console.WriteLine();

                var balance = (from s in book.TransactionDatabase.AllSplits
                               where s.Account.Id == account.Id
                               where s.Transaction.DatePosted.Date <= DateTime.Today
                               select s.Value).Sum();
                var quantity = (from s in book.TransactionDatabase.AllSplits
                               where s.Account.Id == account.Id
                               where s.Transaction.DatePosted.Date <= DateTime.Today
                               select s.Quantity).Sum();
                var commodity = account.Commodity;
                var price = book.PriceDatabase.GetPrice(commodity, baseCommodity);
                if (price.HasValue && price.Value != 1)
                {
                    Console.WriteLine("Cost Basis: $" + balance);
                    Console.WriteLine("Balance: " + quantity + " " + commodity.Id);
                    Console.WriteLine("Balance: $" + quantity * price);
                }
                else
                {
                    Console.WriteLine("Balance: $" + balance);
                }
                Console.WriteLine();

                Console.WriteLine("Count of scheduled items: " + book.ScheduleDatabase.Schedules.Count);


            }
            finally
            {
                Console.ReadKey(true);
            }
        }
    }
}

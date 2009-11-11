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

                var account = (from a in book.AccountDatabase.Accounts
                               where a.Name.Contains("Cash")
                               select a).FirstOrDefault();

                if (account == null)
                {
                    Console.WriteLine("Account could not be found.");
                    return;
                }

                Console.WriteLine("Found account: [" + account.FullName + "]");
                Console.WriteLine();

                var balance = (from s in book.TransactionDatabase.AllSplits
                               where s.Account == account
                               where s.Transaction.DatePosted.Date <= DateTime.Today
                               select s.Value).Sum();
                Console.WriteLine("Balance: $" + balance);
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

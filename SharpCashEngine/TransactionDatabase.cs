using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class TransactionDatabase
    {
        private List<Transaction> transactions;

        internal TransactionDatabase(IEnumerable<Transaction> transactions)
        {
            this.transactions = new List<Transaction>();
            if (transactions != null)
            {
                this.transactions.AddRange(transactions);
            }
        }

        public ICollection<Transaction> Transactions
        {
            get
            {
                return this.transactions.AsReadOnly();
            }
        }

        public IEnumerable<Split> AllSplits
        {
            get
            {
                foreach (var t in transactions)
                {
                    foreach (var s in t.Splits)
                    {
                        yield return s;
                    }
                }
            }
        }
    }
}

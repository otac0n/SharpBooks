using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Template
    {
        private AccountDatabase accountDatabase;
        private TransactionDatabase transactionDatabase;

        internal Template(AccountDatabase accountDatabase, TransactionDatabase transactionDatabase)
        {
            this.accountDatabase = accountDatabase;
            this.transactionDatabase = transactionDatabase;
        }

        public AccountDatabase AccountDatabase
        {
            get
            {
                return this.accountDatabase;
            }
        }

        public TransactionDatabase TransactionDatabase
        {
            get
            {
                return this.transactionDatabase;
            }
        }
    }
}

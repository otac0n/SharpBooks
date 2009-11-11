using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class AccountDatabase
    {
        private string accountSeperator = ":";
        private Dictionary<Guid, Account> accounts;

        internal AccountDatabase()
        {
            this.accounts = new Dictionary<Guid, Account>();
        }

        internal void AddAccount(Account account)
        {
            this.accounts.Add(account.Id, account);        
        }

        public string AccountSeperator
        {
            get
            {
                return this.accountSeperator;
            }
        }

        public ICollection<Account> Accounts
        {
            get
            {
                return this.accounts.Values;
            }
        }

        public Account FindAccount(Guid id)
        {
            return this.accounts[id];
        }
    }
}

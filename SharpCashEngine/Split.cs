using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Split
    {
        private AccountDatabase accounts;
        private Transaction transaction;

        private Guid id;
        private Guid account;
        private ReconciledState reconciledState;
        private string memo;
        private decimal value;
        private decimal quantity;

        internal Split(AccountDatabase accounts, Transaction transaction, Guid id, Guid account, ReconciledState reconciledState, string memo, decimal value, decimal quantity)
        {
            this.accounts = accounts;
            this.transaction = transaction;

            this.id = id;
            this.account = account;
            this.reconciledState = reconciledState;
            this.memo = memo;
            this.value = value;
            this.quantity = quantity;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public Guid AccountId
        {
            get
            {
                return this.account;
            }
        }

        public Account Account
        {
            get
            {
                return this.accounts.FindAccount(this.account);
            }
        }

        public Transaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }

        public ReconciledState ReconciledState
        {
            get
            {
                return this.reconciledState;
            }
        }

        public string Memo
        {
            get
            {
                return this.memo;
            }
        }

        public decimal Value
        {
            get
            {
                return this.value;
            }
        }

        public decimal Quantity
        {
            get
            {
                return this.quantity;
            }
        }
    }
}

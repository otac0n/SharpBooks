//-----------------------------------------------------------------------
// <copyright file="Split.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Split
    {
        internal Split(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public Transaction Transaction
        {
            get;
            internal set;
        }

        public DateTime? DateCleared
        {
            get;
            private set;
        }

        public bool IsReconciled
        {
            get;
            private set;
        }

        public Account Account
        {
            get;
            internal set;
        }

        public decimal Ammount
        {
            get;
            internal set;
        }

        public decimal TransactionAmmount
        {
            get;
            internal set;
        }

        public bool IsValid
        {
            get
            {
                return this.GetRuleViolations().Count() == 0;
            }
        }

        public void SetAccount(Account account, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.Account = account;
            }
            finally
            {
                this.Transaction.ExitCriticalSection();
            }
        }

        public void SetAmmount(decimal ammount, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.Ammount = ammount;
            }
            finally
            {
                this.Transaction.ExitCriticalSection();
            }
        }

        public void SetTransactionAmmount(decimal transactionAmmount, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.TransactionAmmount = transactionAmmount;
            }
            finally
            {
                this.Transaction.ExitCriticalSection();
            }
        }

        public void SetDateCleared(DateTime? dateCleared, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.DateCleared = dateCleared;
            }
            finally
            {
                this.Transaction.ExitCriticalSection();
            }
        }

        public void SetIsReconciled(bool isReconciled, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.IsReconciled = isReconciled;
            }
            finally
            {
                this.Transaction.ExitCriticalSection();
            }
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (Math.Sign(this.Ammount) != Math.Sign(this.TransactionAmmount))
            {
                yield return new RuleViolation("Ammount", "The ammount and the transaction ammount of a split must have the same sign.");
            }

            if (this.Account == null)
            {
                yield return new RuleViolation("Account", "The split must be assigned to an account.");
            }

            if (this.Ammount != this.TransactionAmmount && this.Account != null && this.Account.Security == this.Transaction.BaseSecurity)
            {
                yield return new RuleViolation("Ammount", "The ammount and the transaction ammount of a split must have the same value, if they are of the same .");
            }

            yield break;
        }
    }
}

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

        public decimal Amount
        {
            get;
            internal set;
        }

        public decimal TransactionAmount
        {
            get;
            internal set;
        }

        public bool IsValid
        {
            get
            {
                return this.RuleViolations.Count() == 0;
            }
        }

        public IEnumerable<RuleViolation> RuleViolations
        {
            get
            {
                if (Math.Sign(this.Amount) != Math.Sign(this.TransactionAmount))
                {
                    yield return new RuleViolation("Amount", "The amount and the transaction amount of a split must have the same sign.");
                }

                if (this.Account == null)
                {
                    yield return new RuleViolation("Account", "The split must be assigned to an account.");
                }

                if (this.Amount != this.TransactionAmount && this.Account != null && this.Account.Security == this.Transaction.BaseSecurity)
                {
                    yield return new RuleViolation("Amount", "The amount and the transaction amount of a split must have the same value, if they are of the same .");
                }

                yield break;
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

        public void SetAmount(decimal amount, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.Amount = amount;
            }
            finally
            {
                this.Transaction.ExitCriticalSection();
            }
        }

        public void SetTransactionAmount(decimal transactionAmount, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.TransactionAmount = transactionAmount;
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
    }
}

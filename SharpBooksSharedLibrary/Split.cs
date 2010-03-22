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

    public class Split
    {
        internal Split(Transaction transaction, Guid splitId)
        {
            this.SplitId = splitId;
            this.Transaction = transaction;
        }

        public Guid SplitId
        {
            get;
            private set;
        }

        public Transaction Transaction
        {
            get;
            internal set;
        }

        public Commodity Commodity
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

        public void SetCommodity(Commodity commodity, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.Commodity = commodity;
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

            if (this.Ammount != this.TransactionAmmount && this.Commodity == this.Transaction.BaseCommodity)
            {
                yield return new RuleViolation("Ammount", "The ammount and the transaction ammount of a split must have the same value, if they are of the same commodity.");
            }

            yield break;
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="Transaction.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;

    public class Transaction
    {
        private readonly object lockMutex = new object();

        private readonly List<Split> splits = new List<Split>();

        private TransactionLock currentLock;

        public Transaction(Guid transactionId, Commodity baseCommodity)
        {
            if (baseCommodity == null)
            {
                throw new ArgumentNullException("baseCommodity");
            }

            if (transactionId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("transactionId");
            }

            this.BaseCommodity = baseCommodity;
            this.TransactionId = transactionId;
        }

        public Guid TransactionId
        {
            get;
            private set;
        }

        public Commodity BaseCommodity
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get;
            private set;
        }

        public bool IsLocked
        {
            get
            {
                lock (this.lockMutex)
                {
                    return this.currentLock != null;
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return this.GetRuleViolations().Count() == 0;
            }
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            lock (this.lockMutex)
            {
                if (this.splits.Count == 0)
                {
                    yield return new RuleViolation("Splits", "The transaction must have at least one split.");
                }
                else
                {
                    foreach (var split in this.splits)
                    {
                        foreach (var violation in split.GetRuleViolations())
                        {
                            yield return violation;
                        }
                    }

                    if (this.splits.Sum(s => s.TransactionAmmount) != 0m)
                    {
                        yield return new RuleViolation("Sum", "The sum of the splits in the transaction must be equal to zero.");
                    }
                }
            }

            yield break;
        }

        public TransactionLock Lock()
        {
            lock (this.lockMutex)
            {
                if (this.currentLock != null)
                {
                    throw new InvalidOperationException("Could not lock the transaction, because it is already locked.");
                }

                this.currentLock = new TransactionLock(this);
                return this.currentLock;
            }
        }

        public void SetDate(DateTime date, TransactionLock transactionLock)
        {
            lock (this.lockMutex)
            {
                this.ValidateLock(transactionLock);

                this.Date = date;
            }
        }

        public Split AddSplit(TransactionLock transactionLock)
        {
            lock (this.lockMutex)
            {
                this.ValidateLock(transactionLock);

                var split = new Split(this, Guid.NewGuid())
                {
                    Ammount = 0m,
                    TransactionAmmount = 0m,
                };

                this.splits.Add(split);
                return split;
            }
        }

        public void RemoveSplit(Split split, TransactionLock transactionLock)
        {
            if (split == null)
            {
                throw new ArgumentNullException("split");
            }

            lock (this.lockMutex)
            {
                this.ValidateLock(transactionLock);

                if (!this.splits.Contains(split))
                {
                    throw new InvalidOperationException("Could not remove the split from the transaction, because the split is not a member of the transaction.");
                }

                this.splits.Remove(split);
                split.Transaction = null;
            }
        }

        public ReadOnlyCollection<Split> GetSplits(TransactionLock transactionLock)
        {
            lock (this.lockMutex)
            {
                this.ValidateLock(transactionLock);

                return this.splits.AsReadOnly();
            }
        }

        internal void Unlock(TransactionLock transactionLock)
        {
            lock (this.lockMutex)
            {
                if (this.currentLock == null)
                {
                    throw new InvalidOperationException("Could not unlock the transaction, because it is not currently locked.");
                }

                this.currentLock = null;
            }
        }

        internal void EnterCriticalSection()
        {
            Monitor.Enter(this.lockMutex);
        }

        internal void ExitCriticalSection()
        {
            Monitor.Exit(this.lockMutex);
        }

        internal void ValidateLock(TransactionLock transactionLock)
        {
            if (this.currentLock == null)
            {
                throw new InvalidOperationException("Could modify the transaction, because it is not currently locked for editing.");
            }

            if (this.currentLock != transactionLock)
            {
                throw new InvalidOperationException("Could modify the transaction, because the lock provided was not valid.");
            }
        }
    }
}

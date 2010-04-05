//-----------------------------------------------------------------------
// <copyright file="Transaction.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;

    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The 'currentLock' field of the transaction, may fall out of scope safely if the transaction itself is falling out of scope.")]
    public sealed class Transaction
    {
        private readonly object lockMutex = new object();

        private readonly List<Split> splits = new List<Split>();

        private TransactionLock currentLock;

        public Transaction(Guid transactionId, Security baseSecurity)
        {
            if (baseSecurity == null)
            {
                throw new ArgumentNullException("baseSecurity");
            }

            if (transactionId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("transactionId");
            }

            this.BaseSecurity = baseSecurity;
            this.TransactionId = transactionId;
        }

        public Guid TransactionId
        {
            get;
            private set;
        }

        public Security BaseSecurity
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get;
            private set;
        }

        public IEnumerable<Split> Splits
        {
            get
            {
                lock (this.lockMutex)
                {
                    return this.splits.AsReadOnly();
                }
            }
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
                return this.RuleViolations.Count() == 0;
            }
        }

        public IEnumerable<RuleViolation> RuleViolations
        {
            get
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
                            foreach (var violation in split.RuleViolations)
                            {
                                yield return violation;
                            }
                        }

                        if (this.splits.Sum(s => s.TransactionAmount) != 0m)
                        {
                            yield return new RuleViolation("Sum", "The sum of the splits in the transaction must be equal to zero.");
                        }

                        var sameSecurity = from s in this.splits
                                           where s.Account != null && s.Account.Security == this.BaseSecurity
                                           select s;

                        if (!sameSecurity.Any())
                        {
                            yield return new RuleViolation("BaseSecurity", "The transaction must share the same security as at least one of its splits.");
                        }
                    }
                }

                yield break;
            }
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

                var split = new Split(this);

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

        internal void Unlock(TransactionLock transactionLock)
        {
            lock (this.lockMutex)
            {
                if (this.currentLock == null)
                {
                    throw new InvalidOperationException("Could not unlock the transaction, because it is not currently locked.");
                }

                if (this.currentLock != transactionLock)
                {
                    throw new InvalidOperationException("Could not unlock the transaction, because the lock provided was not valid.");
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
                throw new InvalidOperationException("Could not modify the transaction, because it is not currently locked for editing.");
            }

            if (this.currentLock != transactionLock)
            {
                throw new InvalidOperationException("Could not modify the transaction, because the lock provided was not valid.");
            }
        }
    }
}

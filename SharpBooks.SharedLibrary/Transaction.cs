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

    /// <summary>
    /// Encapsulates a financial transaction.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The 'currentLock' field of the transaction, may fall out of scope safely if the transaction itself is falling out of scope.")]
    public sealed class Transaction
    {
        private readonly object lockMutex = new object();

        private readonly List<Split> splits = new List<Split>();

        private TransactionLock currentLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpBooks.Transaction"/> class.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <param name="baseSecurity">The security in which the values of the transaction are expressed.</param>
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

        /// <summary>
        /// Gets the unique identifier of the transaction.
        /// </summary>
        public Guid TransactionId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the security in which the values of the transaction are expressed.
        /// </summary>
        public Security BaseSecurity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date and time at which the transaction took place.
        /// </summary>
        public DateTime Date
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets an enumerable collection of the splits that make up the transaction.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether the transaction is currently locked for editing.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether or not the transaction is currently considered valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.RuleViolations.Count() == 0;
            }
        }

        /// <summary>
        /// Gets an enumerable collection of <see cref="SharpBooks.RuleViolation"/>s that describe features of the transaction that make it invalid.
        /// </summary>
        public IEnumerable<RuleViolation> RuleViolations
        {
            get
            {
                lock (this.lockMutex)
                {
                    if (this.splits.Count == 0)
                    {
                        yield return new RuleViolation("Splits", Localization.Localization.TRANSACTION_MUST_HAVE_SPLIT);
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
                            yield return new RuleViolation("Sum", Localization.Localization.TRANSACTION_SUM_MUST_BE_ZERO);
                        }

                        var sameSecurity = from s in this.splits
                                           where s.Security == this.BaseSecurity
                                           select s;

                        if (!sameSecurity.Any())
                        {
                            yield return new RuleViolation("BaseSecurity", Localization.Localization.TRANSACTION_MUST_SHARE_SECURITY_WITH_A_SPLIT);
                        }
                    }
                }

                yield break;
            }
        }

        /// <summary>
        /// Locks the transaction for editing.
        /// </summary>
        /// <returns>A <see cref="SharpBooks.TransactionLock"/> that must be used for all modifications to the transaction.</returns>
        /// <remarks>
        /// The transaction will be unlocked when the <see cref="SharpBooks.TransactionLock"/> is disposed.
        /// </remarks>
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

        /// <summary>
        /// Sets the date and time at which the transaction took place.
        /// </summary>
        /// <param name="date">The date and time at which the transaction took place.</param>
        /// <param name="transactionLock">A <see cref="SharpBooks.TransactionLock"/> obtained from the <see cref="SharpBooks.Transaction.Lock" /> function.</param>
        public void SetDate(DateTime date, TransactionLock transactionLock)
        {
            lock (this.lockMutex)
            {
                this.ValidateLock(transactionLock);

                this.Date = date;
            }
        }

        /// <summary>
        /// Creates a new split and adds it to the transaction.
        /// </summary>
        /// <param name="transactionLock">A <see cref="SharpBooks.TransactionLock"/> obtained from the <see cref="SharpBooks.Transaction.Lock" /> function.</param>
        /// <returns>The newly created split.</returns>
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

        /// <summary>
        /// Removes a previously added split from the transaction.
        /// </summary>
        /// <param name="split">The previously added split.</param>
        /// <param name="transactionLock">A <see cref="SharpBooks.TransactionLock"/> obtained from the <see cref="Lock" /> function.</param>
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

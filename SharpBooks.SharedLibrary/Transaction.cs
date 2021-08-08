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
    using System.Linq;

    /// <summary>
    /// Encapsulates a financial transaction.
    /// </summary>
    public sealed class Transaction
    {
        private readonly Dictionary<string, string> extensions = new Dictionary<string, string>();

        private readonly List<Split> splits = new List<Split>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
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
            this.Date = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        }

        public string this[string key]
        {
            get
            {
                string value;
                this.extensions.TryGetValue(key, out value);
                return value;
            }
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
        public IList<Split> Splits
        {
            get
            {
                return this.splits.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the transaction is currently considered valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !this.RuleViolations.Any();
            }
        }

        /// <summary>
        /// Gets an enumerable collection of <see cref="RuleViolation"/>s that describe features of the transaction that make it invalid.
        /// </summary>
        public IEnumerable<RuleViolation> RuleViolations
        {
            get
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
        }

        /// <summary>
        /// Sets the date and time at which the transaction took place.
        /// </summary>
        /// <param name="date">The date and time at which the transaction took place.</param>
        public void SetDate(DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentOutOfRangeException("date");
            }

            this.Date = date;
        }

        /// <summary>
        /// Sets an extension on the transaction.
        /// </summary>
        /// <param name="name">The name of the extension.</param>
        /// <param name="value">The value of the extension.</param>
        public void SetExtension(string name, string value)
        {
            if (value == null)
            {
                this.extensions.Remove(name);
            }
            else
            {
                this.extensions[name] = value;
            }
        }

        /// <summary>
        /// Creates a new split and adds it to the transaction.
        /// </summary>
        /// <returns>The newly created split.</returns>
        public Split AddSplit()
        {
            var split = new Split(this);

            this.splits.Add(split);
            return split;
        }

        /// <summary>
        /// Removes a previously added split from the transaction.
        /// </summary>
        /// <param name="split">The previously added split.</param>
        public void RemoveSplit(Split split)
        {
            if (split == null)
            {
                throw new ArgumentNullException("split");
            }

            if (!this.splits.Contains(split))
            {
                throw new InvalidOperationException("Could not remove the split from the transaction, because the split is not a member of the transaction.");
            }

            this.splits.Remove(split);
            split.Transaction = null;
        }

        public Transaction Copy()
        {
            var tNew = new Transaction(this.TransactionId, this.BaseSecurity);
            tNew.SetDate(this.Date);

            foreach (var split in this.splits)
            {
                var sNew = tNew.AddSplit();
                sNew.SetAccount(split.Account);
                sNew.SetAmount(split.Amount);
                sNew.SetDateCleared(split.DateCleared);
                sNew.SetIsReconciled(split.IsReconciled);
                sNew.SetSecurity(split.Security);
                sNew.SetTransactionAmount(split.TransactionAmount);
            }

            return tNew;
        }
    }
}

// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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
        private DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <param name="baseSecurity">The security in which the values of the transaction are expressed.</param>
        public Transaction(Guid transactionId, Security baseSecurity)
        {
            if (transactionId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(transactionId));
            }

            this.BaseSecurity = baseSecurity ?? throw new ArgumentNullException(nameof(baseSecurity));
            this.TransactionId = transactionId;
            this.Date = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
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
        /// Gets or sets the date and time at which the transaction took place.
        /// </summary>
        public DateTime Date
        {
            get => this.date;

            set
            {
                if (value.Kind != DateTimeKind.Utc)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.date = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the transaction is currently considered valid.
        /// </summary>
        public bool IsValid => !this.RuleViolations.Any();

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
        /// Gets an enumerable collection of the splits that make up the transaction.
        /// </summary>
        public IList<Split> Splits => this.splits.AsReadOnly();

        /// <summary>
        /// Gets the unique identifier of the transaction.
        /// </summary>
        public Guid TransactionId
        {
            get;
            private set;
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
        /// Creates a new split and adds it to the transaction.
        /// </summary>
        /// <returns>The newly created split.</returns>
        public Split AddSplit()
        {
            var split = new Split(this);

            this.splits.Add(split);
            return split;
        }

        public Transaction Copy()
        {
            var tNew = new Transaction(this.TransactionId, this.BaseSecurity)
            {
                Date = this.Date,
            };

            foreach (var split in this.splits)
            {
                var sNew = tNew.AddSplit();
                sNew.Account = split.Account;
                sNew.Amount = split.Amount;
                sNew.DateCleared = split.DateCleared;
                sNew.IsReconciled = split.IsReconciled;
                sNew.Security = split.Security;
                sNew.TransactionAmount = split.TransactionAmount;
            }

            return tNew;
        }

        /// <summary>
        /// Removes a previously added split from the transaction.
        /// </summary>
        /// <param name="split">The previously added split.</param>
        public void RemoveSplit(Split split)
        {
            if (split == null)
            {
                throw new ArgumentNullException(nameof(split));
            }

            if (!this.splits.Contains(split))
            {
                throw new InvalidOperationException("Could not remove the split from the transaction, because the split is not a member of the transaction.");
            }

            this.splits.Remove(split);
            split.Transaction = null;
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
    }
}

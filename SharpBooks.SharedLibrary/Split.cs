// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Split
    {
        private DateTime? dateCleared;

        internal Split(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// Gets or sets the account to which the split belongs.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Gets or sets the amount by which the split affects its account.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the split cleared its account.
        /// </summary>
        public DateTime? DateCleared
        {
            get => this.dateCleared;

            set
            {
                if (value.HasValue && value.Value.Kind != DateTimeKind.Utc)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.dateCleared = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the split has been reconciled against its account.
        /// </summary>
        public bool IsReconciled { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the split is currently considered valid.
        /// </summary>
        public bool IsValid => !this.RuleViolations.Any();

        /// <summary>
        /// Gets an enumerable collection of <see cref="RuleViolation"/>s that describe features of the split that make it invalid.
        /// </summary>
        public IEnumerable<RuleViolation> RuleViolations
        {
            get
            {
                if (Math.Sign(this.Amount) != Math.Sign(this.TransactionAmount))
                {
                    yield return new RuleViolation("Amount", Localization.Localization.AMOUNT_AND_TRANSACTION_MUST_HAVE_SAME_SIGN);
                }

                if (this.Account == null)
                {
                    yield return new RuleViolation("Account", Localization.Localization.SPLIT_MUST_BE_ASSIGNED);
                }

                if (this.Account != null && this.Account.AccountType == AccountType.Grouping)
                {
                    yield return new RuleViolation("Account", Localization.Localization.SPLIT_CANNOT_BE_ASSIGNED_TO_GROUPING_ACCOUNT);
                }

                if (this.Security == null)
                {
                    yield return new RuleViolation("Security", Localization.Localization.SPLIT_MUST_HAVE_A_SECURITY);
                }

                if (this.Account != null && this.Account.Security != null && this.Account.Security != this.Security)
                {
                    yield return new RuleViolation("Security", Localization.Localization.SPLIT_SECURITY_MUST_MATCH_ACCOUNT);
                }

                if (this.Amount != this.TransactionAmount && this.Security == this.Transaction.BaseSecurity)
                {
                    yield return new RuleViolation("Amount", Localization.Localization.AMOUNT_AND_TRANSACTION_AMOUNT_MUST_BE_EQUAL);
                }

                if (this.Account != null && this.Account.SmallestFraction.HasValue && this.Amount % (this.Account.Security.FractionTraded / this.Account.SmallestFraction) != 0)
                {
                    yield return new RuleViolation("Amount", Localization.Localization.AMOUNT_OF_SPLIT_MUST_BE_DIVISIBLE);
                }

                yield break;
            }
        }

        /// <summary>
        /// Gets or sets the security of which the split is made up.
        /// </summary>
        public Security Security { get; set; }

        /// <summary>
        /// Gets or sets the transaction to which the split belongs.
        /// </summary>
        public Transaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the amount by which the split affects its transaction.
        /// </summary>
        public long TransactionAmount { get; set; }
    }
}

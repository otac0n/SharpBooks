//-----------------------------------------------------------------------
// <copyright file="Split.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
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
            this.Amount = 0;
            this.TransactionAmount = 0;
        }

        /// <summary>
        /// Gets the transaction to which the split belongs.
        /// </summary>
        public Transaction Transaction
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the date and time at which the split cleared its account.
        /// </summary>
        public DateTime? DateCleared
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the split has been reconciled against its account.
        /// </summary>
        public bool IsReconciled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the account to which the split belongs.
        /// </summary>
        public Account Account
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the security of which the split is made up.
        /// </summary>
        public Security Security
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount by which the split affects its account.
        /// </summary>
        public long Amount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount by which the split affects its transaction.
        /// </summary>
        public long TransactionAmount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not the split is currently considered valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !this.RuleViolations.Any();
            }
        }

        /// <summary>
        /// Gets an enumerable collection of <see cref="SharpBooks.RuleViolation"/>s that describe features of the split that make it invalid.
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

        public void SetSecurity(Security security, TransactionLock transactionLock)
        {
            this.Transaction.EnterCriticalSection();

            try
            {
                this.Transaction.ValidateLock(transactionLock);

                this.Security = security;
            }
            finally
            {
                this.Transaction.ExitCriticalSection();
            }
        }

        public void SetAmount(long amount, TransactionLock transactionLock)
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

        public void SetTransactionAmount(long transactionAmount, TransactionLock transactionLock)
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
            if (dateCleared.HasValue && dateCleared.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentOutOfRangeException("dateCleared");
            }

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

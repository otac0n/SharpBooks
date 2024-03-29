// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Describes an account.
    /// </summary>
    public class Account
    {
        public Account(Guid accountId, AccountType accountType, Security security, Account parentAccount, string name, int? smallestFraction)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(accountId));
            }

            if (!Enum.GetValues(typeof(AccountType)).Cast<AccountType>().Contains(accountType))
            {
                throw new ArgumentOutOfRangeException(nameof(accountType));
            }

            if (security == null && smallestFraction.HasValue)
            {
                throw new ArgumentNullException(nameof(security));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (security != null && !smallestFraction.HasValue)
            {
                throw new ArgumentNullException(nameof(smallestFraction));
            }

            if (smallestFraction <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(smallestFraction));
            }

            if (security != null)
            {
                if (security.FractionTraded % smallestFraction != 0)
                {
                    throw new InvalidOperationException(Localization.Localization.ACCOUNT_FRACTION_MUST_BE_CONGRUENT_TO_SECURITY);
                }
            }

            var parent = parentAccount;
            while (parent != null)
            {
                if (parent.AccountId == accountId)
                {
                    throw new InvalidOperationException(Localization.Localization.ACCOUNT_ID_MUST_BE_UNIQUE);
                }

                parent = parent.ParentAccount;
            }

            this.AccountId = accountId;
            this.AccountType = accountType;
            this.Security = security;
            this.ParentAccount = parentAccount;
            this.Name = name;
            this.SmallestFraction = smallestFraction;
        }

        public Guid AccountId { get; }

        public AccountType AccountType { get; }

        public CompositeBalance Balance => this.Book?.GetAccountBalance(this);

        public string Name { get; }

        public Account ParentAccount { get; }

        public Security Security { get; }

        public int? SmallestFraction { get; }

        public CompositeBalance TotalBalance => this.Book?.GetAccountTotalBalance(this);

        internal Book Book { get; set; }

        public string GetPath(string separator)
        {
            if (this.ParentAccount == null)
            {
                return this.Name;
            }

            return this.ParentAccount.GetPath(separator) + separator + this.Name;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.GetPath(new string(Path.DirectorySeparatorChar, 1));
        }
    }
}

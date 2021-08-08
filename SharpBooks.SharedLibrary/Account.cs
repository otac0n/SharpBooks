// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.IO;
    using System.Linq;

    public class Account
    {
        private readonly int? smallestFraction;

        private Book book;

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
                    throw new InvalidOperationException("An account's smallest fraction must represent a whole number multiple of the units used by its security");
                }
            }

            var parent = parentAccount;
            while (parent != null)
            {
                if (parent.AccountId == accountId)
                {
                    throw new InvalidOperationException("An account may not share an its Account Id with any of its ancestors.");
                }

                parent = parent.ParentAccount;
            }

            this.AccountId = accountId;
            this.AccountType = accountType;
            this.Security = security;
            this.ParentAccount = parentAccount;
            this.Name = name;
            this.smallestFraction = smallestFraction;
        }

        public Guid AccountId { get; }

        public AccountType AccountType { get; }

        public CompositeBalance Balance => this.book?.GetAccountBalance(this);

        public string Name { get; }

        public Account ParentAccount { get; }

        public Security Security { get; }

        public int? SmallestFraction => this.smallestFraction;

        public CompositeBalance TotalBalance => this.book?.GetAccountTotalBalance(this);

        internal Book Book
        {
            get
            {
                return this.book;
            }

            set
            {
                if (this.book != value)
                {
                    this.book = value;
                }
            }
        }

        public string GetPath(string separator)
        {
            if (this.ParentAccount == null)
            {
                return this.Name;
            }

            return this.ParentAccount.GetPath(separator) + separator + this.Name;
        }

        public override string ToString()
        {
            return this.GetPath(Path.DirectorySeparatorChar.ToString());
        }
    }
}

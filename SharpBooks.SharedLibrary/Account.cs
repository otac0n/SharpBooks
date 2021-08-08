// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.IO;
    using System.Linq;

    public class Account
    {
        private readonly Guid accountId;
        private readonly AccountType accountType;
        private readonly string name;
        private readonly Account parentAccount;
        private readonly Security security;
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

            this.accountId = accountId;
            this.accountType = accountType;
            this.security = security;
            this.parentAccount = parentAccount;
            this.name = name;
            this.smallestFraction = smallestFraction;
        }

        public Guid AccountId
        {
            get
            {
                return this.accountId;
            }
        }

        public AccountType AccountType
        {
            get
            {
                return this.accountType;
            }
        }

        public CompositeBalance Balance
        {
            get
            {
                return this.book == null
                    ? null
                    : this.book.GetAccountBalance(this);
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Account ParentAccount
        {
            get
            {
                return this.parentAccount;
            }
        }

        public Security Security
        {
            get
            {
                return this.security;
            }
        }

        public int? SmallestFraction
        {
            get
            {
                return this.smallestFraction;
            }
        }

        public CompositeBalance TotalBalance
        {
            get
            {
                return this.book == null
                    ? null
                    : this.book.GetAccountTotalBalance(this);
            }
        }

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

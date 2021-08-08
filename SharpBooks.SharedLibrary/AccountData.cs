// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Holds a read-only, persistable copy of an account.
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpBooks.AccountData" /> class, copying data from a <see cref="SharpBooks.Account" />.
        /// </summary>
        /// <param name="account">The <see cref="SharpBooks.Account" /> from which to copy.</param>
        public AccountData(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            this.AccountId = account.AccountId;
            this.AccountType = account.AccountType;
            this.ParentAccountId = account.ParentAccount == null ? (Guid?)null : account.ParentAccount.AccountId;
            this.SecurityId = account.Security == null ? (Guid?)null : account.Security.SecurityId;
            this.Name = account.Name;
            this.SmallestFraction = account.SmallestFraction;
        }

        public Guid AccountId
        {
            get;
            private set;
        }

        public AccountType AccountType
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public Guid? ParentAccountId
        {
            get;
            private set;
        }

        public Guid? SecurityId
        {
            get;
            private set;
        }

        public int? SmallestFraction
        {
            get;
            private set;
        }
    }
}

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
        /// Initializes a new instance of the <see cref="AccountData" /> class, copying data from a <see cref="Account" />.
        /// </summary>
        /// <param name="account">The <see cref="Account" /> from which to copy.</param>
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

        public Guid AccountId { get; }

        public AccountType AccountType { get; }

        public string Name { get; }

        public Guid? ParentAccountId { get; }

        public Guid? SecurityId { get; }

        public int? SmallestFraction { get; }
    }
}

//-----------------------------------------------------------------------
// <copyright file="AccountData.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
                throw new ArgumentNullException("account");
            }

            this.AccountId = account.AccountId;
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

        public string Name
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

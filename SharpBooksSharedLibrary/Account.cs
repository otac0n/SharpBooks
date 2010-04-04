//-----------------------------------------------------------------------
// <copyright file="Account.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public class Account
    {
        public Account(Guid accountId, Security security, Account parentAccount, string name)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("accountId");
            }

            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var parent = parentAccount;
            while (parent != null)
            {
                if (parent.AccountId == accountId)
                {
                    throw new InvalidOperationException("An account may not share an its AccountId with any of its ancestors.");
                }

                parent = parent.ParentAccount;
            }

            this.AccountId = accountId;
            this.Security = security;
            this.ParentAccount = parentAccount;
            this.Name = name;
        }

        public Guid AccountId
        {
            get;
            private set;
        }

        public Security Security
        {
            get;
            private set;
        }

        public Account ParentAccount
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="Account.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public class Account
    {
        public Account(Guid accountId, Security security, Account parentAccount)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("accountId");
            }

            if (security == null)
            {
                throw new ArgumentNullException("security");
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
    }
}

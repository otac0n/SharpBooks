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
        public Account(Guid accountId, Security security, Account parentAccount, string name, int smallestFraction)
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

            if (smallestFraction <= 0)
            {
                throw new ArgumentOutOfRangeException("smallestFraction");
            }

            if (security.FractionTraded % smallestFraction != 0)
            {
                throw new InvalidOperationException("An account's smallest fraction must represent a whole number multiple of the units used by its security");
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
            this.Security = security;
            this.ParentAccount = parentAccount;
            this.Name = name;
            this.SmallestFraction = smallestFraction;
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

        public int SmallestFraction
        {
            get;
            private set;
        }

        public static string GetAccountPath(Account account, string separator)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (account.ParentAccount == null)
            {
                return account.Name;
            }

            return GetAccountPath(account.ParentAccount, separator) + separator + account.Name; 
        }
    }
}

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
        public Account(Guid accountId, Commodity commodity, Account parentAccount)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("accountId");
            }

            if (commodity == null)
            {
                throw new ArgumentNullException("commodity");
            }

            this.AccountId = accountId;
            this.Commodity = commodity;
            this.ParentAccount = parentAccount;
        }

        public Guid AccountId
        {
            get;
            private set;
        }

        public Commodity Commodity
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public Account ParentAccount
        {
            get;
            set;
        }
    }
}

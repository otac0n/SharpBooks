﻿//-----------------------------------------------------------------------
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

        public Account ParentAccount
        {
            get;
            set;
        }
    }
}
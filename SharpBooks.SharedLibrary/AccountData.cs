﻿//-----------------------------------------------------------------------
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
        public AccountData(Account account)
        {
            this.AccountId = account.AccountId;
            this.ParentAccountId = account.ParentAccount == null ? (Guid?)null : account.ParentAccount.AccountId;
            this.SecurityId = account.Security.SecurityId;
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

        public Guid SecurityId
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
    }
}
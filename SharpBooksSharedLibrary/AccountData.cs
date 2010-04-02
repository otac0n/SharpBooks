//-----------------------------------------------------------------------
// <copyright file="AccountData.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public class AccountData
    {
        public AccountData(Account account)
        {
            this.AccountId = account.AccountId;
            this.ParentAccountId = account.ParentAccount == null ? (Guid?)null : account.ParentAccount.AccountId;
            this.SecurityType = account.Security.SecurityType;
            this.SecuritySymbol = account.Security.Symbol;
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

        public SecurityType SecurityType
        {
            get;
            private set;
        }

        public string SecuritySymbol
        {
            get;
            private set;
        }
    }
}

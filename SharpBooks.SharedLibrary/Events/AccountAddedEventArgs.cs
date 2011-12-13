//-----------------------------------------------------------------------
// <copyright file="AccountAddedEventArgs.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Events
{
    using System;

    public class AccountAddedEventArgs : EventArgs
    {
        private readonly Account account;

        public AccountAddedEventArgs(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            this.account = account;
        }

        public Account Account
        {
            get
            {
                return this.account;
            }
        }
    }
}

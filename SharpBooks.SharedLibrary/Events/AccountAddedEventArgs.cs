// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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

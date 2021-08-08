// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class AccountRemovedEventArgs : EventArgs
    {
        private readonly Account account;

        public AccountRemovedEventArgs(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
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

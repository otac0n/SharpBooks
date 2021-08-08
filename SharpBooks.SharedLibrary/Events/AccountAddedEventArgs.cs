// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class AccountAddedEventArgs : EventArgs
    {
        public AccountAddedEventArgs(Account account)
        {
            this.Account = account ?? throw new ArgumentNullException(nameof(account));
        }

        public Account Account { get; }
    }
}

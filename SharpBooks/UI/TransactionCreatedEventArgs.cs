// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;

    public class TransactionCreatedEventArgs : EventArgs
    {
        public TransactionCreatedEventArgs(Transaction newTransaction)
        {
            this.NewTransaction = newTransaction ?? throw new ArgumentNullException(nameof(newTransaction));
        }

        public Transaction NewTransaction { get; }
    }
}

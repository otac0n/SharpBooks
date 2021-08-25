// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;

    public class TransactionUpdatedEventArgs : EventArgs
    {
        public TransactionUpdatedEventArgs(Transaction oldTransaction, Transaction newTransaction)
        {
            this.OldTransaction = oldTransaction ?? throw new ArgumentNullException(nameof(oldTransaction));
            this.NewTransaction = newTransaction ?? throw new ArgumentNullException(nameof(newTransaction));
        }

        public Transaction NewTransaction { get; }

        public Transaction OldTransaction { get; }
    }
}

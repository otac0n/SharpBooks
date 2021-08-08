// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class TransactionAddedEventArgs : EventArgs
    {
        public TransactionAddedEventArgs(Transaction transaction)
        {
            this.Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        public Transaction Transaction { get; }
    }
}

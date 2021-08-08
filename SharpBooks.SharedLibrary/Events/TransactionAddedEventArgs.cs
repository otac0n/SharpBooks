// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class TransactionAddedEventArgs : EventArgs
    {
        public TransactionAddedEventArgs(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            this.Transaction = transaction;
        }

        public Transaction Transaction { get; }
    }
}

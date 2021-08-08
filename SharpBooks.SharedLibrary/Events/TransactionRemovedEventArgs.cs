// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class TransactionRemovedEventArgs : EventArgs
    {
        private readonly Transaction transaction;

        public TransactionRemovedEventArgs(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            this.transaction = transaction;
        }

        public Transaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }
    }
}

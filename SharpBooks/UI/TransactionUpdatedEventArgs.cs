// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;

    public class TransactionUpdatedEventArgs : EventArgs
    {
        private readonly Transaction newTransaction;
        private readonly Transaction oldTransaction;

        public TransactionUpdatedEventArgs(Transaction oldTransaction, Transaction newTransaction)
        {
            if (oldTransaction == null)
            {
                throw new ArgumentNullException(nameof(oldTransaction));
            }

            if (newTransaction == null)
            {
                throw new ArgumentNullException(nameof(newTransaction));
            }

            this.oldTransaction = oldTransaction;
            this.newTransaction = newTransaction;
        }

        public Transaction NewTransaction
        {
            get { return this.newTransaction; }
        }

        public Transaction OldTransaction
        {
            get { return this.oldTransaction; }
        }
    }
}

// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;

    public class TransactionCreatedEventArgs : EventArgs
    {
        private readonly Transaction newTransaction;

        public TransactionCreatedEventArgs(Transaction newTransaction)
        {
            if (newTransaction == null)
            {
                throw new ArgumentNullException("newTransaction");
            }

            this.newTransaction = newTransaction;
        }

        public Transaction NewTransaction
        {
            get { return this.newTransaction; }
        }
    }
}

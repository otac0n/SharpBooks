//-----------------------------------------------------------------------
// <copyright file="TransactionUpdatedEventArgs.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
                throw new ArgumentNullException("oldTransaction");
            }

            if (newTransaction == null)
            {
                throw new ArgumentNullException("newTransaction");
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

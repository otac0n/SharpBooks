//-----------------------------------------------------------------------
// <copyright file="TransactionUpdatedEventArgs.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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

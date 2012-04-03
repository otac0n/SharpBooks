//-----------------------------------------------------------------------
// <copyright file="TransactionRemovedEventArgs.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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

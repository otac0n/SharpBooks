//-----------------------------------------------------------------------
// <copyright file="TransactionAddedEventArgs.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Events
{
    using System;

    public class TransactionAddedEventArgs : EventArgs
    {
        private readonly Transaction transaction;

        public TransactionAddedEventArgs(Transaction transaction)
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

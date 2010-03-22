//-----------------------------------------------------------------------
// <copyright file="TransactionLock.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public class TransactionLock : IDisposable
    {
        internal TransactionLock(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public Transaction Transaction
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (this.Transaction == null)
            {
                return;
            }

            this.Transaction.Unlock(this);
            this.Transaction = null;
        }
    }
}

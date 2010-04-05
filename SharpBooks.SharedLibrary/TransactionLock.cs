//-----------------------------------------------------------------------
// <copyright file="TransactionLock.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Acts as a token of permission to edit a transaction.
    /// </summary>
    public sealed class TransactionLock : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpBooks.TransactionLock"/> class.
        /// </summary>
        /// <param name="transaction">The <see cref="SharpBooks.Transaction" /> to which the lock belongs.</param>
        internal TransactionLock(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// Gets the <see cref="SharpBooks.Transaction"/> to which the lock belongs.
        /// </summary>
        public Transaction Transaction
        {
            get;
            private set;
        }

        /// <summary>
        /// Disposes of the lock.
        /// </summary>
        /// <remarks>
        /// Calling dispose on the <see cref="SharpBooks.TransactionLock"/> automatically unlocks the <see cref="SharpBooks.Transaction" /> to which it belongs.
        /// </remarks>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the lock on the associated <see cref="SharpBooks.Transaction" />.
        /// </summary>
        /// <param name="disposing">Indicates whether or not to dispose of managed resources.  Pass true to dispose managed and unmanaged resources, or false to just dispose unmanaged ones.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Transaction != null)
                {
                    this.Transaction.Unlock(this);
                    this.Transaction = null;
                }
            }
        }
    }
}

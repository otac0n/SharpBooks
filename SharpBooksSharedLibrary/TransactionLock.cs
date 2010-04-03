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
            if (this.Transaction == null)
            {
                return;
            }

            this.Transaction.Unlock(this);
            this.Transaction = null;
        }
    }
}

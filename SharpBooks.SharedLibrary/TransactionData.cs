//-----------------------------------------------------------------------
// <copyright file="TransactionData.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Holds a read-only, persistable copy of a transaction.
    /// </summary>
    public sealed class TransactionData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionData"/> class.
        /// </summary>
        /// <param name="transaction">The <see cref="Transaction"/> from which to copy.</param>
        public TransactionData(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            this.TransactionId = transaction.TransactionId;
            this.Date = transaction.Date;
            this.BaseSecurityId = transaction.BaseSecurity.SecurityId;
            this.Splits = (from s in transaction.Splits
                           select new SplitData(s)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the ID of the transaction's base security.
        /// </summary>
        public Guid BaseSecurityId { get; }

        /// <summary>
        /// Gets the point in time at which the transaction took place.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets a read-only list of the splits that make up the transaction.
        /// </summary>
        public IList<SplitData> Splits { get; }

        /// <summary>
        /// Gets the transaction's ID.
        /// </summary>
        public Guid TransactionId { get; }
    }
}

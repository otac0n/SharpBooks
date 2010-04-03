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
        /// Initializes a new insance of the <see cref="SharpBooks.TransactionData"/> class.
        /// </summary>
        /// <param name="transaction">The <see cref="SharpBooks.Transaction"/> from which to copy.</param>
        public TransactionData(Transaction transaction)
        {
            this.TransactionId = transaction.TransactionId;
            this.Date = transaction.Date;
            this.BaseSecurityId = transaction.BaseSecurity.SecurityId;
            this.Splits = (from s in transaction.Splits
                           select new SplitData(s)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets this transaction's Transaction Id.
        /// </summary>
        public Guid TransactionId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the point in time at which this transactioin took place.
        /// </summary>
        public DateTime Date
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Security Id of this transaction's base security.
        /// </summary>
        public Guid BaseSecurityId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a read-only list of the splits that make up this transaction.
        /// </summary>
        public IEnumerable<SplitData> Splits
        {
            get;
            private set;
        }
    }
}

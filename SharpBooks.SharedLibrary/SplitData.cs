﻿//-----------------------------------------------------------------------
// <copyright file="SplitData.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Holds a read-only, persistable copy of a split.
    /// </summary>
    public sealed class SplitData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpBooks.SplitData"/> class.
        /// </summary>
        /// <param name="split">The <see cref="SharpBooks.Split"/> from which to copy.</param>
        public SplitData(Split split)
        {
            if (split == null)
            {
                throw new ArgumentNullException("split");
            }

            this.TransactionId = split.Transaction.TransactionId;
            this.AccountId = split.Account.AccountId;
            this.SecurityId = split.Security.SecurityId;
            this.Amount = split.Amount;
            this.DateCleared = split.DateCleared;
            this.IsReconciled = split.IsReconciled;
            this.TransactionAmount = split.TransactionAmount;
        }

        /// <summary>
        /// Gets the Id of the transaction to which the split belongs.
        /// </summary>
        public Guid TransactionId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the Id of the account to which the split belongs.
        /// </summary>
        public Guid AccountId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Id of the security of which the split is made up.
        /// </summary>
        public Guid SecurityId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date and time at which the split cleared its account.
        /// </summary>
        public DateTime? DateCleared
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the split has been reconciled against its account.
        /// </summary>
        public bool IsReconciled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount by which the split affects its account.
        /// </summary>
        public long Amount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount by which the split affects its transaction.
        /// </summary>
        public long TransactionAmount
        {
            get;
            private set;
        }
    }
}

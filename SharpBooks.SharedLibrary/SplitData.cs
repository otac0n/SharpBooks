//-----------------------------------------------------------------------
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
        public SplitData(Split split)
        {
            this.TransactionId = split.Transaction.TransactionId;
            this.AccountId = split.Account.AccountId;
            this.Amount = split.Amount;
            this.DateCleared = split.DateCleared;
            this.IsReconciled = split.IsReconciled;
            this.TransactionAmount = split.TransactionAmount;
        }

        public Guid TransactionId
        {
            get;
            internal set;
        }

        public DateTime? DateCleared
        {
            get;
            private set;
        }

        public bool IsReconciled
        {
            get;
            private set;
        }

        public Guid AccountId
        {
            get;
            private set;
        }

        public long Amount
        {
            get;
            private set;
        }

        public long TransactionAmount
        {
            get;
            private set;
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="TransactionData.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TransactionData
    {
        public TransactionData(Transaction transaction)
        {
            this.TransactionId = transaction.TransactionId;
            this.Date = transaction.Date;
            this.BaseSecurityType = transaction.BaseSecurity.SecurityType;
            this.BaseSecuritySymbol = transaction.BaseSecurity.Symbol;
            this.Splits = (from s in transaction.Splits
                           select new SplitData(s)).ToList().AsReadOnly();
        }

        public Guid TransactionId
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get;
            private set;
        }

        public SecurityType BaseSecurityType
        {
            get;
            private set;
        }

        public string BaseSecuritySymbol
        {
            get;
            private set;
        }

        public IEnumerable<SplitData> Splits
        {
            get;
            private set;
        }
    }
}

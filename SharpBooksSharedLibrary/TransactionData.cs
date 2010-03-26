//-----------------------------------------------------------------------
// <copyright file="TransactionData.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TransactionData
    {
        public TransactionData(Transaction transaction)
        {
            
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

        public Guid BaseSecurityId
        {
            get;
            private set;
        }
    }
}

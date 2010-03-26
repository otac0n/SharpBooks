//-----------------------------------------------------------------------
// <copyright file="TransactionData.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
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

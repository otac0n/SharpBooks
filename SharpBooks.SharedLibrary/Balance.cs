//-----------------------------------------------------------------------
// <copyright file="Balance.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public sealed class Balance
    {
        private readonly Security security;
        private readonly long amount;
        private readonly bool isExact;

        public Balance(Security security, long amount, bool isExact)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            this.security = security;
            this.amount = amount;
            this.isExact = isExact;
        }

        public Security Security
        {
            get
            {
                return this.security;
            }
        }

        public long Amount
        {
            get
            {
                return this.amount;
            }
        }

        public bool IsExact
        {
            get
            {
                return this.isExact;
            }
        }

        public override string ToString()
        {
            return (this.isExact ? string.Empty : "\u2248") + this.security.FormatValue(this.amount);
        }
    }
}

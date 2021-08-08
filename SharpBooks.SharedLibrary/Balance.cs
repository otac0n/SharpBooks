// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    public sealed class Balance
    {
        private readonly long amount;
        private readonly bool isExact;
        private readonly Security security;

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

        public Security Security
        {
            get
            {
                return this.security;
            }
        }

        public override string ToString()
        {
            return (this.isExact ? string.Empty : "\u2248") + this.security.FormatValue(this.amount);
        }
    }
}

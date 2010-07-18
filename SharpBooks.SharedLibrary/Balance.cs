using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks
{
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

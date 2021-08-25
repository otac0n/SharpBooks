// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    public sealed class Balance
    {
        public Balance(Security security, long amount, bool isExact)
        {
            this.Security = security ?? throw new ArgumentNullException(nameof(security));
            this.Amount = amount;
            this.IsExact = isExact;
        }

        public long Amount { get; }

        public bool IsExact { get; }

        public Security Security { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return (this.IsExact ? string.Empty : "\u2248") + this.Security.FormatValue(this.Amount);
        }
    }
}

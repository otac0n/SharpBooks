// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Holds a read-only, persistable copy of a security.
    /// </summary>
    public sealed class SecurityData
    {
        public SecurityData(Security security)
        {
            if (security == null)
            {
                throw new ArgumentNullException(nameof(security));
            }

            this.SecurityId = security.SecurityId;
            this.SecurityType = security.SecurityType;
            this.Symbol = security.Symbol;
            this.Name = security.Name;
            this.Format = security.Format;
            this.FractionTraded = security.FractionTraded;
        }

        public CurrencyFormat Format
        {
            get;
            private set;
        }

        public int FractionTraded
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public Guid SecurityId
        {
            get;
            private set;
        }

        public SecurityType SecurityType
        {
            get;
            private set;
        }

        public string Symbol
        {
            get;
            private set;
        }
    }
}

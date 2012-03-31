//-----------------------------------------------------------------------
// <copyright file="Security.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "'Security' is a standard term for the financial instrument represented by this class.")]
    public sealed class Security
    {
        public Security(Guid securityId, SecurityType securityType, string name, string symbol, CurrencyFormat format, int fractionTraded)
        {
            if (securityId == Guid.Empty)
            {
                throw new ArgumentNullException("securityId");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException("symbol");
            }

            if (format == null)
            {
                throw new ArgumentNullException("signFormat");
            }

            if (fractionTraded <= 0)
            {
                throw new ArgumentOutOfRangeException("fractionTraded", "The fraction traded must be greater than or equal to one.");
            }

            this.SecurityId = securityId;
            this.SecurityType = securityType;
            this.Name = name;
            this.Symbol = symbol;
            this.Format = format;
            this.FractionTraded = fractionTraded;
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

        public string Name
        {
            get;
            private set;
        }

        public string Symbol
        {
            get;
            private set;
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

        public string FormatValue(long value)
        {
            return this.Format.Format(value, this.FractionTraded);
        }
    }
}

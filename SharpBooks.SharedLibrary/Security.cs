// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "'Security' is a standard term for the financial instrument represented by this class.")]
    public sealed class Security
    {
        public Security(Guid securityId, SecurityType securityType, string name, string symbol, CurrencyFormat format, int fractionTraded)
        {
            if (securityId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(securityId));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            if (fractionTraded <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fractionTraded), Localization.Localization.SECURITY_FRACTION_TRADED_MUST_BE_POSITIVE);
            }

            this.SecurityId = securityId;
            this.SecurityType = securityType;
            this.Name = name;
            this.Symbol = symbol;
            this.Format = format ?? throw new ArgumentNullException(nameof(format));
            this.FractionTraded = fractionTraded;
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

        public string FormatValue(long value)
        {
            return this.Format.Format(value, this.FractionTraded);
        }

        public long ParseValue(string s)
        {
            if (!this.TryParseValue(s, out var amount))
            {
                throw new FormatException("The value was not in an acceptable format.");
            }

            return amount;
        }

        public bool TryParseValue(string s, out long result)
        {
            return this.Format.TryParse(s, this.FractionTraded, out result);
        }
    }
}

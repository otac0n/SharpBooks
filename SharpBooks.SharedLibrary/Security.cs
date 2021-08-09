// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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

        public CurrencyFormat Format { get; }

        public int FractionTraded { get; }

        public string Name { get; }

        public Guid SecurityId { get; }

        public SecurityType SecurityType { get; }

        public string Symbol { get; }

        public string FormatValue(long value)
        {
            return this.Format.Format(value, this.FractionTraded);
        }

        public long ParseValue(string s)
        {
            if (!this.TryParseValue(s, out var amount))
            {
                throw new FormatException(Localization.Localization.INVALID_FORMAT);
            }

            return amount;
        }

        public bool TryParseValue(string s, out long result)
        {
            return this.Format.TryParse(s, this.FractionTraded, out result);
        }
    }
}

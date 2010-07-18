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
        public Security(Guid securityId, SecurityType securityType, string name, string symbol, string signFormat, int fractionTraded)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException("symbol");
            }

            if (string.IsNullOrEmpty(signFormat))
            {
                throw new ArgumentNullException("signFormat");
            }

            if (!ValidateSignFormat(signFormat))
            {
                throw new ArgumentException("The sign format of a security must include the dollar amount (i.e. '{0}')  as part of the format string.", "signFormat");
            }

            if (fractionTraded <= 0)
            {
                throw new ArgumentOutOfRangeException("fractionTraded", "The fraction traded must be greateder than or equal to one.");
            }

            this.SecurityId = securityId;
            this.SecurityType = securityType;
            this.Name = name;
            this.Symbol = symbol;
            this.SignFormat = signFormat;
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

        public string SignFormat
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
            decimal acutalValue = (decimal)value / (decimal)this.FractionTraded;

            return string.Format(CultureInfo.InvariantCulture, this.SignFormat, acutalValue);
        }

        private static bool ValidateSignFormat(string signFormat)
        {
            if (string.IsNullOrEmpty(signFormat) ||
                !signFormat.Contains("{") ||
                signFormat.Contains("{{"))
            {
                return false;
            }

            string result;

            try
            {
                result = string.Format(CultureInfo.InvariantCulture, signFormat, (decimal)1);
            }
            catch (FormatException)
            {
                return false;
            }

            return !string.IsNullOrEmpty(result);
        }
    }
}

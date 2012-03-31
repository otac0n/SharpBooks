//-----------------------------------------------------------------------
// <copyright file="Security.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public enum PositiveFormat
    {
        Prefix,
        Suffix,
        PrefixSpaced,
        SuffixSpaced,
    }

    public enum NegativeFormat
    {
        Parenthesis,
        Prefix,
        Infix,
        Suffix,
    }

    public class CurrencyFormat
    {
        private static readonly Dictionary<PositiveFormat, int> positivePatternMapping = new Dictionary<PositiveFormat, int>
        {
            { PositiveFormat.Prefix, 0 },
            { PositiveFormat.Suffix, 1 },
            { PositiveFormat.PrefixSpaced, 2 },
            { PositiveFormat.SuffixSpaced, 3 },
        };

        private static readonly Dictionary<Tuple<PositiveFormat, NegativeFormat>, int> negativePatternMapping = new Dictionary<Tuple<PositiveFormat, NegativeFormat>, int>
        {
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Parenthesis), 0 },
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Prefix),      1 },
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Infix),       2 },
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Suffix),      3 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Parenthesis), 4 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Prefix),      5 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Infix),       6 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Suffix),      7 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Parenthesis), 14 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Prefix),      9 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Infix),       12 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Suffix),      11 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Parenthesis), 15 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Prefix),      8 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Infix),       13 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Suffix),      10 },
        };

        private readonly int decimalDigits;
        private readonly string decimalSeparator;
        private readonly string groupSeparator;
        private readonly int[] groupSizes;
        private readonly string currencySymbol;
        private readonly PositiveFormat positiveFormat;
        private readonly NegativeFormat negativeFormat;

        private readonly NumberFormatInfo numberFormatInfo;

        public CurrencyFormat(
            int decimalDigits = 2,
            string decimalSeparator = ".",
            string groupSeparator = ",",
            IEnumerable<int> groupSizes = null,
            string currencySymbol = "",
            PositiveFormat positiveFormat = PositiveFormat.Prefix,
            NegativeFormat negativeFormat = NegativeFormat.Parenthesis)
        {
            if (decimalDigits < 0 || decimalDigits > 99)
            {
                throw new ArgumentOutOfRangeException("decimalDigits");
            }

            if (string.IsNullOrEmpty(decimalSeparator))
            {
                throw new ArgumentNullException("decimalSeparator");
            }

            if (string.IsNullOrEmpty(groupSeparator))
            {
                throw new ArgumentNullException("groupSeparator");
            }

            var copy = (groupSizes ?? new[] { 3 }).ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                var v = copy[i];
                if (v < 0 || (v == 0 && i != copy.Length - 1))
                {
                    throw new ArgumentOutOfRangeException("groupSizes");
                }
            }

            if (currencySymbol == null)
            {
                throw new ArgumentNullException("symbol");
            }

            this.decimalDigits = decimalDigits;
            this.decimalSeparator = decimalSeparator;
            this.groupSeparator = groupSeparator;
            this.groupSizes = copy;
            this.positiveFormat = positiveFormat;
            this.negativeFormat = negativeFormat;
            this.currencySymbol = currencySymbol;

            this.numberFormatInfo = new NumberFormatInfo
            {
                CurrencyDecimalDigits = this.decimalDigits,
                CurrencyDecimalSeparator = this.decimalSeparator,
                CurrencyGroupSeparator = this.groupSeparator,
                CurrencyGroupSizes = this.groupSizes,
                CurrencyPositivePattern = positivePatternMapping[this.positiveFormat],
                CurrencyNegativePattern = negativePatternMapping[Tuple.Create(this.positiveFormat, this.negativeFormat)],
                CurrencySymbol = this.currencySymbol,
            };
        }

        public int DecimalDigits
        {
            get { return this.decimalDigits; }
        }

        public string DecimalSeparator
        {
            get { return this.decimalSeparator; }
        }

        public string GroupSeparator
        {
            get { return this.groupSeparator; }
        }

        public int[] GroupSizes
        {
            get { return this.groupSizes.ToArray(); }
        }

        public PositiveFormat PositiveFormat
        {
            get { return this.positiveFormat; }
        }

        public NegativeFormat NegativeFormat
        {
            get { return this.negativeFormat; }
        }

        public string Symbol
        {
            get { return this.currencySymbol; }
        }

        public string Format(long value, int fractionTraded)
        {
            decimal actualValue = (decimal)value / (decimal)fractionTraded;

            return actualValue.ToString("C", this.numberFormatInfo);
        }
    }
}

// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public enum NegativeFormat
    {
        Parentheses,
        Prefix,
        Infix,
        Suffix,
    }

    public enum PositiveFormat
    {
        Prefix,
        Suffix,
        PrefixSpaced,
        SuffixSpaced,
    }

    public class CurrencyFormat
    {
        private static readonly Dictionary<Tuple<PositiveFormat, NegativeFormat>, int> negativePatternMapping = new Dictionary<Tuple<PositiveFormat, NegativeFormat>, int>
        {
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Parentheses), 0 },
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Prefix),      1 },
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Infix),       2 },
            { Tuple.Create(PositiveFormat.Prefix,       NegativeFormat.Suffix),      3 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Parentheses), 4 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Prefix),      5 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Infix),       6 },
            { Tuple.Create(PositiveFormat.Suffix,       NegativeFormat.Suffix),      7 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Parentheses), 14 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Prefix),      9 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Infix),       12 },
            { Tuple.Create(PositiveFormat.PrefixSpaced, NegativeFormat.Suffix),      11 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Parentheses), 15 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Prefix),      8 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Infix),       13 },
            { Tuple.Create(PositiveFormat.SuffixSpaced, NegativeFormat.Suffix),      10 },
        };

        private static readonly Dictionary<PositiveFormat, int> positivePatternMapping = new Dictionary<PositiveFormat, int>
        {
            { PositiveFormat.Prefix, 0 },
            { PositiveFormat.Suffix, 1 },
            { PositiveFormat.PrefixSpaced, 2 },
            { PositiveFormat.SuffixSpaced, 3 },
        };

        private readonly string currencySymbol;
        private readonly int decimalDigits;
        private readonly string decimalSeparator;
        private readonly string groupSeparator;
        private readonly int[] groupSizes;
        private readonly NegativeFormat negativeFormat;
        private readonly NumberFormatInfo numberFormatInfo;
        private readonly PositiveFormat positiveFormat;

        public CurrencyFormat(
            int decimalDigits = 2,
            string decimalSeparator = ".",
            string groupSeparator = ",",
            IEnumerable<int> groupSizes = null,
            string currencySymbol = "",
            PositiveFormat positiveFormat = PositiveFormat.Prefix,
            NegativeFormat negativeFormat = NegativeFormat.Parentheses)
        {
            if (decimalDigits < 0 || decimalDigits > 99)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalDigits));
            }

            if (string.IsNullOrEmpty(decimalSeparator))
            {
                throw new ArgumentNullException(nameof(decimalSeparator));
            }

            if (string.IsNullOrEmpty(groupSeparator))
            {
                throw new ArgumentNullException(nameof(groupSeparator));
            }

            var copy = (groupSizes ?? new[] { 3 }).ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                var v = copy[i];
                if (v < 0 || (v == 0 && i != copy.Length - 1))
                {
                    throw new ArgumentOutOfRangeException(nameof(groupSizes));
                }
            }

            if (currencySymbol == null)
            {
                throw new ArgumentNullException(nameof(currencySymbol));
            }

            if (!Enum.GetValues(typeof(PositiveFormat)).Cast<PositiveFormat>().Contains(positiveFormat))
            {
                throw new ArgumentOutOfRangeException(nameof(positiveFormat));
            }

            if (!Enum.GetValues(typeof(NegativeFormat)).Cast<NegativeFormat>().Contains(negativeFormat))
            {
                throw new ArgumentOutOfRangeException(nameof(negativeFormat));
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

                NumberDecimalDigits = this.DecimalDigits,
                NumberDecimalSeparator = this.decimalSeparator,
                NumberGroupSeparator = this.groupSeparator,
                NumberGroupSizes = this.groupSizes,
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

        public NegativeFormat NegativeFormat
        {
            get { return this.negativeFormat; }
        }

        public PositiveFormat PositiveFormat
        {
            get { return this.positiveFormat; }
        }

        public string Symbol
        {
            get { return this.currencySymbol; }
        }

        public string Format(long value, int fractionTraded)
        {
            decimal actualValue = (decimal)value / (decimal)fractionTraded;

            int count = CountDecimalDigits(actualValue);
            count = Math.Max(
                this.numberFormatInfo.CurrencyDecimalDigits,
                count);

            return actualValue.ToString("C" + count, this.numberFormatInfo);
        }

        public bool TryParse(string s, int fractionTraded, out long result)
        {
            result = 0;
            if (string.IsNullOrWhiteSpace(s))
            {
                return true;
            }

            decimal actualValue;
            if (!decimal.TryParse(s, NumberStyles.Currency, this.numberFormatInfo, out actualValue))
            {
                return false;
            }

            var value = actualValue * fractionTraded;
            if (value != decimal.Truncate(value))
            {
                return false;
            }

            result = (long)value;
            return true;
        }

        private static int CountDecimalDigits(decimal value)
        {
            int count = 0;
            while (decimal.Truncate(value) != value)
            {
                value *= 10;
                count++;
            }

            return count;
        }
    }
}

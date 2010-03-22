//-----------------------------------------------------------------------
// <copyright file="Commodity.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public class Commodity
    {
        public Commodity(CommodityType commodityType, string name, string symbol, string signFormat)
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
                throw new ArgumentException("signFormat");
            }

            this.CommodityType = commodityType;
            this.Name = name;
            this.Symbol = symbol;
            this.SignFormat = signFormat;
        }

        public CommodityType CommodityType
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

        private static bool ValidateSignFormat(string signFormat)
        {
            if (string.IsNullOrEmpty(signFormat) ||
                !signFormat.Contains("{0}") ||
                signFormat.Contains("{{0}}"))
            {
                return false;
            }

            try
            {
                string.Format(signFormat, (decimal)1);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}

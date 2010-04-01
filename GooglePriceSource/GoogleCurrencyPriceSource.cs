﻿//-----------------------------------------------------------------------
// <copyright file="GooglePriceSource.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace GooglePriceSource
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using SharpBooks;
    using SharpBooks.Plugins;
    using Newtonsoft.Json;
    using System.Text.RegularExpressions;

    public class GoogleCurrencyPriceSource : IPriceQuoteSource
    {
        private const string UrlFormat = "http://www.google.com/ig/calculator?hl=en&q=1+{0}+in+{1}";

        public string Name
        {
            get
            {
                return "Google™ Calculator Price Quotes";
            }
        }

        public PriceQuote GetPriceQuote(Security security, Security currecny)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            if (currecny == null)
            {
                throw new ArgumentNullException("currecny");
            }

            if (currecny.SecurityType != SecurityType.Currency)
            {
                throw new ArgumentException("The argument must be a Security with a SecurityType of Currency", "currency");
            }

            if (security.SecurityType != SecurityType.Currency)
            {
                throw BuildError(security.Symbol, "Only currencies are supported.");
            }

            var client = new WebClient();
            string data;

            try
            {
                data = client.DownloadString(string.Format(UrlFormat, security.Symbol, currecny.Symbol));
            }
            catch (WebException ex)
            {
                throw BuildError(security.Symbol + currecny.Symbol, "Check the inner exception for details.", ex);
            }

            var result = JsonConvert.DeserializeObject<CalculatorResult>(data);

            if (!string.IsNullOrEmpty(result.err))
            {
                throw BuildError(security.Symbol + currecny.Symbol, "Google™ calculator returned the following error: Invalid calculator expression (" + result.err + ")");
            }

            var match = Regex.Match(result.rhs, @"^\d+\.\d+");
            decimal price;

            if (!match.Success || !decimal.TryParse(match.Value, out price))
            {
                throw BuildError(security.Symbol + currecny.Symbol, "The data returned was not in a recognized format.");
            }

            var dateTime = DateTime.Now.AddDays(-1).Date;

            checked
            {
                var quantity = (long)security.FractionTraded;
                price *= currecny.FractionTraded;

                var longPrice = (long)Math.Floor(price);

                while (longPrice != price)
                {
                    price *= 10;
                    quantity *= 10;
                    longPrice = (long)Math.Floor(price);
                }

                var gcd = My.Math.GCD(longPrice, quantity / security.FractionTraded);

                quantity /= gcd;
                longPrice /= gcd;

                return new PriceQuote(dateTime, security, quantity, currecny, longPrice, "Google™ Calculator");
            }
        }

        private static Exception BuildError(string symbol, string error)
        {
            return BuildError(symbol, error, null);
        }

        private static Exception BuildError(string symbol, string error, Exception innerException)
        {
            return new PriceRetrievalFailureException("Could not download the Google™ price data for the symbol '" + symbol + "'.  " + error, innerException);
        }

        private class CalculatorResult
        {
            public string lhs
            {
                get;
                set;
            }

            public string rhs
            {
                get;
                set;
            }

            public string err
            {
                get;
                set;
            }

            public bool icc
            {
                get;
                set;
            }
        }
    }
}
﻿//-----------------------------------------------------------------------
// <copyright file="GoogleCurrencyPriceQuoteSource.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace GooglePriceQuoteSource
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using SharpBooks;

    /// <summary>
    /// Provides functionality to download price quote data from Google™ Calculator.
    /// </summary>
    public class GoogleCurrencyPriceQuoteSource : IPriceQuoteSource
    {
        private const string UrlFormat = "http://www.google.com/ig/calculator?hl=en&q=1+{0}+in+{1}";

        /// <summary>
        /// Retrieves a price quote from Google™ Calculator.
        /// </summary>
        /// <param name="security">The security for which to get the quote.</param>
        /// <param name="currency">The currency in which to express the quote.</param>
        /// <returns>The requested price quote.</returns>
        public PriceQuote GetPriceQuote(Security security, Security currency)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }

            if (currency.SecurityType != SecurityType.Currency)
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
                data = client.DownloadString(string.Format(UrlFormat, security.Symbol, currency.Symbol));
            }
            catch (WebException ex)
            {
                throw BuildError(security.Symbol + currency.Symbol, "Check the inner exception for details.", ex);
            }

            var result = JsonConvert.DeserializeObject<CalculatorResult>(data);

            if (!string.IsNullOrEmpty(result.err))
            {
                throw BuildError(security.Symbol + currency.Symbol, "Google™ calculator returned the following error: Invalid calculator expression (" + result.err + ")");
            }

            var match = Regex.Match(result.rhs, @"^\d+\.\d+");
            decimal price;

            if (!match.Success || !decimal.TryParse(match.Value, out price))
            {
                throw BuildError(security.Symbol + currency.Symbol, "The data returned was not in a recognized format.");
            }

            checked
            {
                var quantity = (long)security.FractionTraded;
                price *= currency.FractionTraded;

                var longPrice = (long)Math.Floor(price);

                while (longPrice != price)
                {
                    price *= 10;
                    quantity *= 10;
                    longPrice = (long)Math.Floor(price);
                }

                var gcd = FinancialMath.GCD(longPrice, quantity / security.FractionTraded);

                quantity /= gcd;
                longPrice /= gcd;

                return new PriceQuote(Guid.NewGuid(), DateTime.UtcNow, security, quantity, currency, longPrice, "Google™ Calculator");
            }
        }

        /// <summary>
        /// Performs plugin-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        private static Exception BuildError(string symbol, string error)
        {
            return BuildError(symbol, error, null);
        }

        private static Exception BuildError(string symbol, string error, Exception innerException)
        {
            return new PriceRetrievalFailureException("Could not download the Google™ price data for the symbol '" + symbol + "'.  " + error, innerException);
        }

        /// <summary>
        /// Encapsulates a result from Google™ Calculator.
        /// </summary>
        private class CalculatorResult
        {
            /// <summary>
            /// Gets or sets the left-hand-side of the equation.
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Necessary for JSON deserialization.")]
            public string lhs
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the right-hand-side of the equation.
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Necessary for JSON deserialization.")]
            public string rhs
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the error result from the query.
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Necessary for JSON deserialization.")]
            public string err
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether this result code is unknown.
            /// </summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Necessary for JSON deserialization.")]
            public bool icc
            {
                get;
                set;
            }
        }
    }
}
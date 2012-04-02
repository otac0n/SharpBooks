//-----------------------------------------------------------------------
// <copyright file="GoogleCurrencyPriceQuoteSource.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace GooglePriceQuoteSource
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using SharpBooks;

    /// <summary>
    /// Provides functionality to download price quote data from Google™ Calculator.
    /// </summary>
    public class GoogleCurrencyPriceQuoteSource : IPriceQuoteSource
    {
        private const string UrlFormat = "https://www.google.com/finance/getprices?q={0}{1}&x=CURRENCY&i=1&p=1d";

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

            DateTimeOffset date;
            decimal price;
            try
            {
                var result = Parse(data).Last();
                date = result.Item1;
                price = result.Item2;
            }
            catch (InvalidOperationException ex)
            {
                throw BuildError(security.Symbol + currency.Symbol, "Check the inner exception for details.", ex);
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

                return new PriceQuote(Guid.NewGuid(), date.UtcDateTime, security, quantity, currency, longPrice, "Google™ Calculator");
            }
        }

        public static IEnumerable<Tuple<DateTimeOffset, decimal>> Parse(string data)
        {
            var lines = data.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries);

            var headers = new Dictionary<string, string>();

            int i;
            for (i = 0; i < lines.Length; i++)
            {
                int width = 1;
                var index = lines[i].IndexOf("=");
                if (index == -1)
                {
                    width = 3;
                    index = lines[i].IndexOf("%3D");
                }

                if (index == -1)
                {
                    break;
                }

                headers.Add(lines[i].Substring(0, index), lines[i].Substring(index + width));
            }

            var columns = headers["COLUMNS"]
                .Split(',')
                .Select((col, ix) => new { col, ix })
                .ToDictionary(e => e.col, e => e.ix);

            var interval = int.Parse(headers["INTERVAL"]);
            var tz = int.Parse(headers["TIMEZONE_OFFSET"]);
            var epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromMinutes(tz));

            var reference = epoch;
            for (; i < lines.Length; i++)
            {
                var cols = lines[i].Split(',');

                var dateStr = cols[columns["DATE"]];
                var closeStr = cols[columns["CLOSE"]];

                int offset;
                if (dateStr.StartsWith("a"))
                {
                    reference = epoch.AddSeconds(int.Parse(dateStr.Substring(1)));
                    offset = 0;
                }
                else
                {
                    offset = int.Parse(dateStr);
                }

                var date = reference.AddSeconds(offset * interval);
                var close = decimal.Parse(closeStr);

                yield return Tuple.Create(date, close);
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
    }
}
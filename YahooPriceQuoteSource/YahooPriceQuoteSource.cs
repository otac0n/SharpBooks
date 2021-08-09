// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace YahooPriceQuoteSource
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using SharpBooks;
    using SharpBooks.Plugins;

    /// <summary>
    /// Provides functionality to download price quote data from Yahoo!® Finance.
    /// </summary>
    public class YahooPriceQuoteSource : IPriceQuoteSource
    {
        private const string UrlFormat = "http://finance.yahoo.com/d/quotes.csv?f=sd1t1l1&s={0}";
        private const string YahooTimeZoneId = "Eastern Standard Time";

        /// <summary>
        /// Performs plugin-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Retrieves a price quote from Yahoo!® Finance.
        /// </summary>
        /// <param name="security">The security for which to get the quote.</param>
        /// <param name="currency">The currency in which to express the quote.</param>
        /// <returns>The requested price quote.</returns>
        public PriceQuote GetPriceQuote(Security security, Security currency)
        {
            if (security == null)
            {
                throw new ArgumentNullException(nameof(security));
            }

            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (currency.SecurityType != SecurityType.Currency)
            {
                throw new ArgumentException("The argument must be a Security with a SecurityType of Currency", nameof(currency));
            }

            if (security.SecurityType != SecurityType.Stock &&
                security.SecurityType != SecurityType.Fund &&
                security.SecurityType != SecurityType.Currency)
            {
                throw BuildError(security.Symbol, "Only stocks, funds, and currencies are supported.");
            }

            var symbol = security.Symbol;

            if (security.SecurityType == SecurityType.Currency)
            {
                symbol = symbol + currency.Symbol + "=X";
            }

            var client = new WebClient();
            string data;

            try
            {
                data = client.DownloadString(string.Format(UrlFormat, symbol));
            }
            catch (WebException ex)
            {
                throw BuildError(symbol, "Check the inner exception for details.", ex);
            }

            var split = data.Split(',');

            if (split.Length != 4)
            {
                throw BuildError(symbol, "The data returned was not in a recognized format.");
            }

            var date = Unquote(split[1]);
            var time = Unquote(split[2]);
            var value = Unquote(split[3]);

            if (date == "N/A")
            {
                throw BuildError(symbol, "The symbol could not be found.");
            }

            if (!DateTime.TryParse(date, out var utcDate))
            {
                throw BuildError(symbol, "The data returned was not in a recognized format.");
            }

            var yahooTimeZone = TimeZoneInfo.FindSystemTimeZoneById(YahooTimeZoneId);

            var minDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, yahooTimeZone);

            if (!DateTime.TryParse(minDate.ToShortDateString() + " " + time, out var dateTime))
            {
                throw BuildError(symbol, "The data returned was not in a recognized format.");
            }

            if (dateTime < minDate)
            {
                dateTime.AddDays(1);
            }

            dateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, yahooTimeZone);

            if (!decimal.TryParse(value, out var price))
            {
                throw BuildError(symbol, "The data returned was not in a recognized format.");
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

                return new PriceQuote(Guid.NewGuid(), dateTime, security, quantity, currency, longPrice, "Yahoo!® Finance");
            }
        }

        private static Exception BuildError(string symbol, string error)
        {
            return BuildError(symbol, error, null);
        }

        private static Exception BuildError(string symbol, string error, Exception innerException)
        {
            return new PriceRetrievalFailureException("Could not download the Yahoo!® price data for the symbol '" + symbol + "'.  " + error, innerException);
        }

        private static string Unquote(string value)
        {
            var match = Regex.Match(value, @"\A\s*""?(?<value>[^""]*)""?\s*\Z");
            return match.Success ? match.Groups["value"].Value : value;
        }
    }
}

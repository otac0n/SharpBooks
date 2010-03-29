namespace YahooPriceSource
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Text.RegularExpressions;
    using SharpBooks;
    using SharpBooks.Plugins;

    public class YahooPriceSource : IPriceQuoteSource
    {
        private const string UrlFormat = "http://finance.yahoo.com/d/quotes.csv?f=sd1l1&s={0}";

        public string Name
        {
            get
            {
                return "Yahoo!® Finance Stock Quotes";
            }
        }

        public PriceQuote GetPriceQuote(Security security, Security currecny)
        {
            if (security.SecurityType != SecurityType.Stock &&
                security.SecurityType != SecurityType.Fund)
            {
                throw new PriceRetrievalFailureException("The " + this.Name + " only supports Stock and Fund type securities.");
            }

            var client = new WebClient();
            string data;

            try
            {
                data = client.DownloadString(string.Format(UrlFormat, security.Symbol));
            }
            catch (WebException ex)
            {
                throw BuildError(security.Symbol, "Check the inner exception for details.", ex);
            }

            var split = data.Split(',');

            if (split.Length != 3)
            {
                throw BuildError(security.Symbol, "The data returned was not in a recognized format.");
            }

            string symbol = Unquote(split[0]);
            string date = Unquote(split[1]);
            string value = Unquote(split[2]);

            if (date == "N/A")
            {
                throw BuildError(security.Symbol, "The symbol could not be found.");
            }

            if (!string.Equals(symbol, security.Symbol))
            {
                Trace.WriteLine("Symbol '" + security.Symbol + "' != '" + symbol + "'.");
            }

            DateTime dateTime;
            if (!DateTime.TryParse(date, out dateTime))
            {
                throw BuildError(security.Symbol, "The data returned was not in a recognized format.");
            }

            decimal price;
            if (!decimal.TryParse(value, out price))
            {
                throw BuildError(security.Symbol, "The data returned was not in a recognized format.");
            }

            price *= currecny.FractionTraded;
            var longPrice = (long)Math.Floor(price);

            if (price != longPrice)
            {
                throw BuildError(security.Symbol, "The price of the symbol was invalid for the currency '" + currecny.Name + "'");
            }

            return new PriceQuote(dateTime, security, security.FractionTraded, currecny, longPrice);
        }

        private static Exception BuildError(string symbol, string error)
        {
            return BuildError(symbol, error, null);
        }

        private static Exception BuildError(string symbol, string error, Exception innerException)
        {
            return new PriceRetrievalFailureException("Could not download the Yahoo!® price data for the symbol '" + symbol + "'.  " + error);
        }

        private static string Unquote(string value)
        {
            var match = Regex.Match(value, @"\A\s*""?(?<value>[^""]*)""?\s*\Z");
            return match.Success ? match.Groups["value"].Value : value;
        }
    }
}

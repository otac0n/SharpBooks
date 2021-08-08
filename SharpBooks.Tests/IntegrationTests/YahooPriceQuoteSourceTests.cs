// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Tests.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using YahooPriceQuoteSource;

    [TestFixture]
    [Explicit]
    public class YahooPriceQuoteSourceTests
    {
        private readonly Dictionary<string, Security> securities = new Dictionary<string, Security>();

        public YahooPriceQuoteSourceTests()
        {
            var google = new Security(
                Guid.NewGuid(),
                SecurityType.Stock,
                "Google, Inc.",
                "GOOG",
                new CurrencyFormat(currencySymbol: "GOOG", positiveFormat: PositiveFormat.SuffixSpaced, negativeFormat: NegativeFormat.Prefix),
                100);

            var yahoo = new Security(
                Guid.NewGuid(),
                SecurityType.Stock,
                "Yahoo, Inc.",
                "YHOO",
                new CurrencyFormat(currencySymbol: "YHOO", positiveFormat: PositiveFormat.SuffixSpaced, negativeFormat: NegativeFormat.Prefix),
                100);

            var microsoft = new Security(
                Guid.NewGuid(),
                SecurityType.Stock,
                "Microsoft, Inc.",
                "MSFT",
                new CurrencyFormat(currencySymbol: "MSFT", positiveFormat: PositiveFormat.SuffixSpaced, negativeFormat: NegativeFormat.Prefix),
                100);

            var usd = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "United States dollar",
                "USD",
                new CurrencyFormat(currencySymbol: "$"),
                1000);

            var euro = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "Euro",
                "EUR",
                new CurrencyFormat(currencySymbol: "€"),
                100);

            var aud = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "Australian dollar",
                "AUD",
                new CurrencyFormat(currencySymbol: "$"),
                100);

            var nzd = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "New Zealand dollar",
                "NZD",
                new CurrencyFormat(currencySymbol: "$"),
                100);

            var pound = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "Pound sterling",
                "GBP",
                new CurrencyFormat(currencySymbol: "£"),
                100);

            this.securities.Add(google.Symbol, google);
            this.securities.Add(yahoo.Symbol, yahoo);
            this.securities.Add(microsoft.Symbol, microsoft);
            this.securities.Add(euro.Symbol, euro);
            this.securities.Add(usd.Symbol, usd);
            this.securities.Add(aud.Symbol, aud);
            this.securities.Add(nzd.Symbol, nzd);
            this.securities.Add(pound.Symbol, pound);
        }

        [Test]
        [TestCase("USD", "EUR")]
        [TestCase("GBP", "EUR")]
        [TestCase("USD", "GBP")]
        [TestCase("USD", "NZD")]
        [TestCase("NZD", "AUD")]
        [TestCase("GOOG", "USD")]
        [TestCase("YHOO", "USD")]
        [TestCase("MSFT", "USD")]
        public void GetPriceQuote_WhenCalledWithRealWorldSecurities_Succeeds(string securitySymbol, string currencySymbol)
        {
            // Create a new Yahoo Price Source.
            using (var source = new YahooPriceQuoteSource())
            {
                // Retrieve the real-world security and currency.
                var security = this.securities[securitySymbol];
                var currency = this.securities[currencySymbol];

                Assume.That(currency.SecurityType, Is.EqualTo(SecurityType.Currency));

                // Retrieve the quote.
                var quote = source.GetPriceQuote(security, currency);

                // Assert that the returned quote is valid, then display the quote for the test-runner.
                Assert.That(quote, Is.Not.Null);
                TestUtils.DisplayQuote(quote);
            }
        }
    }
}

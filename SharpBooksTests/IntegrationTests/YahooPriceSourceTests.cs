//-----------------------------------------------------------------------
// <copyright file="YahooPriceSourceTests.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using YahooPriceSource;
    using System.Diagnostics;

    [TestFixture]
    [Explicit]
    public class YahooPriceSourceTests
    {
        Dictionary<string, Security> securities = new Dictionary<string, Security>();

        [TestFixtureSetUp]
        public void SetUp()
        {
            var google = new Security(
                SecurityType.Stock,
                "Google, Inc.",
                "GOOG",
                "{0} GOOG",
                100);

            var yahoo = new Security(
                SecurityType.Stock,
                "Yahoo, Inc.",
                "YHOO",
                "{0} YHOO",
                100);

            var microsoft = new Security(
                SecurityType.Stock,
                "Microsoft, Inc.",
                "MSFT",
                "{0} MSFT",
                100);

            var usd = new Security(
                SecurityType.Currency,
                "United States dollar",
                "USD",
                "{0:$0.00#;($0.00#);-$0-}",
                1000);

            var euro = new Security(
                SecurityType.Currency,
                "Euro",
                "EUR",
                "{0:€0.00;(€0.00);-€0-}",
                100);

            var aud = new Security(
                SecurityType.Currency,
                "Australian dollar",
                "AUD",
                "{0:$0.00;($0.00);-$0-}",
                100);

            var nzd = new Security(
                SecurityType.Currency,
                "New Zealand dollar",
                "NZD",
                "{0:$0.00;($0.00);-$0-}",
                100);

            var pound = new Security(
                SecurityType.Currency,
                "Pound sterling",
                "GBP",
                "{0:£0.00;(£0.00);-£0-}",
                100);

            securities.Add(google.Symbol, google);
            securities.Add(yahoo.Symbol, yahoo);
            securities.Add(microsoft.Symbol, microsoft);
            securities.Add(euro.Symbol, euro);
            securities.Add(usd.Symbol, usd);
            securities.Add(aud.Symbol, aud);
            securities.Add(nzd.Symbol, nzd);
            securities.Add(pound.Symbol, pound);
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
            var source = new YahooPriceSource();

            // Retrieve the real-world security and currency.
            var security = securities[securitySymbol];
            var usd = securities[currencySymbol];

            Assume.That(usd.SecurityType, Is.EqualTo(SecurityType.Currency));

            // Retrieve the quote.
            var currency = source.GetPriceQuote(security, usd);

            // Assert that the returned quote is valid, then display the quote for the test-runner.
            Assert.That(currency, Is.Not.Null);
            DisplayQuote(currency);
        }

        private static void DisplayQuote(PriceQuote quote)
        {
            Debug.Write(quote.Security.FormatValue(quote.Quantity));
            Debug.Write(" = ");
            Debug.Write(quote.Currency.FormatValue(quote.Price));
            Debug.Write(" @ ");
            Debug.WriteLine(quote.DateTime);
        }
    }
}

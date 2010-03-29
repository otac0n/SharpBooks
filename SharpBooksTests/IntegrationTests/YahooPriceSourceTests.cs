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

            securities.Add(google.Symbol, google);
            securities.Add(usd.Symbol, usd);
            securities.Add(euro.Symbol, euro);
        }

        [Test]
        public void GetPriceQuote_WhenCalledWithRealWorldValues_Succeeds()
        {
            // Create a new Yahoo Price Source
            var source = new YahooPriceSource();

            // Retrieve a real-world stock for testing.
            var google = securities["GOOG"];

            // Since the stock is based on US Dollars, retrieve the USD currency.
            var usd = securities["USD"];

            // Retrieve the quote.
            var quote = source.GetPriceQuote(google, usd);

            // Assert that the returned quote is valid.
            Assert.That(quote, Is.Not.Null);

            DisplayQuote(quote);
        }

        [Test]
        public void GetPriceQuote_WhenCalledWithCurrencies_Succeeds()
        {
            // Create a new Yahoo Price Source
            var source = new YahooPriceSource();

            // Test converting from Euros...
            var euro = securities["EUR"];

            // ...into United States dollars.
            var usd = securities["USD"];

            // Retrieve the quote.
            var quote = source.GetPriceQuote(euro, usd);

            // Assert that the returned quote is valid.
            Assert.That(quote, Is.Not.Null);
            
            DisplayQuote(quote);
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

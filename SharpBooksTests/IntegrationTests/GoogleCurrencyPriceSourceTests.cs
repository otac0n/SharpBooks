//-----------------------------------------------------------------------
// <copyright file="YahooPriceSourceTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using GooglePriceSource;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class GoogleCurrencyPriceSourceTests
    {
        private readonly Dictionary<string, Security> securities = new Dictionary<string, Security>();

        [TestFixtureSetUp]
        public void SetUp()
        {
            var usd = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "United States dollar",
                "USD",
                "{0:$#,##0.00#;($#,##0.00#);-$0-}",
                1000);

            var euro = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "Euro",
                "EUR",
                "{0:€#,##0.00;(€#,##0.00);-€0-}",
                100);

            var aud = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "Australian dollar",
                "AUD",
                "{0:$#,##0.00;($#,##0.00);-$0-}",
                100);

            var nzd = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "New Zealand dollar",
                "NZD",
                "{0:$#,##0.00;($#,##0.00);-$0-}",
                100);

            var pound = new Security(
                Guid.NewGuid(),
                SecurityType.Currency,
                "Pound sterling",
                "GBP",
                "{0:£#,##0.00;(£#,##0.00);-£0-}",
                100);

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
        public void GetPriceQuote_WhenCalledWithRealWorldCurrencies_Succeeds(string securitySymbol, string currencySymbol)
        {
            // Create a new Google Currency Price Source.
            var source = new GoogleCurrencyPriceQuoteSource();

            // Retrieve the real-world security and currency.
            var security = this.securities[securitySymbol];
            var usd = this.securities[currencySymbol];

            Assume.That(usd.SecurityType, Is.EqualTo(SecurityType.Currency));

            // Retrieve the quote.
            var currency = source.GetPriceQuote(security, usd);

            // Assert that the returned quote is valid, then display the quote for the test-runner.
            Assert.That(currency, Is.Not.Null);
            TestUtils.DisplayQuote(currency);
        }
    }
}

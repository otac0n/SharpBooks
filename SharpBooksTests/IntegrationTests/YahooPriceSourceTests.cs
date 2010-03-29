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

    [TestFixture]
    public class YahooPriceSourceTests
    {
        [Test]
        public void GetPriceQuote_WhenCalledWithRealWorldValues_Succeeds()
        {
            // Create a new Yahoo Price Source
            var source = new YahooPriceSource();

            // Create a real-world stock for testing.
            var google = new Security(
                SecurityType.Stock,
                "Google, Inc.",
                "GOOG",
                "{0} GOOG",
                100);

            // Since the stock is based on US Dollars, create a USD currency.
            var usd = new Security(
                SecurityType.Currency,
                "US Dollars",
                "USD",
                "${0}",
                100);

            // Retrieve the quote.
            var quote = source.GetPriceQuote(google, usd);

            // Assert that the returned quote is valid.
            Assert.That(quote, Is.Not.Null);
        }
    }
}

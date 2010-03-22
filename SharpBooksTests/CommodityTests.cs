//-----------------------------------------------------------------------
// <copyright file="CommodityTests.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CommodityTests
    {
        [Test]
        [TestCase(CommodityType.Currency, "United States dollar", "USD", "${0}")]
        [TestCase(CommodityType.Currency, "Pound Sterling", "GBP", "£{0}")]
        [TestCase(CommodityType.Currency, "Euro", "EUR", "€{0}")]
        [TestCase(CommodityType.Currency, "Japanese yen", "JPY", "¥{0}")]
        [TestCase(CommodityType.Currency, "No Currency", "XXX", "{0}")]
        [TestCase(CommodityType.Currency, "Test Currency", "XTS", "{0}")]
        public void Constructor_WithRealWorldParameters_Succeeds(CommodityType commodityType, string name, string symbol, string signFormat)
        {
            // Construct a new commodity with known good values.
            new Commodity(
                commodityType,
                name,
                symbol,
                signFormat);

            // The test passes, because the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        [TestCase(CommodityType.Currency, "Test Currency", "XTS", "")]
        [TestCase(CommodityType.Currency, "Test Currency", "XTS", null)]
        [TestCase(CommodityType.Currency, "Test Currency", "",    "{0}")]
        [TestCase(CommodityType.Currency, "Test Currency", null,  "{0}")]
        [TestCase(CommodityType.Currency, "",              "XTS", "{0}")]
        [TestCase(CommodityType.Currency, null,            "XTS", "{0}")]
        public void Constructor_WithEmptyParameters_ThrowsException(CommodityType commodityType, string name, string symbol, string signFormat)
        {
            // Build a delegate to construct a new commodity.
            TestDelegate constructCommodity = () => new Commodity(
                commodityType,
                name,
                symbol,
                signFormat);

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructCommodity, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        [TestCase("$")]
        [TestCase("${{0}}")]
        [TestCase("${0")]
        [TestCase("${1}")]
        [TestCase("${0}{1}")]
        public void Constructor_WithInvalidFormat_ThrowsException(string invalidFormat)
        {
            // Build a delegate to construct a new commodity.
            TestDelegate constructCommodity = () => new Commodity(
                CommodityType.Currency,
                "OK_NAME",
                "OK_SYMBOL",
                invalidFormat);

            // Assert that calling the delegate throws an ArgumentException.
            Assert.That(constructCommodity, Throws.InstanceOf<ArgumentException>());
        }
    }
}

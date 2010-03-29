//-----------------------------------------------------------------------
// <copyright file="PriceQuoteTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class PriceQuoteTests
    {
        [Datapoint]
        long zero = 0;

        [Datapoint]
        long one = 1;

        [Datapoint]
        long negOne = -1;

        [Datapoint]
        long max = long.MaxValue;

        [Datapoint]
        long min = long.MinValue;

        [Test]
        public void Constructor_WhenSecurityIsNull_ThrowsException()
        {
        }

        [Test]
        public void Constructor_WhenCurrnecyIsNull_ThrowsException()
        {
        }

        [Test]
        public void Constructor_WhenCurrencyIsNotCurrencyType_ThrowsException()
        {
        }

        [Test]
        public void Constructor_WhenCurrencyIsEqualToSecurity_ThrowsException()
        {
        }

        [Theory]
        public void Constructor_WhenQuantityIsLessThanOrEqualToZero_ThrowsException(long quantity)
        {
            Assume.That(quantity <= 0);

            // Create a new, valid security.
            var security = new Security(
                SecurityType.Fund,
                "Test Fund",
                "TEST",
                "{0}",
                100);

            // Build a test delegate to construct the PriceQuote.
            TestDelegate buildQuote = () => new PriceQuote(
                DateTime.MinValue, // OK
                security, // OK
                quantity,
                TestUtils.TestCurrency, // OK
                100);

            // Assert that calling the delegate with a negative or zero value throws an exception.
            Assert.That(buildQuote, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Theory]
        public void Constructor_WhenPriceIsLessThanOrEqualToZero_ThrowsException()
        {
        }
    }
}

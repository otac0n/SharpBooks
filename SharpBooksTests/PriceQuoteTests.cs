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
        [Datapoints]
        long[] longPoints = new [] { 0, 1, -1, 100, -100, 1000, -1000, long.MaxValue, long.MinValue };

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
        public void Constructor_WhenQuantityOrPriceIsLessThanOrEqualToZero_ThrowsException(long quantity, long price)
        {
            Assume.That(quantity <= 0 || price <= 0);

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
                price); // OK

            // Assert that calling the delegate with a negative or zero value throws an exception.
            Assert.That(buildQuote, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}

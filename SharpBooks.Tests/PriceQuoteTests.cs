//-----------------------------------------------------------------------
// <copyright file="PriceQuoteTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class PriceQuoteTests
    {
        [Datapoints]
        private long[] longDatapoints = new[] { 0, 1, -1, 3, -3, 5, -5, 7, -7, 10, -10, 100, -100, 1000, -1000, long.MaxValue, long.MinValue };

        [Test]
        public void Constructor_WhenCurrencyIsEqualToSecurity_ThrowsException()
        {
            // Build a delegate to construct a new price quote.
            TestDelegate constructPriceQuote = () => new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestCurrency,
                1, // OK
                TestUtils.TestCurrency,
                1, // OK
                "OK_SOURCE");

            // Assert that the delegate throws an InvalidOperationException.
            Assert.That(constructPriceQuote, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Constructor_WhenCurrencyIsNotCurrencyType_ThrowsException()
        {
            var invalidCurrency = new Security(
                Guid.NewGuid(), // OK
                SecurityType.Stock,
                "OK_NAME",
                "OK_SYMBOL",
                new CurrencyFormat(), // OK
                1); // OK

            // Build a delegate to construct a new price quote.
            TestDelegate constructPriceQuote = () => new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestStock, // OK
                1, // OK
                invalidCurrency,
                1, // OK
                "OK_SOURCE");

            // Assert that the delegate throws an InvalidOperationException.
            Assert.That(constructPriceQuote, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Constructor_WhenCurrencyIsNull_ThrowsException()
        {
            // Build a delegate to construct a new price quote.
            TestDelegate constructPriceQuote = () => new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestStock, // OK
                1, // OK
                null,
                1, // OK
                "OK_SOURCE");

            // Assert that the delegate throws an ArgumentNullException.
            Assert.That(constructPriceQuote, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenPriceQuoteIdIsEmpty_ThrowsException()
        {
            // Build a delegate to construct a new price quote.
            TestDelegate constructPriceQuote = () => new PriceQuote(
                Guid.Empty,
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestStock, // OK
                1, // OK
                TestUtils.TestCurrency, // OK
                1, // OK
                "OK_SOURCE");

            // Assert that calling the delegate throws an ArgumentOutOfRangeException.
            Assert.That(constructPriceQuote, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Theory]
        public void Constructor_WhenQuantityOrPriceIsLessThanOrEqualToZero_ThrowsException(long quantity, long price)
        {
            Assume.That(quantity <= 0 || price <= 0);

            // Build a test delegate to construct the PriceQuote.
            TestDelegate buildQuote = () => new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestStock, // OK
                quantity,
                TestUtils.TestCurrency, // OK
                price, // OK
                "OK_SOURCE");

            // Assert that calling the delegate with a negative or zero value throws an exception.
            Assert.That(buildQuote, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WhenSecurityIsNull_ThrowsException()
        {
            // Build a delegate to construct a new price quote.
            TestDelegate constructPriceQuote = () => new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                null,
                1, // OK
                TestUtils.TestCurrency, // OK
                1, // OK
                "OK_SOURCE");

            // Assert that the delegate throws an ArgumentNullException.
            Assert.That(constructPriceQuote, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenSourceIsEmpty_ThrowsException()
        {
            // Build a delegate to construct a new price quote.
            TestDelegate constructPriceQuote = () => new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestStock, // OK
                1, // OK
                TestUtils.TestCurrency, // OK
                1, // OK
                string.Empty);

            // Assert that the delegate throws an ArgumentNullException.
            Assert.That(constructPriceQuote, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenSourceIsNull_ThrowsException()
        {
            // Build a delegate to construct a new price quote.
            TestDelegate constructPriceQuote = () => new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestStock, // OK
                1, // OK
                TestUtils.TestCurrency, // OK
                1, // OK
                null);

            // Assert that the delegate throws an ArgumentNullException.
            Assert.That(constructPriceQuote, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        [TestCase(DateTimeKind.Local)]
        [TestCase(DateTimeKind.Unspecified)]
        public void Constructor_WithNonUTCDate_ThrowsException(DateTimeKind kind)
        {
            Assert.That(() =>
            {
                var ignore = new PriceQuote(
                    Guid.NewGuid(), // OK
                    DateTime.SpecifyKind(DateTime.MinValue, kind), // OK
                    TestUtils.TestStock, // OK
                    1, // OK
                    TestUtils.TestCurrency, // OK
                    1, // OK
                    "OK_SOURCE");
            }, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WithValidParameters_Succeeds()
        {
            new PriceQuote(
                Guid.NewGuid(), // OK
                DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc), // OK
                TestUtils.TestStock, // OK
                1, // OK
                TestUtils.TestCurrency, // OK
                1, // OK
                "OK_SOURCE");

            // The test passes, because the call to the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }
    }
}

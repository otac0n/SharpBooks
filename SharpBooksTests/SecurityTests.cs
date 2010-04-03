//-----------------------------------------------------------------------
// <copyright file="SecurityTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class SecurityTests
    {
        [Datapoints]
        private int[] integerDatapoints = new[] { 0, 1, -1, 100, -100, 1000, -1000, int.MaxValue, int.MinValue };

        [Test]
        [TestCase(SecurityType.Currency, "United States dollar", "USD", "{0:$#,##0.00#;($#,##0.00#);-$0-}", 1000)]
        [TestCase(SecurityType.Currency, "Pound Sterling", "GBP", "£{0}", 100)]
        [TestCase(SecurityType.Currency, "Euro", "EUR", "€{0}", 100)]
        [TestCase(SecurityType.Currency, "Japanese yen", "JPY", "¥{0}", 100)]
        [TestCase(SecurityType.Currency, "No Currency", "XXX", "{0}", 1)]
        [TestCase(SecurityType.Currency, "Test Currency", "XTS", "{0}", 1)]
        public void Constructor_WithRealWorldParameters_Succeeds(SecurityType securityType, string name, string symbol, string signFormat, int fractionTraded)
        {
            // Construct a new security with known good values.
            new Security(
                Guid.NewGuid(),
                securityType,
                name,
                symbol,
                signFormat,
                fractionTraded);

            // The test passes, because the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "Test Currency", "XTS", "")]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "Test Currency", "XTS", null)]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "Test Currency", "",    "{0}")]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "Test Currency", null,  "{0}")]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "",              "XTS", "{0}")]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, null,            "XTS", "{0}")]
        [TestCase("00000000-0000-0000-0000-000000000000", SecurityType.Currency, "Test Currency", "XTS", "")]
        public void Constructor_WithEmptyParameters_ThrowsException(string securityId, SecurityType securityType, string name, string symbol, string signFormat)
        {
            // Build a delegate to construct a new security.
            TestDelegate constructSecurity = () => new Security(
                new Guid(securityId),
                securityType,
                name,
                symbol,
                signFormat,
                1); // OK

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructSecurity, Throws.InstanceOf<ArgumentNullException>());
        }

        [Theory]
        public void Constructor_WithFractionTradedLessThanOrEqualToZero_ThrowsException(int fractionTraded)
        {
            Assume.That(fractionTraded <= 0);

            // Build a delegate to construct a new security.
            TestDelegate constructSecurity = () => new Security(
                Guid.NewGuid(), // OK
                SecurityType.Currency, // OK
                "OK_NAME",
                "OK_SYMBOL",
                "OK_FORMAT{0}",
                fractionTraded);

            // Assert that calling the delegate throws an ArgumentException.
            Assert.That(constructSecurity, Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        [TestCase("$")]
        [TestCase("${{0}}")]
        [TestCase("${0")]
        [TestCase("${1}")]
        [TestCase("${0}{1}")]
        public void Constructor_WithInvalidFormat_ThrowsException(string invalidFormat)
        {
            // Build a delegate to construct a new security with the given format.
            TestDelegate constructSecurity = () => new Security(
                Guid.NewGuid(), // OK
                SecurityType.Currency, // OK
                "OK_NAME",
                "OK_SYMBOL",
                invalidFormat,
                1); // OK

            // Assert that calling the delegate throws an ArgumentException.
            Assert.That(constructSecurity, Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        [TestCase("{0}")]
        [TestCase("${0}")]
        [TestCase("{0:$0.00#}")]
        [TestCase("{0:€0.00;(€0.00);-€0-}")]
        [TestCase("{0} GOOG")]
        [TestCase("{0:$#,##0.00#;($#,##0.00#);-$0-}")]
        [TestCase("{0:¥#,##0.###;(¥#,##0.###);-¥0-}")]
        public void Constructor_WithValidFormat_Succeeds(string validFormat)
        {
            // Construct a new security with the given format.
            new Security(
                Guid.NewGuid(), // OK
                SecurityType.Currency, // OK
                "OK_NAME",
                "OK_SYMBOL",
                validFormat,
                1); // OK

            // The test passes, because the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }
    }
}

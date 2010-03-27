//-----------------------------------------------------------------------
// <copyright file="SecurityTests.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
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
        [Test]
        [TestCase(SecurityType.Currency, "United States dollar", "USD", "${0}")]
        [TestCase(SecurityType.Currency, "Pound Sterling", "GBP", "£{0}")]
        [TestCase(SecurityType.Currency, "Euro", "EUR", "€{0}")]
        [TestCase(SecurityType.Currency, "Japanese yen", "JPY", "¥{0}")]
        [TestCase(SecurityType.Currency, "No Currency", "XXX", "{0}")]
        [TestCase(SecurityType.Currency, "Test Currency", "XTS", "{0}")]
        public void Constructor_WithRealWorldParameters_Succeeds(SecurityType securityType, string name, string symbol, string signFormat)
        {
            // Construct a new security with known good values.
            new Security(
                securityType,
                name,
                symbol,
                signFormat);

            // The test passes, because the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        [TestCase(SecurityType.Currency, "Test Currency", "XTS", "")]
        [TestCase(SecurityType.Currency, "Test Currency", "XTS", null)]
        [TestCase(SecurityType.Currency, "Test Currency", "",    "{0}")]
        [TestCase(SecurityType.Currency, "Test Currency", null,  "{0}")]
        [TestCase(SecurityType.Currency, "",              "XTS", "{0}")]
        [TestCase(SecurityType.Currency, null,            "XTS", "{0}")]
        public void Constructor_WithEmptyParameters_ThrowsException(SecurityType securityType, string name, string symbol, string signFormat)
        {
            // Build a delegate to construct a new security.
            TestDelegate constructSecurity = () => new Security(
                securityType,
                name,
                symbol,
                signFormat);

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructSecurity, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        [TestCase("$")]
        [TestCase("${{0}}")]
        [TestCase("${0")]
        [TestCase("${1}")]
        [TestCase("${0}{1}")]
        public void Constructor_WithInvalidFormat_ThrowsException(string invalidFormat)
        {
            // Build a delegate to construct a new security.
            TestDelegate constructSecurity = () => new Security(
                SecurityType.Currency,
                "OK_NAME",
                "OK_SYMBOL",
                invalidFormat);

            // Assert that calling the delegate throws an ArgumentException.
            Assert.That(constructSecurity, Throws.InstanceOf<ArgumentException>());
        }
    }
}

// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class SecurityTests
    {
        [Datapoints]
        private readonly int[] integerDatapoints = new[] { 0, 1, -1, 3, -3, 5, -5, 7, -7, 10, -10, 100, -100, 1000, -1000, int.MaxValue, int.MinValue };

        [Test]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "Test Currency", "XTS", null)]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "Test Currency", "", "XTS")]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "Test Currency", null, "XTS")]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, "", "XTS", "XTS")]
        [TestCase("a2394d50-0b8e-4374-a66b-540a0a15767e", SecurityType.Currency, null, "XTS", "XTS")]
        [TestCase("00000000-0000-0000-0000-000000000000", SecurityType.Currency, "Test Currency", "XTS", "XTS")]
        public void Constructor_WithEmptyParameters_ThrowsException(string securityId, SecurityType securityType, string name, string symbol, string currencySymbol)
        {
            // Build a delegate to construct a new security.
            Security constructSecurity()
            {
                return new Security(
                    new Guid(securityId),
                    securityType,
                    name,
                    symbol,
                    currencySymbol == null ? null : new CurrencyFormat(currencySymbol: currencySymbol),
                    1); // OK
            }

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructSecurity, Throws.InstanceOf<ArgumentNullException>());
        }

        [Theory]
        public void Constructor_WithFractionTradedLessThanOrEqualToZero_ThrowsException(int fractionTraded)
        {
            Assume.That(fractionTraded <= 0);

            // Build a delegate to construct a new security.
            Security constructSecurity()
            {
                return new Security(
                    Guid.NewGuid(), // OK
                    SecurityType.Currency, // OK
                    "OK_NAME",
                    "OK_SYMBOL",
                    new CurrencyFormat(currencySymbol: "OK_SYMBOL"),
                    fractionTraded);
            }

            // Assert that calling the delegate throws an ArgumentException.
            Assert.That(constructSecurity, Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        [TestCase(SecurityType.Currency, "United States dollar", "USD", "$", 1000)]
        [TestCase(SecurityType.Currency, "Pound Sterling", "GBP", "£", 100)]
        [TestCase(SecurityType.Currency, "Euro", "EUR", "€", 100)]
        [TestCase(SecurityType.Currency, "Japanese yen", "JPY", "¥", 100)]
        [TestCase(SecurityType.Currency, "No Currency", "XXX", "XXX", 1)]
        [TestCase(SecurityType.Currency, "Test Currency", "XTS", "XTS", 1)]
        public void Constructor_WithRealWorldParameters_Succeeds(SecurityType securityType, string name, string symbol, string currencySymbol, int fractionTraded)
        {
            // Construct a new security with known good values.
            _ = new Security(
                Guid.NewGuid(),
                securityType,
                name,
                symbol,
                new CurrencyFormat(currencySymbol: currencySymbol),
                fractionTraded);

            // The test passes, because the constructor has completed successfully.
        }
    }
}

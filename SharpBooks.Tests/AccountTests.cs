//-----------------------------------------------------------------------
// <copyright file="AccountTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class AccountTests
    {
        [Datapoints]
        private int[] integerDatapoints = new[] { 0, 1, -1, 3, -3, 5, -5, 7, -7, 10, -10, 100, -100, 1000, -1000, int.MaxValue, int.MinValue };

        [Test]
        public void Constructor_WithKnownGoodParameters_Succeeds()
        {
            // Create a new account with known good values.
            new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // The test passes, because the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void Constructor_WhenSecurityIsNull_ThrowsException()
        {
            // Build a delegate to construct a new account.
            TestDelegate constructTransaction = () => new Account(
                Guid.NewGuid(), // OK
                null,
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenAccountIdIsEmpty_ThrowsException()
        {
            // Build a delegate to construct a new account.
            TestDelegate constructTransaction = () => new Account(
                Guid.Empty,
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that calling the delegate throws an ArgumentOutOfRangeException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WhenNameIsEmpty_ThrowsException()
        {
            // Build a delegate to construct a new account.
            TestDelegate constructTransaction = () => new Account(
                Guid.NewGuid(),
                TestUtils.TestCurrency, // OK
                null, // OK
                string.Empty,
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenNameIsNull_ThrowsException()
        {
            // Build a delegate to construct a new account.
            TestDelegate constructTransaction = () => new Account(
                Guid.NewGuid(),
                TestUtils.TestCurrency, // OK
                null, // OK
                null,
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentNullException>());
        }

        [Theory]
        public void Constructor_WhenSmallestFractionIsLessThanOrEqualToZero_ThrowsException(int smallestFraction)
        {
            // Assume that the smallest fraction allowed in the account is not evenly divisible to the security's fraction traded.
            Assume.That(smallestFraction <= 0);

            // Build a delegate to construct a new account.
            TestDelegate constructTransaction = () => new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                smallestFraction);

            // Assert that calling the delegate throws an ArgumentOutOfRangeException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Theory]
        public void Constructor_WhenSecurityFractionIsNotAMultipleOfAccountsSmallestFraction_ThrowsException(int smallestFraction)
        {
            // Assume that the smallest fraction allowed in the account is not evenly divisible to the security's fraction traded.
            Assume.That(smallestFraction > 0);
            Assume.That(TestUtils.TestCurrency.FractionTraded % smallestFraction != 0);

            // Build a delegate to construct a new account.
            TestDelegate constructTransaction = () => new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                smallestFraction);

            // Assert that calling the delegate throws an InvalidOperationException.
            Assert.That(constructTransaction, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(100)]
        public void Constructor_WhenAncestorHasTheSameAccountId_ThrowsException(int generations)
        {
            // Build a new, valid account.
            var ancestor = TestUtils.CreateValidAccount();

            // Build the ancestry of the child account.
            var parent = ancestor;
            for (var i = 1; i < generations; i++)
            {
                parent = new Account(
                    Guid.NewGuid(), // OK
                    TestUtils.TestCurrency, // OK
                    parent,
                    "OK_NAME",
                    TestUtils.TestCurrency.FractionTraded); // OK
            }

            // Build a delegate to construct a new account with the same AccountId as its ancestor.
            TestDelegate constructTransaction = () => new Account(
                ancestor.AccountId,
                TestUtils.TestCurrency, // OK
                ancestor,
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that calling the delegate throws an InvalidOperationException.
            Assert.That(constructTransaction, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void GetParentAccount_WhenConstructorIsCalledWithParent_ReturnsParent()
        {
            // Build a parent account.
            var parent = new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Construct the child account, passing the above account as the parent.
            var child = new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                parent,
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that the child returns the above account as its parent.
            Assert.That(child.ParentAccount, Is.EqualTo(parent));
        }
    }
}

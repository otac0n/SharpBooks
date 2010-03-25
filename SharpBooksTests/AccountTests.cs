//-----------------------------------------------------------------------
// <copyright file="AccountTests.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
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
        [Test]
        public void Constructor_WithKnownGoodParameters_Succeeds()
        {
            // Create a new account with known good values.
            new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                null); // OK

            // The test passes, because the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void Constructor_WhenCommodityIsNull_ThrowsException()
        {
            // Build a delegate to construct a new account.
            TestDelegate constructTransaction = () => new Account(
                Guid.NewGuid(), // OK
                null,
                null); // OK

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
                null); // OK

            // Assert that calling the delegate throws an ArgumentOutOfRangeException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentOutOfRangeException>());
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

            // Build the ancestory of the child account.
            var parent = ancestor;
            for (var i = 1; i < generations; i++)
            {
                parent = new Account(
                    Guid.NewGuid(), // OK
                    TestUtils.TestCurrency, // OK
                    parent);
            }

            // Build a delegate to construct a new account with the same AccountId as its ancestor.
            TestDelegate constructTransaction = () => new Account(
                ancestor.AccountId,
                TestUtils.TestCurrency, // OK
                ancestor);

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
                null); // OK

            // Construct the child account, passing the above account as the parent.
            var child = new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                parent);

            // Assert that the child returns the above account as its parent.
            Assert.That(child.ParentAccount, Is.EqualTo(parent));
        }
    }
}

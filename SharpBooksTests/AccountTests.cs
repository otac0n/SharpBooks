//-----------------------------------------------------------------------
// <copyright file="AccountTests.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
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

//-----------------------------------------------------------------------
// <copyright file="BookTests.cs" company="(none)">
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
    public class BookTests
    {
        [Test]
        public void AddAccount_WhenAccountIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Assert that trying to add a null account throws an ArgumentNullException.
            Assert.That(() => book.AddAccount(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddAccount_WhenAccountsParentHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

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

            // Assert that trying to add the child account throws a ParentMissingException.
            Assert.That(() => book.AddAccount(child), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddAccount_WhenAccountIsValidAndHasNoParent_Succeeds()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // The test passes, because the call to AddAccount() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void AddAccount_WhenAccountsParentHasBeenAdded_Succeeds()
        {
            // Create a new, valid book.
            var book = new Book();

            // Build a parent account.
            var parent = new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                null); // OK

            // Add the parent account to the book.
            book.AddAccount(parent);

            // Construct the child account, passing the above account as the parent.
            var child = new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                parent);

            // Add the child account to the book.
            book.AddAccount(child);

            // The test passes, because the call to AddAccount() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void RemoveAccount_WhenAccountIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Assert that trying to remove a null account throws an ArgumentNullException.
            Assert.That(() => book.RemoveAccount(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveAccount_WhenAccountHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Assert that trying to remove an account that has not been added throws an InvalidOperationException.
            Assert.That(() => book.RemoveAccount(account), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveAccount_WhenAccountHasBeenAddedHasNoChildrenAndIsNotInvolvedInAnySplits_Succeeds()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // Remove the account from the book.
            book.RemoveAccount(account);

            // The test passes, because the call to RemoveAccount() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void RemoveAccount_WhenAccountHasAlreadyBeenRemovedAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add and immediately remove the account.
            book.AddAccount(account);
            book.RemoveAccount(account);

            // Assert that trying to remove an account that has already been removed throws an InvalidOperationException.
            Assert.That(() => book.RemoveAccount(account), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveAccount_WhenAccountHasChildren_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Build a parent account.
            var parent = new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                null); // OK

            // Add the parent account to the book.
            book.AddAccount(parent);

            // Construct the child account, passing the above account as the parent.
            var child = new Account(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency, // OK
                parent);

            // Add the child account to the book.
            book.AddAccount(child);

            // Assert that trying to remove the parent account throws an InvalidOperationException.
            Assert.That(() => book.RemoveAccount(parent), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}

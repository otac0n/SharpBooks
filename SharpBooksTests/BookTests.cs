﻿//-----------------------------------------------------------------------
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
        public void AddAccount_DuplicateAttempts_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // Assert that trying to add the account again throws a ParentMissingException.
            Assert.That(() => book.AddAccount(account), Throws.InstanceOf<InvalidOperationException>());
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
        public void RemoveAccount_WhenAccountHasAlreadyBeenRemoved_ThrowsException()
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

        [Test]
        public void RemoveAccount_WhenAccountIsInvolvedInSplits_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // Create a new transaction and add a single, zero split.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Add the transaction to the book.
            book.AddTransaction(transaction);

            // Assert that trying to remove the account while a transaction that depends on it is in the book throws an InvalidOperationException.
            Assert.That(() => book.RemoveAccount(account), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddTransaction_WhenTransactionIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Assert that trying to add a null transaction throws an ArgumentNullException.
            Assert.That(() => book.AddTransaction(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddTransaction_WhenTransactionIsInvalid_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, empty (and therefore, invalid) transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Assert that trying to add an invalid transaction throws an InvalidOperationException.
            Assert.That(() => book.AddTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddTransaction_WhenTransactionIsLocked_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Create a new transaction and add a single, zero split.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Lock the transaction for editing.
            using (transaction.Lock())
            {
                // Assert that trying to add a transaction while it is locked throws an InvalidOperationException.
                Assert.That(() => book.AddTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
            }
        }

        [Test]
        public void AddTransaction_WhenAnySplitAccountHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Create a new transaction and add a single, zero split.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that trying to add a transaction without adding the account throws an InvalidOperationException.
            Assert.That(() => book.AddTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddTransaction_DuplicateAttempts_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new transaction and add a single, zero split.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Add the transaction to the book.
            book.AddTransaction(transaction);

            // Assert that trying to add a transaction that has already been added throws an InvalidOperationException.
            Assert.That(() => book.AddTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveTransaction_WhenTransactionIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Assert that trying to remove a null transaction throws an ArgumentNullException.
            Assert.That(() => book.RemoveTransaction(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveTransaction_WhenValid_Succeeds()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new transaction and add a single, zero split.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Add and immediately remove the transaction from the book.
            book.AddTransaction(transaction);
            book.RemoveTransaction(transaction);

            // The test passes, because the call to RemoveTransaction() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void RemoveTransaction_WhenTransactionHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new transaction and add a single, zero split.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that trying to remove a transaction that has not been added throws an InvalidOperationException.
            Assert.That(() => book.RemoveTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveTransaction_WhenTransactionHasAlreadyBeenRemoved_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new transaction and add a single, zero split.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Add and immediately remove the transaction from the book.
            book.AddTransaction(transaction);
            book.RemoveTransaction(transaction);

            // Assert that trying to remove a transaction that has already been removed throws an InvalidOperationException.
            Assert.That(() => book.RemoveTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [TestFixture]
        public class TransactionTests
        {
            [Test]
            public void GetIsLocked_WhenAddedToABook_ReturnsTrue()
            {
                // Create a new, valid book.
                var book = new Book();

                // Create a new, valid account and add it to the book.
                var account = TestUtils.CreateValidAccount();
                book.AddAccount(account);

                // Create a new transaction and add a single, zero split.
                var transaction = TestUtils.CreateEmptyTransaction();
                using (var transactionLock = transaction.Lock())
                {
                    var split = transaction.AddSplit(transactionLock);
                    split.SetAccount(account, transactionLock);
                }

                // Add the transaction to the book.
                book.AddTransaction(transaction);

                // Assert that the book locks the transaction.
                Assert.That(transaction.IsLocked, Is.True);
            }

            [Test]
            public void GetIsLocked_WhenRemovedFromABook_ReturnsFalse()
            {
                // Create a new, valid book.
                var book = new Book();

                // Create a new, valid account and add it to the book.
                var account = TestUtils.CreateValidAccount();
                book.AddAccount(account);

                // Create a new transaction and add a single, zero split.
                var transaction = TestUtils.CreateEmptyTransaction();
                using (var transactionLock = transaction.Lock())
                {
                    var split = transaction.AddSplit(transactionLock);
                    split.SetAccount(account, transactionLock);
                }

                // Add and immediately remove transaction from the book.
                book.AddTransaction(transaction);
                book.RemoveTransaction(transaction);

                // Assert that the book unlocks the transaction once it is removed.
                Assert.That(transaction.IsLocked, Is.False);
            }
        }
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="TransactionTests.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void Constructor_WithValidParameters_Succeeds()
        {
            // Construct a new transaction with known good values.
            new Transaction(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency); // OK

            // The test passes, because the constructor has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void Constructor_WhenTransactionIdIsEmpty_ThrowsException()
        {
            // Build a delegate to construct a new transaction.
            TestDelegate constructTransaction = () => new Transaction(
                Guid.Empty,
                TestUtils.TestCurrency); // OK

            // Assert that calling the delegate throws an ArgumentOutOfRangeException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WhenCommodityIsNull_ThrowsException()
        {
            // Build a delegate to construct a new transaction.
            TestDelegate constructTransaction = () => new Transaction(
                Guid.NewGuid(), // OK
                null);

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetIsValid_WhenEmpty_ReturnsTrue()
        {
            // Construct a new transaction with known good values.
            var transaction = new Transaction(
                Guid.NewGuid(), // OK
                TestUtils.TestCurrency); // OK

            // Assert that the transaction without any splits is valid.
            Assert.That(transaction.IsValid, Is.True);
        }

        [Test]
        public void GetIsValid_WithSingleZeroSplit_ReturnsTrue()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add an empty split to the transaction.
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that the transaction with a single, zero split is valid.
            Assert.That(transaction.IsValid, Is.True);
        }

        [Test]
        public void GetIsValid_WithSingleNonZeroSplit_ReturnsFalse()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Set the ammount of the split to be non-zero.
                split.SetAmmount(1m, transactionLock);
                split.SetTransactionAmmount(1m, transactionLock);
            }

            // Assert that the transaction with a single, non-zero split is invalid.
            Assert.That(transaction.IsValid, Is.False);
        }

        [Test]
        public void Lock_DuplicateAttempts_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Lock the transaction for editing.
            using (transaction.Lock())
            {
                // Assert that a second edit-lock may not be obtained.
                Assert.That(() => transaction.Lock(), Throws.InstanceOf<InvalidOperationException>());
            }
        }

        [Test]
        public void Unlock_DuplicateAttempts_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Lock and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            transactionLock.Dispose();

            // Assert that a second unlock attempt throws an InvalidOperationException exception.
            Assert.That(() => transaction.Unlock(transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDate_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Set the date of the transaction.
                transaction.SetDate(DateTime.MaxValue, transactionLock);

                // Assert that the Date property reflects the new value.
                Assert.That(transaction.Date, Is.EqualTo(DateTime.MaxValue));
            }
        }

        [Test]
        public void SetDate_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Assert that the transaction will not allow modification without a lock.
            Assert.That(() => transaction.SetDate(DateTime.MaxValue, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDate_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = TestUtils.CreateValidTransaction();
            var transaction2 = TestUtils.CreateValidTransaction();

            // Lock the transaction for editing.
            using (transaction1.Lock())
            {
                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the date.
                    Assert.That(() => transaction1.SetDate(DateTime.MaxValue, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetDate_WithDisposedLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Lock and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            transactionLock.Dispose();

            // Assert that the transaction will not allow modification with a disposed lock.
            Assert.That(() => transaction.SetDate(DateTime.MaxValue, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddSplit_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Assert that the AddSplit function successfully created a split.
                Assert.That(split.Transaction, Is.EqualTo(transaction));
            }
        }

        [Test]
        public void AddSplit_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Assert that the transaction will not allow a split to be added without a lock.
            Assert.That(() => transaction.AddSplit(null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddSplit_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = TestUtils.CreateValidTransaction();
            var transaction2 = TestUtils.CreateValidTransaction();

            // Lock the first transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                using (transaction2.Lock())
                {
                    // Assert that the lock from the first transaction may not be used on the second transaction to add a split.
                    Assert.That(() => transaction2.AddSplit(lock1), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void AddSplit_WithDisposedLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Lock and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            transactionLock.Dispose();

            // Assert that the old lock cannot be reused to add a split.
            Assert.That(() => transaction.AddSplit(transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSplit_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            using (var transactionLock = transaction.Lock())
            {
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Remove the split from the transaction.
                transaction.RemoveSplit(split, transactionLock);
            }

            Assert.That(split.Transaction, Is.Not.EqualTo(transaction));
        }

        [Test]
        public void RemoveSplit_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            using (var transactionLock = transaction.Lock())
            {
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that the transaction will not allow a split to be removed without a lock.
            Assert.That(() => transaction.RemoveSplit(split, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSplit_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = TestUtils.CreateValidTransaction();
            var transaction2 = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the first transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the first transaction.
                var split = transaction1.AddSplit(lock1);
                split.SetAccount(account, lock1);

                // Lock the second transaction for editing.
                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to remove a split.
                    Assert.That(() => transaction1.RemoveSplit(split, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void RemoveSplit_WithDisposedLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            split.SetAccount(account, transactionLock);
            transactionLock.Dispose();

            // Assert that the old lock cannot be reused to remove the split.
            Assert.That(() => transaction.RemoveSplit(split, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSplit_WithNullSplit_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Assert that trying to remove a null split throws an ArgumentNullException
                Assert.That(() => transaction.RemoveSplit(null, transactionLock), Throws.InstanceOf<ArgumentNullException>());
            }
        }

        [Test]
        public void RemoveSplit_WithUnassociatedSplit_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add and remove a split.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                transaction.RemoveSplit(split, transactionLock);

                // Assert that attempting to remove the split again throws an exception.
                Assert.That(() => transaction.RemoveSplit(split, transactionLock), Throws.InstanceOf<InvalidOperationException>());
            }
        }
    }
}

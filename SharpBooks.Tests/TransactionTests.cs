//-----------------------------------------------------------------------
// <copyright file="TransactionTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
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
        public void Constructor_WhenSecurityIsNull_ThrowsException()
        {
            // Build a delegate to construct a new transaction.
            TestDelegate constructTransaction = () => new Transaction(
                Guid.NewGuid(), // OK
                null);

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetIsLocked_WhenLocked_ReturnsTrue()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock the transaction for editing;
            using (transaction.Lock())
            {
                // Assert that the IsLocked returns true while the transaction is locked for editing.
                Assert.That(transaction.IsLocked, Is.True);
            }
        }

        [Test]
        public void GetIsLocked_WhenNotLocked_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Assert that the IsLocked returns false when the transaction has not been locked.
            Assert.That(transaction.IsLocked, Is.False);
        }

        [Test]
        public void GetIsLocked_WhenUnlocked_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock and immediately unlock the transaction.
            transaction.Lock().Dispose();

            // Assert that the IsLocked returns false when the transaction has been unlocked.
            Assert.That(transaction.IsLocked, Is.False);
        }

        [Test]
        public void GetIsValid_WithInvalidSplit_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction, but do not set the account.
                var split = transaction.AddSplit(transactionLock);
                Assume.That(split.IsValid, Is.False);
            }

            // Assert that the transaction is invalid with an invalid split.
            Assert.That(transaction.IsValid, Is.False);
        }

        [Test]
        public void Lock_DuplicateAttempts_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock the transaction for editing.
            using (transaction.Lock())
            {
                // Assert that a second edit-lock may not be obtained.
                Assert.That(() => transaction.Lock(), Throws.InstanceOf<InvalidOperationException>());
            }
        }

        [Test]
        public void Lock_MultipleDisposeAttempts_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            transactionLock.Dispose();

            // Attempt to dispose the transaction lock again.
            transactionLock.Dispose();

            // The test passes, because the second Dispose() call has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void Unlock_DuplicateAttempts_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            transactionLock.Dispose();

            // Assert that a second unlock attempt throws an InvalidOperationException exception.
            Assert.That(() => transaction.Unlock(transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDate_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

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
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Assert that the transaction will not allow modification without a lock.
            Assert.That(() => transaction.SetDate(DateTime.MaxValue, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDate_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

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
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            transactionLock.Dispose();

            // Assert that the transaction will not allow modification with a disposed lock.
            Assert.That(() => transaction.SetDate(DateTime.MaxValue, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddSplit_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);

                // Assert that the AddSplit function successfully created a split.
                Assert.That(split.Transaction, Is.EqualTo(transaction));
            }
        }

        [Test]
        public void AddSplit_WithEmptyLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Assert that the transaction will not allow a split to be added without a lock.
            Assert.That(() => transaction.AddSplit(null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddSplit_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

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
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            transactionLock.Dispose();

            // Assert that the old lock cannot be reused to add a split.
            Assert.That(() => transaction.AddSplit(transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSplit_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            using (var transactionLock = transaction.Lock())
            {
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);
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
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            using (var transactionLock = transaction.Lock())
            {
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);
            }

            // Assert that the transaction will not allow a split to be removed without a lock.
            Assert.That(() => transaction.RemoveSplit(split, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSplit_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the first transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the first transaction.
                var split = transaction1.AddSplit(lock1);
                split.SetAccount(account, lock1);
                split.SetSecurity(account.Security, lock1);

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
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            split.SetAccount(account, transactionLock);
            split.SetSecurity(account.Security, transactionLock);
            transactionLock.Dispose();

            // Assert that the old lock cannot be reused to remove the split.
            Assert.That(() => transaction.RemoveSplit(split, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSplit_WithNullSplit_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

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
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add and remove a split.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);
                transaction.RemoveSplit(split, transactionLock);

                // Assert that attempting to remove the split again throws an exception.
                Assert.That(() => transaction.RemoveSplit(split, transactionLock), Throws.InstanceOf<InvalidOperationException>());
            }
        }
    }
}

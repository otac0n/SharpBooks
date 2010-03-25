//-----------------------------------------------------------------------
// <copyright file="SplitTests.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class SplitTests
    {
        /// <summary>
        /// Holds a valid commodity, based on the ISO 4217 testing currency, XTS.
        /// </summary>
        private readonly Commodity noCurrency = new Commodity(
            CommodityType.Currency,
            "No Currency",
            "XXX",
            "{0}");

        [Test]
        public void GetIsValid_WhenAmmountAndTransactionAmmountDifferButCommodityIsTheSameAsTheTransactionBaseCommodity_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Set the ammount and transaction ammount to be different.
                split.SetAmmount(1, transactionLock);
                split.SetTransactionAmmount(2, transactionLock);

                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAmmountAndTransactionAmmountSignsDiffer_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Set the ammount and transaction ammount to have different signs.
                split.SetAmmount(-1, transactionLock);
                split.SetTransactionAmmount(1, transactionLock);

                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAccountIsNull_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);

                // Assert that the split is invalid without assigning an account.
                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void SetAccount_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();
            
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);

                // Set the account of the split.
                split.SetAccount(account, transactionLock);

                // Assert that the Account property reflects the new value.
                Assert.That(split.Account, Is.EqualTo(account));
            }
        }

        [Test]
        public void SetAccount_WithEmptyLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();
            
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetAccount(account, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetAccount_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetAccount(account, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetAccount_WithDisposedLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetAccount(account, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetAmmount_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Set the ammount of the split.
                split.SetAmmount(1m, transactionLock);

                // Assert that the Ammount property reflects the new value.
                Assert.That(split.Ammount, Is.EqualTo(1m));
            }
        }

        [Test]
        public void SetAmmount_WithEmptyLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetAmmount(1m, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetAmmount_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);
                split.SetAccount(account, lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetAmmount(1m, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetAmmount_WithDisposedLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            split.SetAccount(account, transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetAmmount(1m, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDateCleared_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Set the date of the split.
                split.SetDateCleared(DateTime.MaxValue, transactionLock);

                // Assert that the DateCleared property reflects the new value.
                Assert.That(split.DateCleared, Is.EqualTo(DateTime.MaxValue));
            }
        }

        [Test]
        public void SetDateCleared_WithEmptyLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetDateCleared(DateTime.MaxValue, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDateCleared_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);
                split.SetAccount(account, lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetDateCleared(DateTime.MaxValue, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetDateCleared_WithDisposedLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            split.SetAccount(account, transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetDateCleared(DateTime.MaxValue, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetIsReconciled_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Set the reconciled flag of the split.
                split.SetIsReconciled(true, transactionLock);

                // Assert that the IsReconciled property reflects the new value.
                Assert.That(split.IsReconciled, Is.EqualTo(true));
            }
        }

        [Test]
        public void SetIsReconciled_WithEmptyLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetIsReconciled(true, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetIsReconciled_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);
                split.SetAccount(account, lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetIsReconciled(true, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetIsReconciled_WithDisposedLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            split.SetAccount(account, transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetIsReconciled(true, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetTransactionAmmount_WhenLockIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Set the transaction ammount of the split.
                split.SetTransactionAmmount(1m, transactionLock);

                // Assert that the TransactionAmmount property reflects the new value.
                Assert.That(split.TransactionAmmount, Is.EqualTo(1m));
            }
        }

        [Test]
        public void SetTransactionAmmount_WithEmptyLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetTransactionAmmount(1m, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetTransactionAmmount_WithInvalidLock_ThrowsException()
        {
            // Create two new, empty transactions.
            var transaction1 = TestUtils.CreateEmptyTransaction();
            var transaction2 = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);
                split.SetAccount(account, lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetTransactionAmmount(1m, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetTransactionAmmount_WithDisposedLock_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            split.SetAccount(account, transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetTransactionAmmount(1m, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}

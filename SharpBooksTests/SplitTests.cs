﻿//-----------------------------------------------------------------------
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
        private readonly Commodity testCurrency = new Commodity(
            CommodityType.Currency,
            "Test Currency",
            "XTS",
            "{0}");

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
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);

                // Set the ammount and transaction ammount to be different.
                split.SetAmmount(1, transactionLock);
                split.SetTransactionAmmount(2, transactionLock);

                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAmmountAndTransactionAmmountSignsDiffer_ReturnsFalse()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);

                // Set the ammount and transaction ammount to have different signs.
                split.SetCommodity(this.noCurrency, transactionLock);
                split.SetAmmount(-1, transactionLock);
                split.SetTransactionAmmount(1, transactionLock);

                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void SetCommodity_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);

                // Set the commodity of the split.
                split.SetCommodity(this.noCurrency, transactionLock);

                // Assert that the Commodity property reflects the new value.
                Assert.That(split.Commodity, Is.EqualTo(this.noCurrency));
            }
        }

        [Test]
        public void SetCommodity_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetCommodity(this.noCurrency, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetCommodity_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = this.CreateValidTransaction();
            var transaction2 = this.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetCommodity(this.noCurrency, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetCommodity_WithDisposedLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetCommodity(this.noCurrency, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetAmmount_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);

                // Set the ammount of the split.
                split.SetAmmount(1m, transactionLock);

                // Assert that the Ammount property reflects the new value.
                Assert.That(split.Ammount, Is.EqualTo(1m));
            }
        }

        [Test]
        public void SetAmmount_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetAmmount(1m, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetAmmount_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = this.CreateValidTransaction();
            var transaction2 = this.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);

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
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetAmmount(1m, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetTransactionAmmount_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);

                // Set the transaction ammount of the split.
                split.SetTransactionAmmount(1m, transactionLock);

                // Assert that the TransactionAmmount property reflects the new value.
                Assert.That(split.TransactionAmmount, Is.EqualTo(1m));
            }
        }

        [Test]
        public void SetTransactionAmmount_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetTransactionAmmount(1m, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetTransactionAmmount_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = this.CreateValidTransaction();
            var transaction2 = this.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);

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
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetTransactionAmmount(1m, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDateCleared_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);

                // Set the date of the split.
                split.SetDateCleared(DateTime.MaxValue, transactionLock);

                // Assert that the DateCleared property reflects the new value.
                Assert.That(split.DateCleared, Is.EqualTo(DateTime.MaxValue));
            }
        }

        [Test]
        public void SetDateCleared_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetDateCleared(DateTime.MaxValue, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDateCleared_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = this.CreateValidTransaction();
            var transaction2 = this.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);

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
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetDateCleared(DateTime.MaxValue, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetIsReconciled_WhenLockIsValid_Succeeds()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);

                // Set the reconciled flag of the split.
                split.SetIsReconciled(true, transactionLock);

                // Assert that the IsReconciled property reflects the new value.
                Assert.That(split.IsReconciled, Is.EqualTo(true));
            }
        }

        [Test]
        public void SetIsReconciled_WithEmptyLock_ThrowsException()
        {
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();
            Split split;

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                split = transaction.AddSplit(transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetIsReconciled(true, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetIsReconciled_WithInvalidLock_ThrowsException()
        {
            // Create two new, valid transactions.
            var transaction1 = this.CreateValidTransaction();
            var transaction2 = this.CreateValidTransaction();

            // Lock the transaction for editing.
            using (var lock1 = transaction1.Lock())
            {
                // Add a split to the transaction.
                var split = transaction1.AddSplit(lock1);

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
            // Create a new, valid transaction.
            var transaction = this.CreateValidTransaction();

            // Lock the transaction, add a split, and immediately unlock the transaction.
            var transactionLock = transaction.Lock();
            var split = transaction.AddSplit(transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetIsReconciled(true, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        private Transaction CreateValidTransaction()
        {
            // Create a new transaction that is valid.
            // Guid.NewGuid() is OK here, because it is guaranteed to never return an invalid value.
            return new Transaction(Guid.NewGuid(), this.testCurrency);
        }
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="SplitTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
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
        /// Holds a valid security, based on the ISO 4217 testing currency, XXX.
        /// </summary>
        private readonly Security noCurrency = new Security(
            new Guid("729ffd07-f913-4e49-9ce6-e0852d8237b6"),
            SecurityType.Currency,
            "No Currency",
            "XXX",
            new CurrencyFormat(),
            1);

        [Test]
        public void GetIsValid_WhenAmountAndTransactionAmountDifferButSecurityIsTheSameAsTheTransactionBaseSecurity_ReturnsFalse()
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
                split.SetSecurity(account.Security, transactionLock);

                // Set the amount and transaction amount to be different.
                split.SetAmount(1, transactionLock);
                split.SetTransactionAmount(2, transactionLock);

                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAccountIsGroupingAccount_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new account with a null security.
            var account = new Account(
                Guid.NewGuid(), // OK
                AccountType.Grouping, // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);

                // Assert that the split is invalid because the account is a grouping account and may not have transaction splits assigned to it.
                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAmountAndTransactionAmountSignsDiffer_ReturnsFalse()
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
                split.SetSecurity(account.Security, transactionLock);

                // Set the amount and transaction amount to have different signs.
                split.SetAmount(-1, transactionLock);
                split.SetTransactionAmount(1, transactionLock);

                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAmountIsNotEvenMultipleOfAccountsSmallestFraction_ReturnsFalse()
        {
            // Assume that we are able to run the test.
            Assume.That(TestUtils.TestCurrency.FractionTraded % 10 == 0);

            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account with a smallest fraction of 10 times the base security's fraction traded.
            var account = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME", // OK
                TestUtils.TestCurrency.FractionTraded / 10); // OK

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);

                // Set the amount and transaction amount to one, which is one tenth of the valid amount for the account.
                split.SetAmount(1, transactionLock);
                split.SetTransactionAmount(1, transactionLock);

                Assert.False(split.IsValid);
            }
        }

        [Test]
        [TestCase(+0010)] // +$0.01
        [TestCase(+0100)] // +$0.10
        [TestCase(+1000)] // +$1.00
        [TestCase(+1230)] // +$1.23
        [TestCase(-0010)] // -$0.01
        [TestCase(-0100)] // -$0.10
        [TestCase(-1000)] // -$1.00
        [TestCase(-1230)] // -$1.23
        public void GetIsValid_WhenAmountIsEvenMultipleOfAccountsSmallestFraction_ReturnsTrue(long amount)
        {
            // Assume that we are able to run the test.
            Assume.That(TestUtils.TestCurrency.FractionTraded % 10 == 0);

            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account with a smallest fraction of 10 times the base security's fraction traded.
            var account = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME", // OK
                TestUtils.TestCurrency.FractionTraded / 10); // OK

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);

                // Set the amount and transaction amount to the test value.
                split.SetAmount(amount, transactionLock);
                split.SetTransactionAmount(amount, transactionLock);

                Assert.True(split.IsValid);
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
        public void GetIsValid_WhenSecurityIsNull_ReturnsFalse()
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

                // Assert that the split is invalid without assigning a security.
                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenSecurityIsDifferentFromAccountSecurity_ReturnsFalse()
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
                split.SetSecurity(noCurrency, transactionLock);

                // Assert that the split is invalid when the security is different from the account's.
                Assert.False(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAccountSecurityIsNullButSecuirtyIsNotNull_ReturnsTrue()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new account with a null security.
            var account = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                null, // OK
                null, // OK
                "OK_NAME",
                null); // OK

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(noCurrency, transactionLock);

                // Assert that the split is invalid without assigning an account.
                Assert.True(split.IsValid);
            }
        }

        [Test]
        public void GetIsValid_WhenAccountSecurityAndSecuirtyAreNull_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new account with a null security.
            var account = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                null, // OK
                null, // OK
                "OK_NAME",
                null); // OK

            // Lock the transaction for editing.
            using (var transactionLock = transaction.Lock())
            {
                // Add a split to the transaction.
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);

                // Assert that the split is invalid without assigning a security, regardless of whether the account's security is null.
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
        public void SetAmount_WhenLockIsValid_Succeeds()
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
                split.SetSecurity(account.Security, transactionLock);

                // Set the amount of the split.
                split.SetAmount(1, transactionLock);

                // Assert that the Amount property reflects the new value.
                Assert.That(split.Amount, Is.EqualTo(1));
            }
        }

        [Test]
        public void SetAmount_WithEmptyLock_ThrowsException()
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
                split.SetSecurity(account.Security, transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetAmount(1, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetAmount_WithInvalidLock_ThrowsException()
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
                split.SetSecurity(account.Security, lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetAmount(1, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetAmount_WithDisposedLock_ThrowsException()
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

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetAmount(1, transactionLock), Throws.InstanceOf<InvalidOperationException>());
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
                split.SetSecurity(account.Security, transactionLock);

                // Set the date of the split.
                split.SetDateCleared(DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc), transactionLock);

                // Assert that the DateCleared property reflects the new value.
                Assert.That(split.DateCleared, Is.EqualTo(DateTime.MaxValue));
            }
        }

        [Test]
        [TestCase(DateTimeKind.Local)]
        [TestCase(DateTimeKind.Unspecified)]
        public void SetDateCleared_WithNonUTCDate_ThrowsException(DateTimeKind kind)
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
                split.SetSecurity(account.Security, transactionLock);

                Assert.That(() => split.SetDateCleared(DateTime.SpecifyKind(DateTime.MaxValue, kind), transactionLock), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
                split.SetSecurity(account.Security, transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetDateCleared(DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc), null), Throws.InstanceOf<InvalidOperationException>());
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
                split.SetSecurity(account.Security, lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetDateCleared(DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc), lock2), Throws.InstanceOf<InvalidOperationException>());
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
            split.SetSecurity(account.Security, transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetDateCleared(DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc), transactionLock), Throws.InstanceOf<InvalidOperationException>());
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
                split.SetSecurity(account.Security, transactionLock);

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
                split.SetSecurity(account.Security, transactionLock);
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
                split.SetSecurity(account.Security, lock1);

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
            split.SetSecurity(account.Security, transactionLock);
            transactionLock.Dispose();

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetIsReconciled(true, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetTransactionAmount_WhenLockIsValid_Succeeds()
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
                split.SetSecurity(account.Security, transactionLock);

                // Set the transaction amount of the split.
                split.SetTransactionAmount(1, transactionLock);

                // Assert that the TransactionAmount property reflects the new value.
                Assert.That(split.TransactionAmount, Is.EqualTo(1m));
            }
        }

        [Test]
        public void SetTransactionAmount_WithEmptyLock_ThrowsException()
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
                split.SetSecurity(account.Security, transactionLock);
            }

            // Assert that the split will not allow modification without a lock.
            Assert.That(() => split.SetTransactionAmount(1, null), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetTransactionAmount_WithInvalidLock_ThrowsException()
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
                split.SetSecurity(account.Security, lock1);

                using (var lock2 = transaction2.Lock())
                {
                    // Assert that the lock from the second transaction may not be used on the first transaction to modify the split.
                    Assert.That(() => split.SetTransactionAmount(1, lock2), Throws.InstanceOf<InvalidOperationException>());
                }
            }
        }

        [Test]
        public void SetTransactionAmount_WithDisposedLock_ThrowsException()
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

            // Assert that the split will not allow modification with a disposed lock.
            Assert.That(() => split.SetTransactionAmount(1, transactionLock), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}

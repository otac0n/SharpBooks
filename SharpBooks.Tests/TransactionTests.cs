// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void AddSplit_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Assert that the AddSplit function successfully created a split.
            Assert.That(split.Transaction, Is.EqualTo(transaction));
        }

        [Test]
        public void Constructor_WhenSecurityIsNull_ThrowsException()
        {
            // Build a delegate to construct a new transaction.
            Transaction constructTransaction()
            {
                return new Transaction(
                    Guid.NewGuid(), // OK
                    null);
            }

            // Assert that calling the delegate throws an ArgumentNullException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenTransactionIdIsEmpty_ThrowsException()
        {
            // Build a delegate to construct a new transaction.
            Transaction constructTransaction()
            {
                return new Transaction(
                    Guid.Empty,
                    TestUtils.TestCurrency); // OK
            }

            // Assert that calling the delegate throws an ArgumentOutOfRangeException.
            Assert.That(constructTransaction, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

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
        public void Copy_WithTransactionInAnyState_CopiesTheTransaction()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add a split.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;
            split.Amount = 1000;
            split.TransactionAmount = 100;
            split.IsReconciled = true;
            split.DateCleared = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

            var copy = transaction.Copy();

            Assert.That(transaction.BaseSecurity, Is.EqualTo(copy.BaseSecurity));
            Assert.That(transaction.TransactionId, Is.EqualTo(copy.TransactionId));
            Assert.That(transaction.Splits[0].Account, Is.EqualTo(copy.Splits[0].Account));
            Assert.That(transaction.Splits[0].Amount, Is.EqualTo(copy.Splits[0].Amount));
            Assert.That(transaction.Splits[0].DateCleared, Is.EqualTo(copy.Splits[0].DateCleared));
            Assert.That(transaction.Splits[0].IsReconciled, Is.EqualTo(copy.Splits[0].IsReconciled));
            Assert.That(transaction.Splits[0].Security, Is.EqualTo(copy.Splits[0].Security));
            Assert.That(transaction.Splits[0].TransactionAmount, Is.EqualTo(copy.Splits[0].TransactionAmount));
        }

        [Test]
        public void GetIsValid_WithInvalidSplit_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Add a split to the transaction, but do not set the account.
            var split = transaction.AddSplit();
            Assume.That(split.IsValid, Is.False);

            // Assert that the transaction is invalid with an invalid split.
            Assert.That(transaction.IsValid, Is.False);
        }

        [Test]
        public void RemoveSplit_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Remove the split from the transaction.
            transaction.RemoveSplit(split);

            Assert.That(split.Transaction, Is.Not.EqualTo(transaction));
        }

        [Test]
        public void RemoveSplit_WithNullSplit_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Assert that trying to remove a null split throws an ArgumentNullException
            Assert.That(() => transaction.RemoveSplit(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveSplit_WithUnassociatedSplit_ThrowsException()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add and remove a split.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;
            transaction.RemoveSplit(split);

            // Assert that attempting to remove the split again throws an exception.
            Assert.That(() => transaction.RemoveSplit(split), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SetDate_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Set the date of the transaction.
            transaction.Date = DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);

            // Assert that the Date property reflects the new value.
            Assert.That(transaction.Date, Is.EqualTo(DateTime.MaxValue));
        }

        [Test]
        [TestCase(DateTimeKind.Local)]
        [TestCase(DateTimeKind.Unspecified)]
        public void SetDate_WithNonUTCDate_ThrowsException(DateTimeKind kind)
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            Assert.That(() => transaction.Date = DateTime.SpecifyKind(DateTime.MaxValue, kind), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}

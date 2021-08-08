// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Assert that the split is invalid because the account is a grouping account and may not have transaction splits assigned to it.
            Assert.False(split.IsValid);
        }

        [Test]
        public void GetIsValid_WhenAccountIsNull_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Add a split to the transaction.
            var split = transaction.AddSplit();

            // Assert that the split is invalid without assigning an account.
            Assert.False(split.IsValid);
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

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;

            // Assert that the split is invalid without assigning a security, regardless of whether the account's security is null.
            Assert.False(split.IsValid);
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

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = noCurrency;

            // Assert that the split is invalid without assigning an account.
            Assert.True(split.IsValid);
        }

        [Test]
        public void GetIsValid_WhenAmountAndTransactionAmountDifferButSecurityIsTheSameAsTheTransactionBaseSecurity_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the amount and transaction amount to be different.
            split.Amount = 1;
            split.TransactionAmount = 2;

            Assert.False(split.IsValid);
        }

        [Test]
        public void GetIsValid_WhenAmountAndTransactionAmountSignsDiffer_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the amount and transaction amount to have different signs.
            split.Amount = -1;
            split.TransactionAmount = 1;

            Assert.False(split.IsValid);
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

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the amount and transaction amount to the test value.
            split.Amount = amount;
            split.TransactionAmount = amount;

            Assert.True(split.IsValid);
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

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the amount and transaction amount to one, which is one tenth of the valid amount for the account.
            split.Amount = 1;
            split.TransactionAmount = 1;

            Assert.False(split.IsValid);
        }

        [Test]
        public void GetIsValid_WhenSecurityIsDifferentFromAccountSecurity_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;
            split.Security = noCurrency;

            // assert that the split is invalid when the security is different from the account's.
            Assert.False(split.IsValid);
        }

        [Test]
        public void GetIsValid_WhenSecurityIsNull_ReturnsFalse()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add a split to the transaction.
            var split = transaction.AddSplit();
            split.Account = account;

            // Assert that the split is invalid without assigning a security.
            Assert.False(split.IsValid);
        }

        [Test]
        public void SetAccount_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            split = transaction.AddSplit();

            // Set the account of the split.
            split.Account = account;

            // Assert that the Account property reflects the new value.
            Assert.That(split.Account, Is.EqualTo(account));
        }

        [Test]
        public void SetAmount_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the amount of the split.
            split.Amount = 1;

            // Assert that the Amount property reflects the new value.
            Assert.That(split.Amount, Is.EqualTo(1));
        }

        [Test]
        public void SetDateCleared_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the date of the split.
            split.DateCleared = DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);

            // Assert that the DateCleared property reflects the new value.
            Assert.That(split.DateCleared, Is.EqualTo(DateTime.MaxValue));
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

            // Add a split to the transaction.
            split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            Assert.That(() => split.DateCleared = DateTime.SpecifyKind(DateTime.MaxValue, kind), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SetIsReconciled_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the reconciled flag of the split.
            split.IsReconciled = true;

            // Assert that the IsReconciled property reflects the new value.
            Assert.That(split.IsReconciled, Is.EqualTo(true));
        }

        [Test]
        public void SetTransactionAmount_WhenValueIsValid_Succeeds()
        {
            // Create a new, empty transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            Split split;

            // Add a split to the transaction.
            split = transaction.AddSplit();
            split.Account = account;
            split.Security = account.Security;

            // Set the transaction amount of the split.
            split.TransactionAmount = 1;

            // Assert that the TransactionAmount property reflects the new value.
            Assert.That(split.TransactionAmount, Is.EqualTo(1m));
        }
    }
}

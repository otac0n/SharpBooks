//-----------------------------------------------------------------------
// <copyright file="BookTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class BookTests
    {
        [Test]
        [TestCase("OK_KEY", "OK_VALUE")]
        [TestCase("OK_KEY", "")]
        [TestCase("OK_KEY", null)]
        public void SetSetting_WhenValid_Succeeds(string key, string value)
        {
            // Create a new, empty book.
            var book = new Book();

            // Attempt to set the value of a setting.
            book.SetSetting(key, value);

            // The test passes, because the call to SetSetting() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void SetSetting_WhenKeyIsEmpty_ThrowsException(string key)
        {
            // Create a new, empty book.
            var book = new Book();

            // Assert that attempting to remove a setting with a null or empty key throws an ArgumentNullException exception.
            Assert.That(() => book.SetSetting(key, "OK_VALUE"), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveSetting_WhenValid_Succeeds()
        {
            // Create a new, empty book.
            var book = new Book();

            // Attempt to set the value of a setting.
            book.RemoveSetting("OK_KEY");

            // The test passes, because the call to RemoveSetting() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void RemoveSetting_WhenKeyIsEmpty_ThrowsException(string key)
        {
            // Create a new, empty book.
            var book = new Book();

            // Assert that attempting to remove a setting with a null or empty key throws an ArgumentNullException exception.
            Assert.That(() => book.RemoveSetting(key), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddSecurity_WhenSecurityIsValid_Succeeds()
        {
            // Create a new, empty book.
            var book = new Book();

            // Add the test security to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // The test passes, because the call to AddSecurity() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void AddSecurity_WhenSecurityIsValid_NotifiesOfChange()
        {
            // Create a new, empty book.
            var book = new Book();

            // Wire-up the CollectionChanged event to flag the change.
            bool called = false;
            book.SecurityAdded += (sender, e) => { called = true; };

            // Add the test security to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // Assert that the collection notified us of the change.
            Assert.That(called, Is.True);
        }

        [Test]
        public void AddSecurity_WhenSecurityIsNull_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Assert that trying to add a null security throws an ArgumentNullException.
            Assert.That(() => book.AddSecurity(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddSecurity_WhenAnotherSecurityHasTheSameTypeAndSymbol_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Add a test currency to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // Construct a new security that has the same type and symbol as the above currency.
            var security = new Security(
                Guid.NewGuid(),
                TestUtils.TestCurrency.SecurityType,
                "Duplicate",
                TestUtils.TestCurrency.Symbol,
                "{0}",
                1);

            // Assert that trying to add a security that has the same type and symbol of another security throws an InvalidOperationException.
            Assert.That(() => book.AddSecurity(security), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddSecurity_WhenAnotherSecurityHasTheSameSecurityId_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Add a test currency to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // Construct a new security that has the same type and symbol as the above currency.
            var security = new Security(
                TestUtils.TestCurrency.SecurityId,
                SecurityType.Stock,
                "Duplicate",
                "DIFFERENT_SYMBOL",
                "{0}",
                1);

            // Assert that trying to add a security that has the same SecurityId of another security throws an InvalidOperationException.
            Assert.That(() => book.AddSecurity(security), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddSecurity_DuplicateAttempts_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Add the test security to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // Assert that trying to add the security again throws an InvalidOperationException.
            Assert.That(() => book.AddSecurity(TestUtils.TestCurrency), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSecurity_WhenValid_Succeeds()
        {
            // Create a new, empty book.
            var book = new Book();

            // Add the test security to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // Remove the security from the book.
            book.RemoveSecurity(TestUtils.TestCurrency);

            // The test passes, because the call to RemoveSecurity() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void RemoveSecurity_WhenValid_NotifiesOfChange()
        {
            // Create a new, empty book.
            var book = new Book();

            // Add the test security to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // Wire-up the CollectionChanged event to flag the change.
            bool called = false;
            book.SecurityRemoved += (sender, e) => { called = true; };

            // Remove the security from the book.
            book.RemoveSecurity(TestUtils.TestCurrency);

            // Assert that the collection notified us of the change.
            Assert.That(called, Is.True);
        }

        [Test]
        public void RemoveSecurity_WhenSecurityIsNull_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Assert that trying to add a null security throws an ArgumentNullException.
            Assert.That(() => book.RemoveSecurity(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveSecurity_WhenSecurityHasNotBeenAdded_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Assert that trying to remove the security without being added throws an InvalidOperationException.
            Assert.That(() => book.RemoveSecurity(TestUtils.TestCurrency), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSecurity_WhenSecurityHasAlreadyBeenRemoved_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Add and immediately remove the test security from the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.RemoveSecurity(TestUtils.TestCurrency);

            // Assert that trying to remove the security again throws an InvalidOperationException.
            Assert.That(() => book.RemoveSecurity(TestUtils.TestCurrency), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSecurity_WhenSecurityIsUsedByAccounts_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            book.AddAccount(TestUtils.CreateValidAccount());

            // Assert that trying to remove the security while the account depends on it throws an InvalidOperationException.
            Assert.That(() => book.RemoveSecurity(TestUtils.TestCurrency), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSecurity_WhenSecurityIsUsedInSplits_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new account with no security.
            var account = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                null, // OK
                null, // OK
                "OK_NAME",
                null); // OK

            book.AddAccount(account);

            // Create a transaction that uses this split, but not as the 
            var transaction = new Transaction(Guid.NewGuid(), TestUtils.TestCurrency);
            using (var tLock = transaction.Lock())
            {
                transaction.SetDate(DateTime.Today, tLock);

                var split1 = transaction.AddSplit(tLock);
                split1.SetAccount(account, tLock);
                split1.SetSecurity(TestUtils.TestCurrency, tLock);
                split1.SetAmount(0, tLock);
            }

            book.AddTransaction(transaction);

            // Assert that trying to remove the security while the account depends on it throws an InvalidOperationException.
            Assert.That(() => book.RemoveSecurity(TestUtils.TestCurrency), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSecurity_WhenSecurityIsUsedByPriceQuoteAsSecurity_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency and stock to the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Add the price quote to the book.
            book.AddPriceQuote(priceQuote);

            // Assert that trying to remove the security while the price quote depends on it throws an InvalidOperationException.
            Assert.That(() => book.RemoveSecurity(TestUtils.TestStock), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveSecurity_WhenSecurityIsUsedByPriceQuoteAsCurrency_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency and stock to the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Add the price quote to the book.
            book.AddPriceQuote(priceQuote);

            // Assert that trying to remove the security while the price quote depends on it throws an InvalidOperationException.
            Assert.That(() => book.RemoveSecurity(TestUtils.TestCurrency), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddAccount_WhenAccountIsValidAndHasNoParent_Succeeds()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // The test passes, because the call to AddAccount() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void AddAccount_WhenValid_NotifiesOfChange()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Wire-up the CollectionChanged event to flag the change.
            bool called = false;
            book.AccountAdded += (sender, e) => { called = true; };

            // Add the account to the book.
            book.AddAccount(account);

            // Assert that the collection notified us of the change.
            Assert.That(called, Is.True);
        }

        [Test]
        public void AddAccount_WhenAccountsParentHasBeenAdded_Succeeds()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Build a parent account.
            var parent = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Add the parent account to the book.
            book.AddAccount(parent);

            // Construct the child account, passing the above account as the parent.
            var child = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                parent, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Add the child account to the book.
            book.AddAccount(child);

            // The test passes, because the call to AddAccount() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void AddAccount_WhenAccountIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Assert that trying to add a null account throws an ArgumentNullException.
            Assert.That(() => book.AddAccount(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddAccount_DuplicateAttempts_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // Assert that trying to add the account again throws an InvalidOperationException.
            Assert.That(() => book.AddAccount(account), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddAccount_WhenAccountsSecurityHasNotBeenAdded_ThrowsException()
        {
            // Create a new, empty book.
            var book = new Book();

            // Build a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Assert that trying to add the account without first adding the security throws an InvalidOperationException.
            Assert.That(() => book.AddAccount(account), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddAccount_WhenAccountsParentHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Build a parent account.
            var parent = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Construct the child account, passing the above account as the parent.
            var child = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                parent,
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that trying to add the child account throws an InvalidOperationException.
            Assert.That(() => book.AddAccount(child), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddAccount_WhenAnotherAccountHasTheSameAccountId_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account1 = TestUtils.CreateValidAccount();
            book.AddAccount(account1);

            // Construct the offending account with the same AccountId as the above account.
            var account2 = new Account(
                account1.AccountId,
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME_DIFFERENT",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Assert that trying to add the child account throws an InvalidOperationException.
            Assert.That(() => book.AddAccount(account2), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddAccount_WhenAnotherAccountHasTheSameNameAndParent_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account1 = TestUtils.CreateValidAccount();
            book.AddAccount(account1);

            // Construct the offending account with the same name and parent as the above account.
            var account2 = TestUtils.CreateValidAccount();

            // Assert that trying to add the child account throws an InvalidOperationException.
            Assert.That(() => book.AddAccount(account2), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveAccount_WhenAccountIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Assert that trying to remove a null account throws an ArgumentNullException.
            Assert.That(() => book.RemoveAccount(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveAccount_WhenAccountHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Assert that trying to remove an account that has not been added throws an InvalidOperationException.
            Assert.That(() => book.RemoveAccount(account), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveAccount_WhenAccountHasBeenAddedHasNoChildrenAndIsNotInvolvedInAnySplits_Succeeds()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

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
        public void RemoveAccount_WhenValid_NotifiesOfChange()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // Wire-up the CollectionChanged event to flag the change.
            bool called = false;
            book.AccountRemoved += (sender, e) => { called = true; };

            // Remove the account from the book.
            book.RemoveAccount(account);

            // Assert that the collection notified us of the change.
            Assert.That(called, Is.True);
        }

        [Test]
        public void RemoveAccount_WhenAccountHasAlreadyBeenRemoved_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

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
            var book = TestUtils.CreateValidBook();

            // Build a parent account.
            var parent = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                null, // OK
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Add the parent account to the book.
            book.AddAccount(parent);

            // Construct the child account, passing the above account as the parent.
            var child = new Account(
                Guid.NewGuid(), // OK
                AccountType.Balance, // OK
                TestUtils.TestCurrency, // OK
                parent,
                "OK_NAME",
                TestUtils.TestCurrency.FractionTraded); // OK

            // Add the child account to the book.
            book.AddAccount(child);

            // Assert that trying to remove the parent account throws an InvalidOperationException.
            Assert.That(() => book.RemoveAccount(parent), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveAccount_WhenAccountIsInvolvedInSplits_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account
            var account = TestUtils.CreateValidAccount();

            // Add the account to the book.
            book.AddAccount(account);

            // Create a new, valid transaction that depends on the account.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Add the transaction to the book.
            book.AddTransaction(transaction);

            // Assert that trying to remove the account while a transaction that depends on it is in the book throws an InvalidOperationException.
            Assert.That(() => book.RemoveAccount(account), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddTransaction_WhenTransactionIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Assert that trying to add a null transaction throws an ArgumentNullException.
            Assert.That(() => book.AddTransaction(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddTransaction_WhenTransactionIsInvalid_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, empty (and therefore, invalid) transaction.
            var transaction = TestUtils.CreateEmptyTransaction();

            // Assert that trying to add an invalid transaction throws an InvalidOperationException.
            Assert.That(() => book.AddTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddTransaction_WhenTransactionIsLocked_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

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
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Create a new, valid transaction that depends on the account.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Assert that trying to add a transaction without adding the account throws an InvalidOperationException.
            Assert.That(() => book.AddTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddTransaction_WhenAnotherTransactionHasTheSameTransactionId_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction and add it to the book.
            var transaction1 = TestUtils.CreateValidTransaction(account);
            book.AddTransaction(transaction1);

            // Construct a new, valid transaction that shares its TransactionId with transaction1.
            var transaction2 = new Transaction(
                transaction1.TransactionId,
                TestUtils.TestCurrency); // OK
            using (var transactionLock = transaction2.Lock())
            {
                var split = transaction2.AddSplit(transactionLock);
                split.SetAccount(account, transactionLock);
                split.SetSecurity(account.Security, transactionLock);
            }

            // Assert that trying to add the transaction throws an InvalidOperationException.
            Assert.That(() => book.AddTransaction(transaction2), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddTransaction_WhenTheTransactionHasPreviouslyBeenRemoved_Succeeds()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction and add it to the book.
            var transaction1 = TestUtils.CreateValidTransaction(account);
            book.AddTransaction(transaction1);

            // Remove the transaction.
            book.RemoveTransaction(transaction1);

            // Add the transaction again.
            book.AddTransaction(transaction1);

            // The test passes, because the call to AddTransaction() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void AddTransaction_DuplicateAttempts_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Add the transaction to the book.
            book.AddTransaction(transaction);

            // Assert that trying to add a transaction that has already been added throws an InvalidOperationException.
            Assert.That(() => book.AddTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveTransaction_WhenTransactionIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Assert that trying to remove a null transaction throws an ArgumentNullException.
            Assert.That(() => book.RemoveTransaction(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveTransaction_WhenValid_Succeeds()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

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
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Assert that trying to remove a transaction that has not been added throws an InvalidOperationException.
            Assert.That(() => book.RemoveTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemoveTransaction_WhenTransactionHasAlreadyBeenRemoved_ThrowsException()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Add and immediately remove the transaction from the book.
            book.AddTransaction(transaction);
            book.RemoveTransaction(transaction);

            // Assert that trying to remove a transaction that has already been removed throws an InvalidOperationException.
            Assert.That(() => book.RemoveTransaction(transaction), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddPriceQuote_WhenPriceQuoteIsValid_Succeeds()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency and stock to the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Attempt to add the price quote to the book.
            book.AddPriceQuote(priceQuote);

            // The test passes, because the call to AddAccount() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void AddPriceQuote_WhenPriceQuoteIsValid_NotifiesOfChange()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency and stock to the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Wire-up the CollectionChanged event to flag the change.
            bool called = false;
            book.PriceQuoteAdded += (sender, e) => { called = true; };

            // Attempt to add the price quote to the book.
            book.AddPriceQuote(priceQuote);

            // Assert that the collection notified us of the change.
            Assert.That(called, Is.True);
        }

        [Test]
        public void AddPriceQuote_WhenPriceQuoteHasAlreadyBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency and stock to the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities and add it to the book.
            var priceQuote = TestUtils.CreateValidPriceQuote();
            book.AddPriceQuote(priceQuote);

            // Assert that attempting to add the price quote again throws an InvalidOperationException.
            Assert.That(() => book.AddPriceQuote(priceQuote), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddPriceQuote_WhenAnotherPriceQuoteWithSameSecurityCurrencySourceAndDateHasBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency and stock to the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities and add it to the book.
            var priceQuote1 = TestUtils.CreateValidPriceQuote();
            book.AddPriceQuote(priceQuote1);

            // Create a new, valid price quote that is identical to the above quote.
            var priceQuote2 = TestUtils.CreateValidPriceQuote();

            // Assert that attempting to add the second price quote when the first price quote duplicates it throws an InvalidOperationException.
            Assert.That(() => book.AddPriceQuote(priceQuote2), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddPriceQuote_WhenAnotherPriceQuoteWithSamePriceQuoteIdHasBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency and stock to the book.
            book.AddSecurity(TestUtils.TestCurrency);
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities and add it to the book.
            var priceQuote1 = TestUtils.CreateValidPriceQuote();
            book.AddPriceQuote(priceQuote1);

            // Create a new, valid price quote that has the same price quote id as the above quote.
            var priceQuote2 = new PriceQuote(
                priceQuote1.PriceQuoteId,
                DateTime.MinValue, // OK
                TestUtils.TestStock, // OK
                1, // OK
                TestUtils.TestCurrency, // OK
                1, // OK
                "OK_SOURCE");

            // Assert that attempting to add the second price quote when the first price quote duplicates it throws an InvalidOperationException.
            Assert.That(() => book.AddPriceQuote(priceQuote2), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddPriceQuote_WhenPriceQuoteIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Assert that attempting to add a price quote of null throws an ArgumentNullException.
            Assert.That(() => book.AddPriceQuote(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddPriceQuote_WhenSecurityHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test currency (but not a test stock) to the book.
            book.AddSecurity(TestUtils.TestCurrency);

            // Create a new, valid price quote based on the above securities.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Assert that attempting to add the price quote without adding the security to the book throws an InvalidOperationException.
            Assert.That(() => book.AddPriceQuote(priceQuote), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AddPriceQuote_WhenCurrencyHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test stock (but not a test currency) to the book.
            book.AddSecurity(TestUtils.TestStock);

            // Create a new, valid price quote based on the above securities.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Assert that attempting to add the price quote without adding the currency to the book throws an InvalidOperationException.
            Assert.That(() => book.AddPriceQuote(priceQuote), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemovePriceQuote_WhenValid_Succeeds()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test stock and currency to the book.
            book.AddSecurity(TestUtils.TestStock);
            book.AddSecurity(TestUtils.TestCurrency);

            // Create a new, valid price quote based on the above securities and add it to the book.
            var priceQuote = TestUtils.CreateValidPriceQuote();
            book.AddPriceQuote(priceQuote);

            // Remove the price quote from the book.
            book.RemovePriceQuote(priceQuote);

            // The test passes, because the call to RemovePriceQuote() has completed successfully.
            Assert.True(true);  // Assert.Pass() was not used, to maintain compatibility with ReSharper.
        }

        [Test]
        public void RemovePriceQuote_WhenValid_NotifiesOfChange()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test stock and currency to the book.
            book.AddSecurity(TestUtils.TestStock);
            book.AddSecurity(TestUtils.TestCurrency);

            // Create a new, valid price quote based on the above securities and add it to the book.
            var priceQuote = TestUtils.CreateValidPriceQuote();
            book.AddPriceQuote(priceQuote);

            // Wire-up the CollectionChanged event to flag the change.
            bool called = false;
            book.PriceQuoteRemoved += (sender, e) => { called = true; };

            // Remove the price quote from the book.
            book.RemovePriceQuote(priceQuote);

            // Assert that the collection notified us of the change.
            Assert.That(called, Is.True);
        }

        [Test]
        public void RemovePriceQuote_WhenPriceQuoteIsNull_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Assert that attempting to remove a null price quote throws an ArgumentNullException.
            Assert.That(() => book.RemovePriceQuote(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RemovePriceQuote_WhenPriceQuoteHasNotBeenAdded_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test stock and currency to the book.
            book.AddSecurity(TestUtils.TestStock);
            book.AddSecurity(TestUtils.TestCurrency);

            // Create a new, valid price quote.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Assert that attempting to remove a price quote that has not been added throws an InvalidOperationException.
            Assert.That(() => book.RemovePriceQuote(priceQuote), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void RemovePriceQuote_WhenPriceQuoteHasAlreadyBeenRemoved_ThrowsException()
        {
            // Create a new, valid book.
            var book = new Book();

            // Add a test stock and currency to the book.
            book.AddSecurity(TestUtils.TestStock);
            book.AddSecurity(TestUtils.TestCurrency);

            // Create a new, valid price quote.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Add and immediately remove the price quote from the book.
            book.AddPriceQuote(priceQuote);
            book.RemovePriceQuote(priceQuote);

            // Assert that attempting to remove a price quote that has already been removed throws an InvalidOperationException.
            Assert.That(() => book.RemovePriceQuote(priceQuote), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}

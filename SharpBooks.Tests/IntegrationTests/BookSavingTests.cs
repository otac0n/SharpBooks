//-----------------------------------------------------------------------
// <copyright file="BookSavingTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests.IntegrationTests
{
    using System;
    using NUnit.Framework;
    using SharpBooks.Tests.Fakes;

    [TestFixture]
    public class BookSavingTests
    {
        [Test]
        public void CreateSavePoint_WhenCalled_Succeeds()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Request a save point from the book.
            using (var savePoint = book.CreateSavePoint())
            {
                // Assert that an actual save point was returned.
                Assert.That(savePoint, Is.Not.Null);
            }
        }

        [Test]
        public void Replay_WhenSavePointIsNull_ReplaysFromTheBeginning()
        {
            // Create a new, valid book.
            var book = new Book();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Get two valid securities.
            var security1 = TestUtils.TestCurrency;
            var security2 = TestUtils.TestStock;

            // Create a new, valid price quote based on the above securities.
            var priceQuote = TestUtils.CreateValidPriceQuote();

            // Add and immediately remove the security, account, and transaction from the book.
            book.AddSecurity(security1);
            book.AddSecurity(security2);
            book.AddAccount(account);
            book.AddPriceQuote(priceQuote);
            book.RemovePriceQuote(priceQuote);
            book.AddTransaction(transaction);
            book.RemoveTransaction(transaction);
            book.RemoveAccount(account);
            book.RemoveSecurity(security2);
            book.RemoveSecurity(security1);

            // Create a new mock data adapter and replay the book changes into the mock.
            var destination = new MockSaver();
            book.Replay(destination, null);

            // Assert that the mock received the proper additional and removals of each component.
            Assert.That(destination.SecurityAdditions.Count, Is.EqualTo(2));
            Assert.That(destination.SecurityRemovals.Count, Is.EqualTo(2));
            Assert.That(destination.PriceQuoteAdditions.Count, Is.EqualTo(1));
            Assert.That(destination.PriceQuoteRemovals.Count, Is.EqualTo(1));
            Assert.That(destination.AccountAdditions.Count, Is.EqualTo(1));
            Assert.That(destination.AccountRemovals.Count, Is.EqualTo(1));
            Assert.That(destination.TransactionAdditions.Count, Is.EqualTo(1));
            Assert.That(destination.TransactionRemovals.Count, Is.EqualTo(1));
        }

        [Test]
        public void Replay_WhenSavePointIsValid_ReplaysFromTheSavedPoint()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account.
            var account = TestUtils.CreateValidAccount();

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Add and immediately remove the account and transaction from the book.
            book.AddAccount(account);
            book.AddTransaction(transaction);
            var savePoint = book.CreateSavePoint();
            book.RemoveTransaction(transaction);
            book.RemoveAccount(account);

            // Create a new mock data adapter and replay the book changes into the mock.
            var destination = new MockSaver();
            book.Replay(destination, savePoint);

            // Assert that the mock received one addition and one removal of each an account and a transaction.
            Assert.That(destination.SecurityAdditions.Count, Is.EqualTo(0));
            Assert.That(destination.SecurityRemovals.Count, Is.EqualTo(0));
            Assert.That(destination.AccountAdditions.Count, Is.EqualTo(0));
            Assert.That(destination.AccountRemovals.Count, Is.EqualTo(1));
            Assert.That(destination.TransactionAdditions.Count, Is.EqualTo(0));
            Assert.That(destination.TransactionRemovals.Count, Is.EqualTo(1));
        }

        [Test]
        public void Replay_WithInvalidSavePoint_ThrowsException()
        {
            // Create two new, valid books.
            var book1 = TestUtils.CreateValidBook();
            var book2 = TestUtils.CreateValidBook();

            // Create a stub data adapter as a destination.
            var destination = new StubSaver();

            // Create a save point in the first book.
            using (var sp1 = book1.CreateSavePoint())
            {
                // Assert that trying to use the save point from the first book on the second book throws an InvalidOperationException.
                Assert.That(() => book2.Replay(destination, sp1), Throws.InstanceOf<InvalidOperationException>());
            }
        }

        [Test]
        public void Replay_WithDisposedSavePoint_ThrowsException()
        {
            // Create a new, valid books.
            var book = TestUtils.CreateValidBook();

            // Create a stub data adapter as a destination.
            var destination = new StubSaver();

            // Create a save point in the first book and dispose it.
            var savePoint = book.CreateSavePoint();
            savePoint.Dispose();

            // Assert that trying to use the disposed save point throws an InvalidOperationException.
            Assert.That(() => book.Replay(destination, savePoint), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="BookSavingTests.cs" company="(none)">
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
        public void Replay_WhenSavePointIsNull_ReplaysFromTheBegining()
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
            book.RemoveTransaction(transaction);
            book.RemoveAccount(account);

            // Create a new mock data adapter and replay the book changes into the mock.
            var destination = new MockDataAdapter();
            book.Replay(destination, null);

            // Assert that the mock recieved one addition and one removal of each an account and a transaciton.
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
            var destination = new MockDataAdapter();
            book.Replay(destination, savePoint);

            // Assert that the mock recieved one addition and one removal of each an account and a transaciton.
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
            var destination = new StubDataAdapter();

            // Create a save point in the first book.
            using(var sp1 = book1.CreateSavePoint())
            {
                // Assert that trying to use the save point from the first book on the second book throws an InvalidOperationException.
                Assert.That(() => book2.Replay(destination, sp1), Throws.InstanceOf<InvalidOperationException>());
            }
        }
    }
}

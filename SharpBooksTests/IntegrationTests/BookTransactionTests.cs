//-----------------------------------------------------------------------
// <copyright file="BookTransactionTests.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests.IntegrationTests
{
    using NUnit.Framework;

    [TestFixture]
    public class BookTransactionTests
    {
        [Test]
        public void GetIsLocked_WhenAddedToABook_ReturnsTrue()
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

            // Assert that the book locks the transaction.
            Assert.That(transaction.IsLocked, Is.True);
        }

        [Test]
        public void GetIsLocked_WhenRemovedFromABook_ReturnsFalse()
        {
            // Create a new, valid book.
            var book = TestUtils.CreateValidBook();

            // Create a new, valid account and add it to the book.
            var account = TestUtils.CreateValidAccount();
            book.AddAccount(account);

            // Create a new, valid transaction.
            var transaction = TestUtils.CreateValidTransaction(account);

            // Add and immediately remove transaction from the book.
            book.AddTransaction(transaction);
            book.RemoveTransaction(transaction);

            // Assert that the book unlocks the transaction once it is removed.
            Assert.That(transaction.IsLocked, Is.False);
        }
    }
}

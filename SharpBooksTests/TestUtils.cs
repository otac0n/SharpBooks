//-----------------------------------------------------------------------
// <copyright file="TestUtils.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;

    internal static class TestUtils
    {
        /// <summary>
        /// Holds a valid security, based on the ISO 4217 testing currency, XTS.
        /// </summary>
        public static readonly Security TestCurrency = new Security(
            SecurityType.Currency,
            "Test Currency",
            "XTS",
            "{0}",
            1);

        public static Transaction CreateEmptyTransaction()
        {
            // Create a new transaction that is empty, but otherwise valid.
            // Guid.NewGuid() is OK here, because it is guaranteed to never return an invalid value.
            return new Transaction(Guid.NewGuid(), TestUtils.TestCurrency);
        }

        public static Transaction CreateValidTransaction(Account splitAccouunt)
        {
            // Create a new transaction with a single, zero split assigned to splitAccount.
            var transaction = TestUtils.CreateEmptyTransaction();
            using (var transactionLock = transaction.Lock())
            {
                var split = transaction.AddSplit(transactionLock);
                split.SetAccount(splitAccouunt, transactionLock);
            }

            return transaction;
        }

        public static Account CreateValidAccount()
        {
            // Create a new account that is valid.
            // Guid.NewGuid() is OK here, because it is guaranteed to never return an invalid value.
            return new Account(Guid.NewGuid(), TestUtils.TestCurrency, null);
        }

        public static Book CreateValidBook()
        {
            // Create a new book that empty, except for the test currency.
            var book = new Book();
            book.AddSecurity(TestCurrency);
            return book;
        }
    }
}

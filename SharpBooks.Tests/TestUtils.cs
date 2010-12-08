//-----------------------------------------------------------------------
// <copyright file="TestUtils.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using System.Diagnostics;

    internal static class TestUtils
    {
        /// <summary>
        /// Holds a valid security, based on the ISO 4217 testing currency, XTS.
        /// </summary>
        public static readonly Security TestCurrency = new Security(
            new Guid("a2394d50-0b8e-4374-a66b-540a0a15767e"),
            SecurityType.Currency,
            "Test Currency",
            "XTS",
            "{0}",
            1000);

        /// <summary>
        /// Holds a valid stock, based on Google, for testing.
        /// </summary>
        public static readonly Security TestStock = new Security(
            new Guid("35228973-89e8-4abd-98e9-5523552c62ce"),
            SecurityType.Currency,
            "Test Stock",
            "GOOG",
            "{0} GOOG",
            1000);

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
            return new Account(Guid.NewGuid(), TestUtils.TestCurrency, null, "OK_NAME", TestUtils.TestCurrency.FractionTraded);
        }

        public static Book CreateValidBook()
        {
            // Create a new book that empty, except for the test currency.
            var book = new Book();
            book.AddSecurity(TestCurrency);
            return book;
        }

        public static void DisplayQuote(PriceQuote quote)
        {
            Debug.Write(quote.Security.FormatValue(quote.Quantity));
            Debug.Write(" = ");
            Debug.Write(quote.Currency.FormatValue(quote.Price));
            Debug.Write(" @ ");
            Debug.WriteLine(quote.DateTime);
        }

        public static PriceQuote CreateValidPriceQuote()
        {
            // Create a new price quote that is valid.
            // Guid.NewGuid() is OK here, because it is guaranteed to never return an invalid value.
            return new PriceQuote(Guid.NewGuid(), DateTime.MinValue, TestStock, 1, TestCurrency, 1, "TESTS");
        }
    }
}

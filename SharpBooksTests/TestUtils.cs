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
        /// Holds a valid commodity, based on the ISO 4217 testing currency, XTS.
        /// </summary>
        public static readonly Commodity TestCurrency = new Commodity(
            CommodityType.Currency,
            "Test Currency",
            "XTS",
            "{0}");

        public static Transaction CreateValidTransaction()
        {
            // Create a new transaction that is valid.
            // Guid.NewGuid() is OK here, because it is guaranteed to never return an invalid value.
            return new Transaction(Guid.NewGuid(), TestUtils.TestCurrency);
        }

        public static Account CreateValidAccount()
        {
            // Create a new account that is valid.
            // Guid.NewGuid() is OK here, because it is guaranteed to never return an invalid value.
            return new Account(Guid.NewGuid(), TestUtils.TestCurrency, null);
        }
    }
}

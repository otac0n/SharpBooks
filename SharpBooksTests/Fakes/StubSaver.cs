//-----------------------------------------------------------------------
// <copyright file="StubSaver.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests.Fakes
{
    using System;

    public class StubSaver : ISaver
    {
        public void SetSetting(string key, string value)
        {
        }

        public void RemoveSetting(string key)
        {
        }

        public void AddSecurity(SecurityData security)
        {
        }

        public void RemoveSecurity(Guid securityId)
        {
        }

        public void AddPriceQuote(PriceQuoteData priceQuote)
        {
        }

        public void RemovePriceQuote(Guid priceQuoteId)
        {
        }

        public void AddAccount(AccountData account)
        {
        }

        public void RemoveAccount(Guid accountId)
        {
        }

        public void AddTransaction(TransactionData transaction)
        {
        }

        public void RemoveTransaction(Guid transactionId)
        {
        }
    }
}

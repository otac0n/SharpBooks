// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Tests.Fakes
{
    using System;

    public class StubSaver : ISaver
    {
        public void AddAccount(AccountData account)
        {
        }

        public void AddPriceQuote(PriceQuoteData priceQuote)
        {
        }

        public void AddSecurity(SecurityData security)
        {
        }

        public void AddTransaction(TransactionData transaction)
        {
        }

        public void RemoveAccount(Guid accountId)
        {
        }

        public void RemovePriceQuote(Guid priceQuoteId)
        {
        }

        public void RemoveSecurity(Guid securityId)
        {
        }

        public void RemoveSetting(string key)
        {
        }

        public void RemoveTransaction(Guid transactionId)
        {
        }

        public void SetSetting(string key, string value)
        {
        }
    }
}

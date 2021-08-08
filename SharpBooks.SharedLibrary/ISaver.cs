// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    public interface ISaver
    {
        void AddAccount(AccountData account);

        void AddPriceQuote(PriceQuoteData priceQuote);

        void AddSecurity(SecurityData security);

        void AddTransaction(TransactionData transaction);

        void RemoveAccount(Guid accountId);

        void RemovePriceQuote(Guid priceQuoteId);

        void RemoveSecurity(Guid securityId);

        void RemoveSetting(string key);

        void RemoveTransaction(Guid transactionId);

        void SetSetting(string key, string value);
    }
}

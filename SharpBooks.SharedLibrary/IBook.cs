// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    public interface IBook : IReadOnlyBook
    {
        void AddAccount(Account account);

        void AddPriceQuote(PriceQuote priceQuote);

        void AddSecurity(Security security);

        void AddTransaction(Transaction transaction);

        IReadOnlyBook AsReadOnly();

        void RemoveAccount(Account account);

        void RemovePriceQuote(PriceQuote priceQuote);

        void RemoveSecurity(Security security);

        void RemoveSetting(string key);

        void RemoveTransaction(Transaction transaction);

        void ReplaceTransaction(Transaction oldTransaction, Transaction newTransaction);

        void SetSetting(string key, string value);
    }
}

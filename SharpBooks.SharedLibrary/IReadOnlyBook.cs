// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using SharpBooks.Events;

    public interface IReadOnlyBook
    {
        event EventHandler<AccountAddedEventArgs> AccountAdded;

        event EventHandler<AccountRemovedEventArgs> AccountRemoved;

        event EventHandler<PriceQuoteAddedEventArgs> PriceQuoteAdded;

        event EventHandler<PriceQuoteRemovedEventArgs> PriceQuoteRemoved;

        event EventHandler<SecurityAddedEventArgs> SecurityAdded;

        event EventHandler<SecurityRemovedEventArgs> SecurityRemoved;

        event EventHandler<TransactionAddedEventArgs> TransactionAdded;

        event EventHandler<TransactionRemovedEventArgs> TransactionRemoved;

        ICollection<Account> Accounts { get; }

        ICollection<PriceQuote> PriceQuotes { get; }

        ICollection<Account> RootAccounts { get; }

        ICollection<Security> Securities { get; }

        ReadOnlyDictionary<string, string> Settings { get; }

        ICollection<Transaction> Transactions { get; }

        IReadOnlyBook AsReadOnly();

        SavePoint CreateSavePoint();

        CompositeBalance GetAccountBalance(Account account);

        ICollection<Split> GetAccountSplits(Account account);

        CompositeBalance GetAccountTotalBalance(Account account);

        void Replay(ISaver dataAdapter, SavePoint savePoint = null);
    }
}

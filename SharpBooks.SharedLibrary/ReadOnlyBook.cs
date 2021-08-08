// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using SharpBooks.Events;

    public class ReadOnlyBook : IReadOnlyBook
    {
        private readonly Book book;

        internal ReadOnlyBook(Book book)
        {
            this.book = book ?? throw new ArgumentNullException(nameof(book));
        }

        /// <inheritdoc/>
        public event EventHandler<AccountAddedEventArgs> AccountAdded
        {
            add => this.book.AccountAdded += value;
            remove => this.book.AccountAdded -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<AccountRemovedEventArgs> AccountRemoved
        {
            add => this.book.AccountRemoved += value;
            remove => this.book.AccountRemoved -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<PriceQuoteAddedEventArgs> PriceQuoteAdded
        {
            add => this.book.PriceQuoteAdded += value;
            remove => this.book.PriceQuoteAdded -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<PriceQuoteRemovedEventArgs> PriceQuoteRemoved
        {
            add => this.book.PriceQuoteRemoved += value;
            remove => this.book.PriceQuoteRemoved -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<SecurityAddedEventArgs> SecurityAdded
        {
            add => this.book.SecurityAdded += value;
            remove => this.book.SecurityAdded -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<SecurityRemovedEventArgs> SecurityRemoved
        {
            add => this.book.SecurityRemoved += value;
            remove => this.book.SecurityRemoved -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<TransactionAddedEventArgs> TransactionAdded
        {
            add => this.book.TransactionAdded += value;
            remove => this.book.TransactionAdded -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<TransactionRemovedEventArgs> TransactionRemoved
        {
            add => this.book.TransactionRemoved += value;
            remove => this.book.TransactionRemoved -= value;
        }

        /// <inheritdoc/>
        public ICollection<Account> Accounts => this.book.Accounts;

        /// <inheritdoc/>
        public ICollection<PriceQuote> PriceQuotes => this.book.PriceQuotes;

        /// <inheritdoc/>
        public ICollection<Account> RootAccounts => this.book.RootAccounts;

        /// <inheritdoc/>
        public ICollection<Security> Securities => this.book.Securities;

        public IDictionary<string, string> Settings => this.book.Settings;

        /// <inheritdoc/>
        ReadOnlyDictionary<string, string> IReadOnlyBook.Settings => this.book.Settings;

        /// <inheritdoc/>
        public ICollection<Transaction> Transactions => this.book.Transactions;

        /// <inheritdoc/>
        public IReadOnlyBook AsReadOnly() => this.book.AsReadOnly();

        /// <inheritdoc/>
        public SavePoint CreateSavePoint() => this.book.CreateSavePoint();

        /// <inheritdoc/>
        public CompositeBalance GetAccountBalance(Account account) => this.book.GetAccountBalance(account);

        /// <inheritdoc/>
        public ICollection<Split> GetAccountSplits(Account account) => this.book.GetAccountSplits(account);

        /// <inheritdoc/>
        public CompositeBalance GetAccountTotalBalance(Account account) => this.book.GetAccountTotalBalance(account);

        /// <inheritdoc/>
        public void Replay(ISaver dataAdapter, SavePoint savePoint = null) => this.book.Replay(dataAdapter, savePoint);
    }
}

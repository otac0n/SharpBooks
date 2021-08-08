// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using SharpBooks.Events;

    public class ReadOnlyBook
    {
        private readonly Book book;

        internal ReadOnlyBook(Book book)
        {
            this.book = book ?? throw new ArgumentNullException(nameof(book));
            this.book.AccountAdded += (o, e) => this.AccountAdded?.Invoke(this, e);
            this.book.AccountRemoved += (o, e) => this.AccountRemoved?.Invoke(this, e);
            this.book.PriceQuoteAdded += (o, e) => this.PriceQuoteAdded?.Invoke(this, e);
            this.book.PriceQuoteRemoved += (o, e) => this.PriceQuoteRemoved?.Invoke(this, e);
            this.book.SecurityAdded += (o, e) => this.SecurityAdded?.Invoke(this, e);
            this.book.SecurityRemoved += (o, e) => this.SecurityRemoved?.Invoke(this, e);
            this.book.TransactionAdded += (o, e) => this.TransactionAdded?.Invoke(this, e);
            this.book.TransactionRemoved += (o, e) => this.TransactionRemoved?.Invoke(this, e);
        }

        public event EventHandler<AccountAddedEventArgs> AccountAdded;

        public event EventHandler<AccountRemovedEventArgs> AccountRemoved;

        public event EventHandler<PriceQuoteAddedEventArgs> PriceQuoteAdded;

        public event EventHandler<PriceQuoteRemovedEventArgs> PriceQuoteRemoved;

        public event EventHandler<SecurityAddedEventArgs> SecurityAdded;

        public event EventHandler<SecurityRemovedEventArgs> SecurityRemoved;

        public event EventHandler<TransactionAddedEventArgs> TransactionAdded;

        public event EventHandler<TransactionRemovedEventArgs> TransactionRemoved;

        public ICollection<Account> Accounts => this.book.Accounts;

        public ICollection<PriceQuote> PriceQuotes => this.book.PriceQuotes;

        public ICollection<Account> RootAccounts => this.book.RootAccounts;

        public ICollection<Security> Securities => this.book.Securities;

        public IDictionary<string, string> Settings => this.book.Settings;

        public ICollection<Transaction> Transactions => this.book.Transactions;

        public ICollection<Split> GetAccountSplits(Account account)
        {
            return this.book.GetAccountSplits(account);
        }
    }
}

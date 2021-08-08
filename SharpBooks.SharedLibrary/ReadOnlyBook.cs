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
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            this.book = book;
            this.book.AccountAdded += (o, e) => this.AccountAdded.SafeInvoke(this, e);
            this.book.AccountRemoved += (o, e) => this.AccountRemoved.SafeInvoke(this, e);
            this.book.PriceQuoteAdded += (o, e) => this.PriceQuoteAdded.SafeInvoke(this, e);
            this.book.PriceQuoteRemoved += (o, e) => this.PriceQuoteRemoved.SafeInvoke(this, e);
            this.book.SecurityAdded += (o, e) => this.SecurityAdded.SafeInvoke(this, e);
            this.book.SecurityRemoved += (o, e) => this.SecurityRemoved.SafeInvoke(this, e);
            this.book.TransactionAdded += (o, e) => this.TransactionAdded.SafeInvoke(this, e);
            this.book.TransactionRemoved += (o, e) => this.TransactionRemoved.SafeInvoke(this, e);
        }

        public event EventHandler<AccountAddedEventArgs> AccountAdded;

        public event EventHandler<AccountRemovedEventArgs> AccountRemoved;

        public event EventHandler<PriceQuoteAddedEventArgs> PriceQuoteAdded;

        public event EventHandler<PriceQuoteRemovedEventArgs> PriceQuoteRemoved;

        public event EventHandler<SecurityAddedEventArgs> SecurityAdded;

        public event EventHandler<SecurityRemovedEventArgs> SecurityRemoved;

        public event EventHandler<TransactionAddedEventArgs> TransactionAdded;

        public event EventHandler<TransactionRemovedEventArgs> TransactionRemoved;

        public ICollection<Account> Accounts
        {
            get
            {
                return this.book.Accounts;
            }
        }

        public ICollection<PriceQuote> PriceQuotes
        {
            get
            {
                return this.book.PriceQuotes;
            }
        }

        public ICollection<Account> RootAccounts
        {
            get
            {
                return this.book.RootAccounts;
            }
        }

        public ICollection<Security> Securities
        {
            get
            {
                return this.book.Securities;
            }
        }

        public IDictionary<string, string> Settings
        {
            get
            {
                return this.book.Settings;
            }
        }

        public ICollection<Transaction> Transactions
        {
            get
            {
                return this.book.Transactions;
            }
        }

        public ICollection<Split> GetAccountSplits(Account account)
        {
            return this.book.GetAccountSplits(account);
        }
    }
}

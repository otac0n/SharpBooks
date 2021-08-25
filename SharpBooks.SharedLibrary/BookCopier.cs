// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Linq;

    public sealed class BookCopier : ILoader, ISaver
    {
        private readonly Book bookToCopy = new Book();

        private Book destinationBook;

        public BookCopier(Book bookToCopy)
        {
            this.bookToCopy = bookToCopy;
        }

        /// <inheritdoc/>
        public void AddAccount(AccountData account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            lock (this)
            {
                var security = this.destinationBook.Securities.Single(s => s.SecurityId == account.SecurityId);

                Account parent = null;
                if (account.ParentAccountId.HasValue)
                {
                    parent = this.destinationBook.Accounts.Single(a => a.AccountId == account.ParentAccountId);
                }

                var newAccount = new Account(
                    account.AccountId,
                    account.AccountType,
                    security,
                    parent,
                    account.Name,
                    account.SmallestFraction);

                this.destinationBook.AddAccount(
                    newAccount);
            }
        }

        /// <inheritdoc/>
        public void AddPriceQuote(PriceQuoteData priceQuote)
        {
            lock (this)
            {
                var security = this.destinationBook.Securities.Single(s => s.SecurityId == priceQuote.SecuritySecurityId);

                var currency = this.destinationBook.Securities.Single(s => s.SecurityId == priceQuote.CurrencySecurityId);

                var newPriceQuote = new PriceQuote(
                    priceQuote.PriceQuoteId,
                    priceQuote.DateTime,
                    security,
                    priceQuote.Quantity,
                    currency,
                    priceQuote.Price,
                    priceQuote.Source);

                this.destinationBook.AddPriceQuote(
                    newPriceQuote);
            }
        }

        /// <inheritdoc/>
        public void AddSecurity(SecurityData security)
        {
            if (security == null)
            {
                throw new ArgumentNullException(nameof(security));
            }

            lock (this)
            {
                var newSecurity = new Security(
                    security.SecurityId,
                    security.SecurityType,
                    security.Name,
                    security.Symbol,
                    security.Format,
                    security.FractionTraded);

                this.destinationBook.AddSecurity(newSecurity);
            }
        }

        /// <inheritdoc/>
        public void AddTransaction(TransactionData transaction)
        {
            lock (this)
            {
                var baseSecurity = this.destinationBook.Securities.Single(s => s.SecurityId == transaction.BaseSecurityId);

                var newTransaction = new Transaction(
                    transaction.TransactionId,
                    baseSecurity)
                {
                    Date = transaction.Date,
                };

                foreach (var split in transaction.Splits)
                {
                    var newSplit = newTransaction.AddSplit();

                    var account = this.destinationBook.Accounts.Single(a => a.AccountId == split.AccountId);
                    var security = this.destinationBook.Securities.Single(s => s.SecurityId == split.SecurityId);

                    newSplit.Account = account;
                    newSplit.Security = security;
                    newSplit.Amount = split.Amount;
                    newSplit.TransactionAmount = split.TransactionAmount;
                    newSplit.DateCleared = split.DateCleared;
                    newSplit.IsReconciled = split.IsReconciled;
                }

                this.destinationBook.AddTransaction(
                    newTransaction);
            }
        }

        /// <inheritdoc/>
        public Book Load()
        {
            lock (this)
            {
                try
                {
                    this.destinationBook = new Book();
                    this.bookToCopy.Replay(this, null);

                    return this.destinationBook;
                }
                finally
                {
                    this.destinationBook = null;
                }
            }
        }

        /// <inheritdoc/>
        public void RemoveAccount(Guid accountId)
        {
            lock (this)
            {
                var account = this.destinationBook.Accounts.Single(a => a.AccountId == accountId);

                this.destinationBook.RemoveAccount(
                    account);
            }
        }

        /// <inheritdoc/>
        public void RemovePriceQuote(Guid priceQuoteId)
        {
            lock (this)
            {
                var priceQuote = this.destinationBook.PriceQuotes.Single(q => q.PriceQuoteId == priceQuoteId);

                this.destinationBook.RemovePriceQuote(
                    priceQuote);
            }
        }

        /// <inheritdoc/>
        public void RemoveSecurity(Guid securityId)
        {
            lock (this)
            {
                var security = this.destinationBook.Securities.Single(s => s.SecurityId == securityId);

                this.destinationBook.RemoveSecurity(
                    security);
            }
        }

        /// <inheritdoc/>
        public void RemoveSetting(string key)
        {
            lock (this)
            {
                this.destinationBook.RemoveSetting(key);
            }
        }

        /// <inheritdoc/>
        public void RemoveTransaction(Guid transactionId)
        {
            lock (this)
            {
                var transaction = this.destinationBook.Transactions.Single(t => t.TransactionId == transactionId);

                this.destinationBook.RemoveTransaction(
                    transaction);
            }
        }

        /// <inheritdoc/>
        public void SetSetting(string key, string value)
        {
            lock (this)
            {
                this.destinationBook.SetSetting(key, value);
            }
        }
    }
}

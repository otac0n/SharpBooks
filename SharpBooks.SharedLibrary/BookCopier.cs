//-----------------------------------------------------------------------
// <copyright file="BookCopier.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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

        public void SetSetting(string key, string value)
        {
            lock (this)
            {
                this.destinationBook.SetSetting(key, value);
            }
        }

        public void RemoveSetting(string key)
        {
            lock (this)
            {
                this.destinationBook.RemoveSetting(key);
            }
        }

        public void AddSecurity(SecurityData security)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            lock (this)
            {
                var newSecurity = new Security(
                    security.SecurityId,
                    security.SecurityType,
                    security.Name,
                    security.Symbol,
                    security.SignFormat,
                    security.FractionTraded);

                this.destinationBook.AddSecurity(newSecurity);
            }
        }

        public void RemoveSecurity(Guid securityId)
        {
            lock (this)
            {
                var security = this.destinationBook.Securities.Where(s => s.SecurityId == securityId).Single();

                this.destinationBook.RemoveSecurity(
                    security);
            }
        }

        public void AddAccount(AccountData account)
        {
            lock (this)
            {
                var security = this.destinationBook.Securities.Where(s => s.SecurityId == account.SecurityId).Single();

                Account parent = null;
                if (account.ParentAccountId.HasValue)
                {
                    parent = this.destinationBook.Accounts.Where(a => a.AccountId == account.ParentAccountId).Single();
                }

                var newAccount = new Account(
                    account.AccountId,
                    security,
                    parent,
                    account.Name,
                    account.SmallestFraction);

                this.destinationBook.AddAccount(
                    newAccount);
            }
        }

        public void RemoveAccount(Guid accountId)
        {
            lock (this)
            {
                var account = this.destinationBook.Accounts.Where(a => a.AccountId == accountId).Single();

                this.destinationBook.RemoveAccount(
                    account);
            }
        }

        public void AddTransaction(TransactionData transaction)
        {
            lock (this)
            {
                var baseSecurity = this.destinationBook.Securities.Where(s => s.SecurityId == transaction.BaseSecurityId).Single();

                var newTransaction = new Transaction(
                    transaction.TransactionId,
                    baseSecurity);

                using (var tlock = newTransaction.Lock())
                {
                    newTransaction.SetDate(transaction.Date, tlock);

                    foreach (var split in transaction.Splits)
                    {
                        var newSplit = newTransaction.AddSplit(tlock);

                        var account = this.destinationBook.Accounts.Where(a => a.AccountId == split.AccountId).Single();
                        var security = this.destinationBook.Securities.Where(s => s.SecurityId == split.SecurityId).Single();

                        newSplit.SetAccount(account, tlock);
                        newSplit.SetSecurity(security, tlock);
                        newSplit.SetAmount(split.Amount, tlock);
                        newSplit.SetDateCleared(split.DateCleared, tlock);
                        newSplit.SetIsReconciled(split.IsReconciled, tlock);
                    }
                }

                this.destinationBook.AddTransaction(
                    newTransaction);
            }
        }

        public void RemoveTransaction(Guid transactionId)
        {
            lock (this)
            {
                var transaction = this.destinationBook.Transactions.Where(t => t.TransactionId == transactionId).Single();

                this.destinationBook.RemoveTransaction(
                    transaction);
            }
        }

        public void AddPriceQuote(PriceQuoteData priceQuote)
        {
            lock (this)
            {
                var security = this.destinationBook.Securities.Where(s => s.SecurityId == priceQuote.SecuritySecurityId).Single();

                var currency = this.destinationBook.Securities.Where(s => s.SecurityId == priceQuote.CurrencySecurityId).Single();

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

        public void RemovePriceQuote(Guid priceQuoteId)
        {
            lock (this)
            {
                var priceQuote = this.destinationBook.PriceQuotes.Where(q => q.PriceQuoteId == priceQuoteId).Single();

                this.destinationBook.RemovePriceQuote(
                    priceQuote);
            }
        }
    }
}

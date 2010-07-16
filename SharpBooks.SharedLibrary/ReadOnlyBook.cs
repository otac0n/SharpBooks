//-----------------------------------------------------------------------
// <copyright file="ReadOnlyBook.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;

    public class ReadOnlyBook
    {
        private readonly Book book;

        internal ReadOnlyBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException("book");
            }

            this.book = book;
        }

        public ICollection<Security> Securities
        {
            get
            {
                return this.book.Securities;
            }
        }

        public ReadOnlyObservableCollection<Account> Accounts
        {
            get
            {
                return this.book.Accounts;
            }
        }

        public ICollection<Transaction> Transactions
        {
            get
            {
                return this.book.Transactions;
            }
        }

        public ICollection<PriceQuote> PriceQuotes
        {
            get
            {
                return this.book.PriceQuotes;
            }
        }

        public ICollection<Split> GetAccountSplits(Account account)
        {
            return book.GetAccountSplits(account);
        }

        public string GetSetting(string key)
        {
            return this.book.GetSetting(key);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="Book.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Book
    {
        private readonly List<Account> accounts = new List<Account>();

        public void AddAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (account.ParentAccount != null && !this.accounts.Contains(account.ParentAccount))
            {
                throw new InvalidOperationException("You cannot add an account to a book until the account's parent has been added.");
            }

            this.accounts.Add(account);
        }

        public void RemoveAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (!this.accounts.Contains(account))
            {
                throw new InvalidOperationException("Could not remove the account from the book, because the account is not a member of the book.");
            }

            var childAccounts = from a in this.accounts
                                where a.ParentAccount == account
                                select a;

            if (childAccounts.Any())
            {
                throw new InvalidOperationException("Could not remove the account from the book, because the account currently has children.");
            }

            this.accounts.Remove(account);
        }
    }
}

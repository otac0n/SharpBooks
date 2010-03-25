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
        private readonly Dictionary<Transaction, TransactionLock> transactions = new Dictionary<Transaction, TransactionLock>();

        public void AddAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (this.accounts.Contains(account))
            {
                throw new InvalidOperationException("Could not add the account to the book, because the account already belongs to the book.");
            }

            if (account.ParentAccount != null && !this.accounts.Contains(account.ParentAccount))
            {
                throw new InvalidOperationException("Could not add the account to the book, becaues the account's parent has not been added.");
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

        public void AddTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (this.transactions.ContainsKey(transaction))
            {
                throw new InvalidOperationException("Could not add the transaction to the book, because the transaction already belongs to the book.");
            }

            this.transactions.Add(transaction, transaction.Lock());
        }
    }
}

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

            var involvedTransactions = from t in this.transactions.Keys
                                       where (from s in t.GetSplits(this.transactions[t])
                                              where s.Account == account
                                              select s).Any()
                                       select t;

            if (involvedTransactions.Any())
            {
                throw new InvalidOperationException("Could not remove the account from the book, because the account currently has splits.");
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

            if (!transaction.IsValid)
            {
                throw new InvalidOperationException("Could not add the transaction to the book, because the transaction is not valid.");
            }

            TransactionLock transactionLock = null;

            try
            {
                transactionLock = transaction.Lock();

                var splitsWithoutAccountsInBook = from s in transaction.GetSplits(transactionLock)
                                                  where !this.accounts.Contains(s.Account)
                                                  select s;

                if (splitsWithoutAccountsInBook.Any())
                {
                    throw new InvalidOperationException("Could not add the transaction to the book, becaues the transaction contains at least on split whose account has not been added.");
                }

                this.transactions.Add(transaction, transactionLock);
                transactionLock = null;
            }
            finally
            {
                if (transactionLock != null)
                {
                    transactionLock.Dispose();
                }
            }
        }

        public void RemoveTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (!this.transactions.ContainsKey(transaction))
            {
                throw new InvalidOperationException("Could not remove the transaction from the book, because the transaction is not a member of the book.");
            }

            this.transactions[transaction].Dispose();
            this.transactions.Remove(transaction);
        }
    }
}

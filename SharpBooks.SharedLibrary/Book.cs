﻿//-----------------------------------------------------------------------
// <copyright file="Book.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using SharpBooks.Events;

    public class Book
    {
        private readonly object lockMutex = new object();
        private readonly HashSet<Security> securities = new HashSet<Security>();
        private readonly HashSet<Account> accounts = new HashSet<Account>();
        private readonly HashSet<Account> rootAccounts = new HashSet<Account>();
        private readonly HashSet<PriceQuote> priceQuotes = new HashSet<PriceQuote>();
        private readonly HashSet<Guid> transactionIds = new HashSet<Guid>();
        private readonly Dictionary<Transaction, TransactionLock> transactions = new Dictionary<Transaction, TransactionLock>();
        private readonly Dictionary<SavePoint, SaveTrack> saveTracks = new Dictionary<SavePoint, SaveTrack>();
        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();
        private readonly SaveTrack baseSaveTrack = new SaveTrack();

        private readonly ReadOnlyBook readOnlyFacade;
        private readonly ICollection<Security> securitiesReadOnly;
        private readonly ICollection<Account> accountsReadOnly;
        private readonly ICollection<Account> rootAccountsReadOnly;
        private readonly ICollection<PriceQuote> priceQuotesReadOnly;
        private readonly ReadOnlyDictionary<string, string> settingsReadOnly;

        // Indexes:
        private readonly Dictionary<Account, CompositeBalance> balances = new Dictionary<Account, CompositeBalance>();
        private readonly Dictionary<Account, CompositeBalance> totalBalances = new Dictionary<Account, CompositeBalance>();

        public Book()
        {
            this.securitiesReadOnly = new ReadOnlyCollectionWrapper<Security>(this.securities);
            this.accountsReadOnly = new ReadOnlyCollectionWrapper<Account>(this.accounts);
            this.rootAccountsReadOnly = new ReadOnlyCollectionWrapper<Account>(this.rootAccounts);
            this.priceQuotesReadOnly = new ReadOnlyCollectionWrapper<PriceQuote>(this.priceQuotes);
            this.settingsReadOnly = new ReadOnlyDictionary<string, string>(this.settings);
            this.readOnlyFacade = new ReadOnlyBook(this);
        }

        public event EventHandler<AccountAddedEventArgs> AccountAdded;
        public event EventHandler<AccountRemovedEventArgs> AccountRemoved;
        public event EventHandler<PriceQuoteAddedEventArgs> PriceQuoteAdded;
        public event EventHandler<PriceQuoteRemovedEventArgs> PriceQuoteRemoved;
        public event EventHandler<SecurityAddedEventArgs> SecurityAdded;
        public event EventHandler<SecurityRemovedEventArgs> SecurityRemoved;
        public event EventHandler<TransactionAddedEventArgs> TransactionAdded;
        public event EventHandler<TransactionRemovedEventArgs> TransactionRemoved;

        public ICollection<Security> Securities
        {
            get { return this.securitiesReadOnly; }
        }

        public ICollection<Account> Accounts
        {
            get { return this.accountsReadOnly; }
        }

        public ICollection<Account> RootAccounts
        {
            get { return this.rootAccountsReadOnly; }
        }

        public ICollection<Transaction> Transactions
        {
            get { return this.transactions.Keys; }
        }

        public ICollection<PriceQuote> PriceQuotes
        {
            get { return this.priceQuotesReadOnly; }
        }

        public ReadOnlyDictionary<string, string> Settings
        {
            get { return this.settingsReadOnly; }
        }

        public ICollection<Split> GetAccountSplits(Account account)
        {
            lock (this.lockMutex)
            {
                if (account == null)
                {
                    throw new ArgumentNullException("account");
                }

                if (!this.accounts.Contains(account))
                {
                    throw new InvalidOperationException("The account specified is not a member of the book.");
                }

                var splits = new List<Split>();

                foreach (var t in this.transactions.Keys)
                {
                    splits.AddRange(t.Splits.Where(s => s.Account == account));
                }

                return splits.AsReadOnly();
            }
        }

        /// <summary>
        /// Adds a transaction to all of the respective balances.
        /// </summary>
        /// <param name="transaction">The transaction being added.</param>
        private void AddTransactionToBalances(Transaction transaction)
        {
            foreach (var split in transaction.Splits)
            {
                var acct = split.Account;
                var balance = this.balances[acct];
                var newBal = balance.CombineWith(split.Security, split.Amount, isExact: true);

                if (newBal != balance)
                {
                    this.balances[acct] = newBal;

                    while (acct != null)
                    {
                        this.totalBalances.Remove(acct);
                        acct = acct.ParentAccount;
                    }
                }
            }
        }

        /// <summary>
        /// Removes a transaction from all of the respective balances.
        /// </summary>
        /// <param name="transaction">The transaction being removed.</param>
        private void RemoveTransactionFromBalances(Transaction transaction)
        {
            foreach (var split in transaction.Splits)
            {
                var acct = split.Account;
                var balance = this.balances[acct];
                var newBal = balance.CombineWith(split.Security, -split.Amount, isExact: true);

                if (newBal != balance)
                {
                    this.balances[acct] = newBal;

                    while (acct != null)
                    {
                        this.totalBalances.Remove(acct);
                        acct = acct.ParentAccount;
                    }
                }
            }
        }

        public CompositeBalance GetAccountBalance(Account account)
        {
            lock (this.lockMutex)
            {
                if (account == null)
                {
                    throw new ArgumentNullException("account");
                }

                CompositeBalance balance;
                if (!this.balances.TryGetValue(account, out balance))
                {
                    throw new InvalidOperationException("The account specified is not a member of the book.");
                }

                return balance;
            }
        }

        public CompositeBalance GetAccountTotalBalance(Account account)
        {
            lock (this.lockMutex)
            {
                if (account == null)
                {
                    throw new ArgumentNullException("account");
                }

                CompositeBalance balance;
                if (this.totalBalances.TryGetValue(account, out balance))
                {
                    return balance;
                }

                if (!this.balances.TryGetValue(account, out balance))
                {
                    throw new InvalidOperationException("The account specified is not a member of the book.");
                }

                var subAccountBalances = from a in this.accounts
                                         where a.ParentAccount == account
                                         select this.GetAccountTotalBalance(a);

                balance = new CompositeBalance(balance, subAccountBalances);
                this.totalBalances[account] = balance;
                return balance;
            }
        }

        /// <summary>
        /// Sets the value of a setting.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetSetting(string key, string value)
        {
            lock (this.lockMutex)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }

                this.settings[key] = value;
                this.UpdateSaveTracks(st => st.SetSetting(key, value));
            }
        }

        /// <summary>
        /// Removes a setting from the <see cref="Book"/>.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        public void RemoveSetting(string key)
        {
            lock (this.lockMutex)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }

                this.settings.Remove(key);
                this.UpdateSaveTracks(st => st.RemoveSetting(key));
            }
        }

        /// <summary>
        /// Adds a security to the <see cref="Book"/>.
        /// </summary>
        /// <param name="security">The security to add.</param>
        public void AddSecurity(Security security)
        {
            lock (this.lockMutex)
            {
                if (security == null)
                {
                    throw new ArgumentNullException("security");
                }

                if (this.securities.Contains(security))
                {
                    throw new InvalidOperationException("Could not add the security to the book, because the security already belongs to the book.");
                }

                var duplicateIds = from s in this.securities
                                   where s.SecurityId == security.SecurityId
                                   select s;

                if (duplicateIds.Any())
                {
                    throw new InvalidOperationException("Could not add the security to the book, because another security has already been added with the same Security Id.");
                }

                var duplicateData = from s in this.securities
                                    where s.SecurityType == security.SecurityType
                                    where string.Equals(s.Symbol, security.Symbol, StringComparison.OrdinalIgnoreCase)
                                    select s;

                if (duplicateData.Any())
                {
                    throw new InvalidOperationException(
                        "Could not add the security to the book, because another security has already been added with the same Security Type and Symbol.");
                }

                this.securities.Add(security);
                this.UpdateSaveTracks(st => st.AddSecurity(new SecurityData(security)));
            }

            this.SecurityAdded.SafeInvoke(this, new SecurityAddedEventArgs(security));
        }

        /// <summary>
        /// Removes a security from the <see cref="Book"/>.
        /// </summary>
        /// <param name="security">The security to remove.</param>
        public void RemoveSecurity(Security security)
        {
            lock (this.lockMutex)
            {
                if (security == null)
                {
                    throw new ArgumentNullException("security");
                }

                if (!this.securities.Contains(security))
                {
                    throw new InvalidOperationException("Could not remove the security from the book, because the security is not a member of the book.");
                }

                var dependantAccounts = from a in this.accounts
                                        where a.Security == security
                                        select a;

                if (dependantAccounts.Any())
                {
                    throw new InvalidOperationException("Could not remove the security from the book, because at least one account depends on it.");
                }

                var dependantSplits = from t in this.transactions.Keys
                                      from s in t.Splits
                                      where s.Security == security
                                      select s;

                if (dependantSplits.Any())
                {
                    throw new InvalidOperationException("Could not remove the security from the book, because at least one transaction depends on it.");
                }

                var dependantPriceQuotes = from q in this.priceQuotes
                                           where q.Security == security || q.Currency == security
                                           select q;

                if (dependantPriceQuotes.Any())
                {
                    throw new InvalidOperationException("Could not remove the security from the book, because at least one price quote depends on it.");
                }

                this.securities.Remove(security);
                this.UpdateSaveTracks(st => st.RemoveSecurity(security.SecurityId));
            }

            this.SecurityRemoved.SafeInvoke(this, new SecurityRemovedEventArgs(security));
        }

        /// <summary>
        /// Adds an account to the <see cref="Book"/>.
        /// </summary>
        /// <param name="account">The account to add.</param>
        public void AddAccount(Account account)
        {
            lock (this.lockMutex)
            {
                if (account == null)
                {
                    throw new ArgumentNullException("account");
                }

                if (this.accounts.Contains(account))
                {
                    throw new InvalidOperationException("Could not add the account to the book, because the account already belongs to the book.");
                }

                if (account.Security != null && !this.securities.Contains(account.Security))
                {
                    throw new InvalidOperationException("Could not add the account to the book, because the account's security has not been added.");
                }

                if (account.ParentAccount != null && !this.accounts.Contains(account.ParentAccount))
                {
                    throw new InvalidOperationException("Could not add the account to the book, because the account's parent has not been added.");
                }

                var duplicateIds = from a in this.accounts
                                   where a.AccountId == account.AccountId
                                   select a;

                if (duplicateIds.Any())
                {
                    throw new InvalidOperationException("Could not add the account to the book, because another account has already been added with the same Account Id.");
                }

                var duplicateNames = from a in this.accounts
                                     where a.Name == account.Name
                                     where a.ParentAccount == account.ParentAccount
                                     select a;

                if (duplicateNames.Any())
                {
                    throw new InvalidOperationException("Could not add the account to the book, because another account has already been added with the same Name and Parent.");
                }

                this.accounts.Add(account);
                if (account.ParentAccount == null)
                {
                    this.rootAccounts.Add(account);
                }

                this.UpdateSaveTracks(st => st.AddAccount(new AccountData(account)));
                account.Book = this;

                this.balances.Add(account, new CompositeBalance());
            }

            this.AccountAdded.SafeInvoke(this, new AccountAddedEventArgs(account));
        }

        /// <summary>
        /// Removes an account from the <see cref="Book"/>.
        /// </summary>
        /// <param name="account">The account to remove.</param>
        public void RemoveAccount(Account account)
        {
            lock (this.lockMutex)
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
                                           where (from s in t.Splits
                                                  where s.Account == account
                                                  select s).Any()
                                           select t;

                if (involvedTransactions.Any())
                {
                    throw new InvalidOperationException("Could not remove the account from the book, because the account currently has splits.");
                }

                this.accounts.Remove(account);
                if (account.ParentAccount == null)
                {
                    this.rootAccounts.Remove(account);
                }

                this.UpdateSaveTracks(st => st.RemoveAccount(account.AccountId));
                account.Book = null;

                this.balances.Remove(account);
                this.totalBalances.Remove(account);
            }

            this.AccountRemoved.SafeInvoke(this, new AccountRemovedEventArgs(account));
        }

        /// <summary>
        /// Adds a price quote to the <see cref="Book"/>.
        /// </summary>
        /// <param name="priceQuote">The price quote to add.</param>
        public void AddPriceQuote(PriceQuote priceQuote)
        {
            lock (this.lockMutex)
            {
                if (priceQuote == null)
                {
                    throw new ArgumentNullException("priceQuote");
                }

                if (this.priceQuotes.Contains(priceQuote))
                {
                    throw new InvalidOperationException("Could not add the price quote to the book, because the price quote already belongs to the book.");
                }

                if (!this.securities.Contains(priceQuote.Security))
                {
                    throw new InvalidOperationException("Could not add the price quote to the book, because the price quote's security has not been added.");
                }

                if (!this.securities.Contains(priceQuote.Currency))
                {
                    throw new InvalidOperationException("Could not add the price quote to the book, because the price quote's currency has not been added.");
                }

                var duplicateIds = from q in this.priceQuotes
                                   where q.PriceQuoteId == priceQuote.PriceQuoteId
                                   select q;

                if (duplicateIds.Any())
                {
                    throw new InvalidOperationException("Could not add the price quote to the book, because another price quote has already been added with the same Price Quote Id.");
                }

                var duplicateData = from q in this.priceQuotes
                                    where q.Security == priceQuote.Security
                                    where q.Currency == priceQuote.Currency
                                    where q.DateTime == priceQuote.DateTime
                                    where string.Equals(q.Source, priceQuote.Source, StringComparison.OrdinalIgnoreCase)
                                    select q;

                if (duplicateData.Any())
                {
                    throw new InvalidOperationException(
                        "Could not add the price quote to the book, because another price quote has already been added with the same Security, Currency, Date, and Source.");
                }

                this.priceQuotes.Add(priceQuote);
                this.UpdateSaveTracks(st => st.AddPriceQuote(new PriceQuoteData(priceQuote)));
            }

            this.PriceQuoteAdded.SafeInvoke(this, new PriceQuoteAddedEventArgs(priceQuote));
        }

        /// <summary>
        /// Removes a price quote from the <see cref="Book"/>.
        /// </summary>
        /// <param name="priceQuote">The price quote to remove.</param>
        public void RemovePriceQuote(PriceQuote priceQuote)
        {
            lock (this.lockMutex)
            {
                if (priceQuote == null)
                {
                    throw new ArgumentNullException("priceQuote");
                }

                if (!this.priceQuotes.Contains(priceQuote))
                {
                    throw new InvalidOperationException("Could not remove the price quote from the book, because the price quote is not a member of the book.");
                }

                this.priceQuotes.Remove(priceQuote);
                this.UpdateSaveTracks(st => st.RemovePriceQuote(priceQuote.PriceQuoteId));
            }

            this.PriceQuoteRemoved.SafeInvoke(this, new PriceQuoteRemovedEventArgs(priceQuote));
        }

        /// <summary>
        /// Adds a transaction to the <see cref="Book"/>.
        /// </summary>
        /// <param name="transaction">The transaction to add.</param>
        public void AddTransaction(Transaction transaction)
        {
            lock (this.lockMutex)
            {
                if (transaction == null)
                {
                    throw new ArgumentNullException("transaction");
                }

                if (this.transactions.ContainsKey(transaction))
                {
                    throw new InvalidOperationException("Could not add the transaction to the book, because the transaction already belongs to the book.");
                }

                if (this.transactionIds.Contains(transaction.TransactionId))
                {
                    throw new InvalidOperationException(
                        "Could not add the transaction to the book, because another transaction has already been added with the same Transaction Id.");
                }

                TransactionLock transactionLock = null;

                try
                {
                    transactionLock = transaction.Lock();

                    if (!transaction.IsValid)
                    {
                        throw new InvalidOperationException("Could not add the transaction to the book, because the transaction is not valid.");
                    }

                    var splitsWithoutAccountsInBook = from s in transaction.Splits
                                                      where !this.accounts.Contains(s.Account)
                                                      select s;

                    if (splitsWithoutAccountsInBook.Any())
                    {
                        throw new InvalidOperationException(
                            "Could not add the transaction to the book, because the transaction contains at least one split whose account has not been added.");
                    }

                    var splitsWithoutSecurityInBook = from s in transaction.Splits
                                                      where s.Account.Security == null
                                                      where !this.securities.Contains(s.Security)
                                                      select s;

                    if (splitsWithoutSecurityInBook.Any())
                    {
                        throw new InvalidOperationException(
                            "Could not add the transaction to the book, because the transaction contains at least one split whose security has not been added.");
                    }

                    this.transactions.Add(transaction, transactionLock);
                    this.transactionIds.Add(transaction.TransactionId);
                    this.UpdateSaveTracks(st => st.AddTransaction(new TransactionData(transaction)));
                    transactionLock = null;
                }
                finally
                {
                    if (transactionLock != null)
                    {
                        transactionLock.Dispose();
                    }
                }

                this.AddTransactionToBalances(transaction);
            }

            this.TransactionAdded.SafeInvoke(this, new TransactionAddedEventArgs(transaction));
        }

        /// <summary>
        /// Removes a transaction from the <see cref="Book"/>.
        /// </summary>
        /// <param name="transaction">The transaction to remove.</param>
        public void RemoveTransaction(Transaction transaction)
        {
            lock (this.lockMutex)
            {
                if (transaction == null)
                {
                    throw new ArgumentNullException("transaction");
                }

                TransactionLock transactionLock;
                if (!this.transactions.TryGetValue(transaction, out transactionLock))
                {
                    throw new InvalidOperationException("Could not remove the transaction from the book, because the transaction is not a member of the book.");
                }

                transactionLock.Dispose();
                this.transactions.Remove(transaction);
                this.transactionIds.Remove(transaction.TransactionId);
                this.UpdateSaveTracks(st => st.RemoveTransaction(transaction.TransactionId));

                this.RemoveTransactionFromBalances(transaction);
            }

            this.TransactionRemoved.SafeInvoke(this, new TransactionRemovedEventArgs(transaction));
        }

        /// <summary>
        /// Replaces a transaction in a <see cref="Book"/> with an updated copy of the same transaction.
        /// </summary>
        /// <param name="oldTransaction">The transaction that should be replaced.</param>
        /// <param name="newTransaction">The transaction that will replace the old transaction.</param>
        public void ReplaceTransaction(Transaction oldTransaction, Transaction newTransaction)
        {
            lock (this.lockMutex)
            {
                if (oldTransaction == null)
                {
                    throw new ArgumentNullException("oldTransaction");
                }

                if (newTransaction == null)
                {
                    throw new ArgumentNullException("newTransaction");
                }

                if (oldTransaction.TransactionId != newTransaction.TransactionId)
                {
                    throw new InvalidOperationException("The new transaction given may not replace the old transaction, because they do not share the same TransactionId.");
                }

                TransactionLock oldTransactionLock;
                if (!this.transactions.TryGetValue(oldTransaction, out oldTransactionLock))
                {
                    throw new InvalidOperationException("Could not remove the transaction from the book, because the transaction is not a member of the book.");
                }

                TransactionLock newTransactionLock = null;

                try
                {
                    newTransactionLock = newTransaction.Lock();

                    if (!newTransaction.IsValid)
                    {
                        throw new InvalidOperationException("Could not replace the transaction in the book, because the new transaction is not valid.");
                    }

                    var splitsWithoutAccountsInBook = from s in newTransaction.Splits
                                                      where !this.accounts.Contains(s.Account)
                                                      select s;

                    if (splitsWithoutAccountsInBook.Any())
                    {
                        throw new InvalidOperationException(
                            "Could not replace the transaction in the book, because the new transaction contains at least one split whose account has not been added.");
                    }

                    var splitsWithoutSecurityInBook = from s in newTransaction.Splits
                                                      where s.Account.Security == null
                                                      where !this.securities.Contains(s.Security)
                                                      select s;

                    if (splitsWithoutSecurityInBook.Any())
                    {
                        throw new InvalidOperationException(
                            "Could not add the transaction to the book, because the transaction contains at least one split whose security has not been added.");
                    }

                    oldTransactionLock.Dispose();

                    this.transactions.Remove(oldTransaction);
                    this.transactions.Add(newTransaction, newTransactionLock);

                    this.UpdateSaveTracks(st => st.RemoveTransaction(oldTransaction.TransactionId));
                    this.UpdateSaveTracks(st => st.AddTransaction(new TransactionData(newTransaction)));

                    newTransactionLock = null;
                }
                finally
                {
                    if (newTransactionLock != null)
                    {
                        newTransactionLock.Dispose();
                    }
                }

                this.RemoveTransactionFromBalances(oldTransaction);
                this.AddTransactionToBalances(newTransaction);
            }

            this.TransactionRemoved.SafeInvoke(this, new TransactionRemovedEventArgs(oldTransaction));
            this.TransactionAdded.SafeInvoke(this, new TransactionAddedEventArgs(newTransaction));
        }

        /// <summary>
        /// Returns a read-only wrapper for the current <see cref="Book"/>.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyBook"/> that acts a wrapper for the current <see cref="Book"/>.</returns>
        public ReadOnlyBook AsReadOnly()
        {
            return this.readOnlyFacade;
        }

        /// <summary>
        /// Creates a <see cref="SavePoint"/> that can be used to keep track of changes in the current <see cref="Book"/>.
        /// </summary>
        /// <returns>A <see cref="SavePoint"/> corresponding to the current state of the current <see cref="Book"/>.</returns>
        public SavePoint CreateSavePoint()
        {
            lock (this.lockMutex)
            {
                var savePoint = new SavePoint(this);

                this.saveTracks.Add(savePoint, new SaveTrack());

                return savePoint;
            }
        }

        /// <summary>
        /// Replays the changes in the current <see cref="Book"/> that have taken place since the <paramref name="savePoint"/> has been created.
        /// </summary>
        /// <param name="dataAdapter">The <see cref="ISaver"/> to which to replay the changes.</param>
        /// <param name="savePoint">The <see cref="SavePoint"/> from which the changes should be replayed.  If null, the entirety of the current <see cref="Book"/> will be replayed.</param>
        public void Replay(ISaver dataAdapter, SavePoint savePoint = null)
        {
            SaveTrack track;

            lock (this.lockMutex)
            {
                if (savePoint != null)
                {
                    if (!this.saveTracks.ContainsKey(savePoint))
                    {
                        throw new InvalidOperationException("Could replay the book's modifications, because the save point could not be found.");
                    }

                    track = this.saveTracks[savePoint];
                }
                else
                {
                    track = this.baseSaveTrack;
                }
            }

            lock (track)
            {
                track.Replay(dataAdapter);
            }
        }

        internal void RemoveSavePoint(SavePoint savePoint)
        {
            lock (this.lockMutex)
            {
                if (!this.saveTracks.ContainsKey(savePoint))
                {
                    throw new InvalidOperationException("Could not remove the save point, because it does not exist in the book.");
                }

                this.saveTracks.Remove(savePoint);
            }
        }

        private void UpdateSaveTracks(Action<SaveTrack> action)
        {
            lock (this.baseSaveTrack)
            {
                action.Invoke(this.baseSaveTrack);
            }

            foreach (var track in this.saveTracks)
            {
                lock (track.Value)
                {
                    action.Invoke(track.Value);
                }
            }
        }
    }
}

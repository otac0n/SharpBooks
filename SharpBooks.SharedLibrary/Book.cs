//-----------------------------------------------------------------------
// <copyright file="Book.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.ObjectModel;

    public class Book
    {
        private readonly object lockMutex = new object();
        private readonly ObservableCollection<Security> securities = new ObservableCollection<Security>();
        private readonly ObservableCollection<Account> accounts = new ObservableCollection<Account>();
        private readonly ObservableCollection<PriceQuote> priceQuotes = new ObservableCollection<PriceQuote>();
        private readonly Dictionary<Transaction, TransactionLock> transactions = new Dictionary<Transaction, TransactionLock>();
        private readonly Dictionary<SavePoint, SaveTrack> saveTracks = new Dictionary<SavePoint, SaveTrack>();
        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();
        private readonly SaveTrack baseSaveTrack = new SaveTrack();

        private readonly ReadOnlyBook readOnlyFacade;
        private readonly ReadOnlyObservableCollection<Security> securitiesReadOnly;
        private readonly ReadOnlyObservableCollection<Account> accountsReadOnly;
        private readonly ReadOnlyObservableCollection<PriceQuote> priceQuotesReadOnly;

        public Book()
        {
            this.securitiesReadOnly = new ReadOnlyObservableCollection<Security>(this.securities);
            this.accountsReadOnly = new ReadOnlyObservableCollection<Account>(this.accounts);
            this.priceQuotesReadOnly = new ReadOnlyObservableCollection<PriceQuote>(this.priceQuotes);
            this.readOnlyFacade = new ReadOnlyBook(this);
        }

        public ICollection<Security> Securities
        {
            get
            {
                lock (this.lockMutex)
                {
                    return this.securitiesReadOnly;
                }
            }
        }

        public ReadOnlyObservableCollection<Account> Accounts
        {
            get
            {
                lock (this.lockMutex)
                {
                    return this.accountsReadOnly;
                }
            }
        }

        public ICollection<Transaction> Transactions
        {
            get
            {
                lock (this.lockMutex)
                {
                    return new List<Transaction>(this.transactions.Keys).AsReadOnly();
                }
            }
        }

        public ICollection<PriceQuote> PriceQuotes
        {
            get
            {
                lock (this.lockMutex)
                {
                    return this.priceQuotesReadOnly;
                }
            }
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

                foreach (var t in this.transactions)
                {
                    splits.AddRange(t.Key.Splits.Where(s => s.Account == account));
                }

                return splits.AsReadOnly();
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

        public string GetSetting(string key)
        {
            lock (this.lockMutex)
            {
                return this.settings.ContainsKey(key) ? this.settings[key] : null;
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
        }

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

                if (!this.securities.Contains(account.Security))
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
                this.UpdateSaveTracks(st => st.AddAccount(new AccountData(account)));
            }
        }

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
                this.UpdateSaveTracks(st => st.RemoveAccount(account.AccountId));
            }
        }

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
        }

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
        }

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

                if (!transaction.IsValid)
                {
                    throw new InvalidOperationException("Could not add the transaction to the book, because the transaction is not valid.");
                }

                var duplicateIds = from t in this.transactions.Keys
                                   where t.TransactionId == transaction.TransactionId
                                   select t;

                if (duplicateIds.Any())
                {
                    throw new InvalidOperationException(
                        "Could not add the transaction to the book, because another transaction has already been added with the same Transaction Id.");
                }

                TransactionLock transactionLock = null;

                try
                {
                    transactionLock = transaction.Lock();

                    var splitsWithoutAccountsInBook = from s in transaction.Splits
                                                      where !this.accounts.Contains(s.Account)
                                                      select s;

                    if (splitsWithoutAccountsInBook.Any())
                    {
                        throw new InvalidOperationException(
                            "Could not add the transaction to the book, because the transaction contains at least on split whose account has not been added.");
                    }

                    this.transactions.Add(transaction, transactionLock);
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
            }
        }

        public void RemoveTransaction(Transaction transaction)
        {
            lock (this.lockMutex)
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
                this.UpdateSaveTracks(st => st.RemoveTransaction(transaction.TransactionId));
            }
        }

        public ReadOnlyBook AsReadOnly()
        {
            return this.readOnlyFacade;
        }

        public SavePoint CreateSavePoint()
        {
            lock (this.lockMutex)
            {
                var savePoint = new SavePoint(this);

                this.saveTracks.Add(savePoint, new SaveTrack());

                return savePoint;
            }
        }

        public void Replay(ISaver dataAdapter, SavePoint savePoint)
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

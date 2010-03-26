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
        private readonly List<Security> securities = new List<Security>();
        private readonly List<Account> accounts = new List<Account>();
        private readonly Dictionary<Transaction, TransactionLock> transactions = new Dictionary<Transaction, TransactionLock>();
        private readonly Dictionary<SavePoint, SaveTrack> saveTracks = new Dictionary<SavePoint, SaveTrack>();
        private readonly SaveTrack baseSaveTrack = new SaveTrack();

        public Book()
        {
        }

        public void AddSecurity(Security security)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            if (this.securities.Contains(security))
            {
                throw new InvalidOperationException("Could not add the security to the book, because the security already belongs to the book.");
            }

            this.securities.Add(security);
        }

        public void RemoveSecurity(Security security)
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

            this.securities.Remove(security);
        }

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

            if (!this.securities.Contains(account.Security))
            {
                throw new InvalidOperationException("Could not add the account to the book, becaues the account's security has not been added.");
            }

            if (account.ParentAccount != null && !this.accounts.Contains(account.ParentAccount))
            {
                throw new InvalidOperationException("Could not add the account to the book, becaues the account's parent has not been added.");
            }

            var duplicateIds = from a in this.accounts
                               where a.AccountId == account.AccountId
                               select a;

            if (duplicateIds.Any())
            {
                throw new InvalidOperationException("Could not add the account to the book, because another account has already been added with the same AccountId.");
            }

            this.accounts.Add(account);
            this.baseSaveTrack.AddAccount(account);
            foreach (var track in this.saveTracks)
            {
                track.Value.AddAccount(account);
            }
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
            this.baseSaveTrack.RemoveAccount(account);
            foreach (var track in this.saveTracks)
            {
                track.Value.RemoveAccount(account);
            }
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

            var duplicateIds = from t in this.transactions.Keys
                               where t.TransactionId == transaction.TransactionId
                               select t;

            if (duplicateIds.Any())
            {
                throw new InvalidOperationException("Could not add the transaction to the book, because another transaction has already been added with the same TransactionId.");
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
                this.baseSaveTrack.AddTransaction(transaction);
                foreach (var track in this.saveTracks)
                {
                    track.Value.AddTransaction(transaction);
                }
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
            this.baseSaveTrack.RemoveTransaction(transaction);
            foreach (var track in this.saveTracks)
            {
                track.Value.RemoveTransaction(transaction);
            }
        }

        public SavePoint CreateSavePoint()
        {
            var savePoint = new SavePoint();

            this.saveTracks.Add(savePoint, new SaveTrack());
            
            return savePoint;
        }

        public void Replay(IDataAdapter dataAdapter, SavePoint savePoint)
        {
            SaveTrack track;

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

            track.Replay(dataAdapter);
        }

        private class SaveTrack
        {
            private enum ActionType
            {
                AddAccount,
                RemoveAccount,
                AddTransaction,
                RemoveTransaction
            }

            private List<Action> actions = new List<Action>();

            public void Replay(IDataAdapter dataAdapter)
            {
                foreach (var action in actions)
                {
                    switch (action.ActionType)
                    {
                        case ActionType.AddAccount:
                            dataAdapter.AddAccount((AccountData)action.Item);
                            break;
                        case ActionType.RemoveAccount:
                            dataAdapter.RemoveAccount((Guid)action.Item);
                            break;
                        case ActionType.AddTransaction:
                            dataAdapter.AddTransaction((TransactionData)action.Item);
                            break;
                        case ActionType.RemoveTransaction:
                            dataAdapter.RemoveTransaction((Guid)action.Item);
                            break;
                    }
                }
            }

            public void AddAccount(Account account)
            {
                this.actions.Add(new Action
                {
                    ActionType = ActionType.AddAccount,
                    Item = new AccountData(account),
                });
            }

            public void RemoveAccount(Account account)
            {
                this.actions.Add(new Action
                {
                    ActionType = ActionType.RemoveAccount,
                    Item = account.AccountId,
                });
            }

            public void AddTransaction(Transaction transaction)
            {
                this.actions.Add(new Action
                {
                    ActionType = ActionType.AddTransaction,
                    Item = new TransactionData(transaction),
                });
            }

            public void RemoveTransaction(Transaction transaction)
            {
                this.actions.Add(new Action
                {
                    ActionType = ActionType.RemoveTransaction,
                    Item = transaction.TransactionId,
                });
            }

            private class Action
            {
                public ActionType ActionType
                {
                    get;
                    set;
                }

                public object Item
                {
                    get;
                    set;
                }
            }
        }
    }
}

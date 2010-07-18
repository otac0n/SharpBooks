﻿//-----------------------------------------------------------------------
// <copyright file="Account.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    public class Account : INotifyPropertyChanged
    {
        private readonly Guid accountId;
        private readonly Security security;
        private readonly Account parentAccount;
        private readonly string name;
        private readonly int smallestFraction;

        private readonly ObservableCollection<Account> childrenAccounts = new ObservableCollection<Account>();
        private readonly ReadOnlyObservableCollection<Account> childrenAccountsReadOnly;

        private Book book;
        private Balance balance;
        private CompositeBalance totalBalance;

        public Account(Guid accountId, Security security, Account parentAccount, string name, int smallestFraction)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("accountId");
            }

            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (smallestFraction <= 0)
            {
                throw new ArgumentOutOfRangeException("smallestFraction");
            }

            if (security.FractionTraded % smallestFraction != 0)
            {
                throw new InvalidOperationException("An account's smallest fraction must represent a whole number multiple of the units used by its security");
            }

            var parent = parentAccount;
            while (parent != null)
            {
                if (parent.AccountId == accountId)
                {
                    throw new InvalidOperationException("An account may not share an its Account Id with any of its ancestors.");
                }

                parent = parent.ParentAccount;
            }

            this.accountId = accountId;
            this.security = security;
            this.parentAccount = parentAccount;
            this.name = name;
            this.smallestFraction = smallestFraction;

            this.childrenAccountsReadOnly = new ReadOnlyObservableCollection<Account>(this.childrenAccounts);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Guid AccountId
        {
            get
            {
                return this.accountId;
            }
        }

        public Security Security
        {
            get
            {
                return this.security;
            }
        }

        public Account ParentAccount
        {
            get
            {
                return this.parentAccount;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int SmallestFraction
        {
            get
            {
                return this.smallestFraction;
            }
        }

        public ReadOnlyObservableCollection<Account> ChildrenAccounts
        {
            get
            {
                return this.childrenAccountsReadOnly;
            }
        }

        public Balance Balance
        {
            get
            {
                if (this.balance == null)
                {
                    this.balance = new Balance(
                        this.Security,
                        (from s in this.book.GetAccountSplits(this)
                         select s.Amount).Sum(),
                        true);
                }
                
                return this.balance;
            }
        }

        public CompositeBalance TotalBalance
        {
            get
            {
                if (this.totalBalance == null)
                {
                    this.totalBalance = new CompositeBalance(
                        this.Security,
                        this.Balance,
                        (from c in this.ChildrenAccounts
                         select c.TotalBalance));
                }

                return this.totalBalance;
            }
        }

        internal Book Book
        {
            get
            {
                return this.book;
            }

            set
            {
                if (this.book != value)
                {
                    if (this.book != null)
                    {
                        this.book.Accounts.CollectionChanged -= this.BookAccounts_CollectionChanged;
                        this.book.Transactions.CollectionChanged -= this.BookTransactions_CollectionChanged;
                        this.childrenAccounts.Clear();
                    }

                    this.book = value;

                    if (this.book != null)
                    {
                        this.book.Accounts.CollectionChanged += this.BookAccounts_CollectionChanged;
                        this.book.Transactions.CollectionChanged += this.BookTransactions_CollectionChanged;

                        foreach (var child in from a in this.book.Accounts
                                              where a.ParentAccount == this
                                              select a)
                        {
                            child.PropertyChanged += this.Child_PropertyChanged;
                            this.childrenAccounts.Add(child);
                        }
                    }
                }
            }
        }

        void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalBalance")
            {
                this.InvalidateTotalBalance();
            }
        }

        private void InvalidateTotalBalance()
        {
            this.totalBalance = null;

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("TotalBalance"));
            }
        }

        public static string GetAccountPath(Account account, string separator)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (account.ParentAccount == null)
            {
                return account.Name;
            }

            return GetAccountPath(account.ParentAccount, separator) + separator + account.Name;
        }

        private void BookAccounts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var removal in from Account a in e.OldItems
                                        where a.ParentAccount == this
                                        select a)
                {
                    removal.PropertyChanged -= this.Child_PropertyChanged;
                    this.childrenAccounts.Remove(removal);
                }
            }

            if (e.NewItems != null)
            {
                foreach (var addition in from Account a in e.NewItems
                                         where a.ParentAccount == this
                                         select a)
                {
                    addition.PropertyChanged += this.Child_PropertyChanged;
                    this.childrenAccounts.Add(addition);
                }
            }
        }

        private void BookTransactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool affected = false;

            if (e.OldItems != null)
            {
                if ((from Transaction t in e.OldItems
                     where (from s in t.Splits
                            where s.Account == this
                            select s).Any()
                     select t).Any())
                {
                    affected = true;
                }
            }

            if (e.NewItems != null && !affected)
            {
                if ((from Transaction t in e.NewItems
                     where (from s in t.Splits
                            where s.Account == this
                            select s).Any()
                     select t).Any())
                {
                    affected = true;
                }
            }

            this.InvalidateBalance();
        }

        private void InvalidateBalance()
        {
            this.balance = null;
            
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Balance"));
            }

            this.InvalidateTotalBalance();
        }
    }
}

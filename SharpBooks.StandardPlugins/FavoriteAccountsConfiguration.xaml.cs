//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountsConfiguration.xaml.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Newtonsoft.Json;

    /// <summary>
    /// Gathers the user's selections for the configuration of the "Favorite Accounts" widget.
    /// </summary>
    internal partial class FavoriteAccountsConfiguration : Window
    {
        private readonly ObservableCollection<AccountView> accounts = new ObservableCollection<AccountView>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteAccountsConfiguration"/> class.
        /// </summary>
        public FavoriteAccountsConfiguration()
        {
            InitializeComponent();
        }

        public ObservableCollection<AccountView> Accounts
        {
            get
            {
                return this.accounts;
            }
        }

        /// <summary>
        /// Shows the <see cref="FavoriteAccountsConfiguration"/> screen and waits for the user to make a selection.
        /// </summary>
        /// <param name="book">The book from which to read the list of accounts.</param>
        /// <param name="settings">The current settings of the "Favorite Accounts" widget.</param>
        /// <returns>The new settings for the "Favorite Accounts" widget.</returns>
        public string GetSettings(ReadOnlyBook book, string settings)
        {
            var config = FavoriteAccountsWidget.LoadSettings(settings);

            var newAccounts = from a in book.Accounts
                              let path = Account.GetAccountPath(a, config.PathSeperator)
                              let fave = (from ap in config.AccountPaths
                                          where string.Equals(ap, path, StringComparison.OrdinalIgnoreCase)
                                          select ap).Any()
                              select new AccountView
                              {
                                  Name = path,
                                  Favorite = fave,
                              };

            this.accounts.Clear();
            foreach (var a in newAccounts)
            {
                this.accounts.Add(a);
            }

            var result = this.ShowDialog();

            if (result.HasValue && result.Value)
            {
                config.AccountPaths = (from a in this.accounts
                                       where a.Favorite
                                       select a.Name).ToList();
            }

            return JsonConvert.SerializeObject(config);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Hide();
        }

        private void This_Loaded(object sender, RoutedEventArgs e)
        {
            this.DialogResult = null;
        }

        public class AccountView
        {
            public string Name
            {
                get;
                set;
            }

            public bool Favorite
            {
                get;
                set;
            }
        }
    }
}

// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Newtonsoft.Json;

    /// <summary>
    /// Gathers the user's selections for the configuration of the "Favorite Accounts" widget.
    /// </summary>
    public partial class FavoriteAccountsConfiguration : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteAccountsConfiguration"/> class.
        /// </summary>
        public FavoriteAccountsConfiguration()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows the <see cref="FavoriteAccountsConfiguration"/> screen and waits for the user to make a selection.
        /// </summary>
        /// <param name="book">The book from which to read the list of accounts.</param>
        /// <param name="settings">The current settings of the "Favorite Accounts" widget.</param>
        /// <returns>The new settings for the "Favorite Accounts" widget.</returns>
        public string GetSettings(IReadOnlyBook book, string settings)
        {
            var config = FavoriteAccountsWidget.LoadSettings(settings);

            var newAccounts = from a in book.Accounts
                              let path = a.GetPath(config.PathSeperator)
                              let fave = (from ap in config.AccountPaths
                                          where string.Equals(ap, path, StringComparison.OrdinalIgnoreCase)
                                          select ap).Any()
                              select new AccountView
                              {
                                  Name = path,
                                  Favorite = fave,
                              };

            this.accountViewBindingSource.Clear();
            foreach (var a in newAccounts)
            {
                this.accountViewBindingSource.Add(a);
            }

            var result = this.ShowDialog();

            if (result == DialogResult.OK)
            {
                config.AccountPaths = (from AccountView a in this.accountViewBindingSource
                                       where a.Favorite
                                       select a.Name).ToList();
            }

            return JsonConvert.SerializeObject(config);
        }

        internal class AccountView
        {
            public bool Favorite { get; set; }

            public string Name { get; set; }
        }
    }
}

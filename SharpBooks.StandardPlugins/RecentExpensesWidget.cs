// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using SharpBooks.Plugins;

    internal class RecentExpensesWidget : IWidget
    {
        private Control control;
        private EventProxy events;
        private RecentExpensesSettings settings;

        public RecentExpensesWidget()
        {
        }

        public static RecentExpensesSettings LoadSettings(string settings)
        {
            var config = new RecentExpensesSettings();

            if (!string.IsNullOrEmpty(settings))
            {
                try
                {
                    config = JsonConvert.DeserializeObject<RecentExpensesSettings>(settings);
                }
                catch (JsonReaderException)
                {
                }
            }

            return config;
        }

        /// <inheritdoc/>
        public string Configure(IReadOnlyBook book, string currentSettings)
        {
            ////var view = new RecentExpensesConfiguration();
            ////currentSettings = view.GetSettings(book, currentSettings);

            return currentSettings;
        }

        /// <inheritdoc/>
        public Control Create(IReadOnlyBook book, EventProxy events)
        {
            if (this.control != null)
            {
                throw new InvalidOperationException();
            }

            this.events = events;
            this.control = null; ////new PiePlot();

            this.PopulateControl(book);

            return this.control;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.control != null)
            {
                this.control = null;
            }
        }

        /// <inheritdoc/>
        public void Refresh(IReadOnlyBook book, EventProxy events)
        {
            if (this.control == null)
            {
                throw new InvalidOperationException();
            }

            this.events = events;

            this.PopulateControl(book);
        }

        public void SetConfiguration(IReadOnlyBook book, string settings)
        {
            this.settings = LoadSettings(settings);
        }

        private void Account_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var args = new AccountSelectedEventArgs
            {
                AccountId = (Guid)((Control)sender).Tag,
            };

            this.events.RaiseAccountSelected(sender, args);
        }

        private void PopulateControl(IReadOnlyBook book)
        {
            ////var accounts = from a in book.Accounts
            ////               let path = Account.GetAccountPath(a, this.settings.PathSeperator)
            ////               where (from ap in this.settings.AccountPaths
            ////                      where string.Equals(ap, path, StringComparison.OrdinalIgnoreCase)
            ////                      select ap).Any()
            ////               orderby path
            ////               select a;
            ////
            ////foreach (var account in accounts)
            ////{
            ////    var balance = book.GetAccountSplits(account).Sum(s => s.Amount);
            ////}
        }
    }
}

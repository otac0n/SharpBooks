// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using SharpBooks.Plugins;

    internal class FavoriteAccountsWidget : IWidget
    {
        private FlowLayoutPanel control;
        private EventProxy events;
        private FavoriteAccountSettings settings;

        public FavoriteAccountsWidget()
        {
        }

        public static FavoriteAccountSettings LoadSettings(string settings)
        {
            var config = new FavoriteAccountSettings();

            if (!string.IsNullOrEmpty(settings))
            {
                try
                {
                    config = JsonConvert.DeserializeObject<FavoriteAccountSettings>(settings);
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
            var view = new FavoriteAccountsConfiguration();
            currentSettings = view.GetSettings(book, currentSettings);

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
            this.control = new FlowLayoutPanel();

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
            this.control.Controls.Clear();

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
            var accounts = from a in book.Accounts
                           let path = a.GetPath(this.settings.PathSeperator)
                           where (from ap in this.settings.AccountPaths
                                  where string.Equals(ap, path, StringComparison.OrdinalIgnoreCase)
                                  select ap).Any()
                           orderby path
                           select a;

            var empty = true;

            foreach (var account in accounts)
            {
                empty = false;

                var balance = book.GetAccountSplits(account).Sum(s => s.Amount);

                ////var accountImage = new Image
                ////{
                ////    Height = 16,
                ////    Width = 16,
                ////    Source = new BitmapImage(new Uri("pack://application:,,,/SharpBooks;component/resources/Coinstack.png")),
                ////    Margin = new Thickness(5.0d, 0.0d, 2.0d, 0.0d),
                ////};

                ////var nameLabel = new Label
                ////{
                ////    Content = account.Name,
                ////    Margin = new Thickness(2.0d, 0.0d, 0.0d, 0.0d),
                ////};

                ////var amountLabel = new Label
                ////{
                ////    Content = account.Security.FormatValue(balance),
                ////    Margin = new Thickness(2.0d, 0.0d, 2.0d, 0.0d),
                ////    HorizontalContentAlignment = HorizontalAlignment.Right,
                ////    Foreground = balance >= 0 ? Brushes.Black : Brushes.Red,
                ////};
            }

            if (empty)
            {
                var emptyLabel = new Label
                {
                    Text = "You have not selected any favorite accounts.",
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                };

                this.control.Controls.Add(emptyLabel);
            }
        }

        private class Settings
        {
            public List<string> Accounts
            {
                get;
                set;
            }

            public char PathSeperator
            {
                get;
                set;
            }
        }
    }
}

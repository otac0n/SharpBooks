//-----------------------------------------------------------------------
// <copyright file="RecentExpensesWidget.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Newtonsoft.Json;
    using SharpBooks.Plugins;
    using SharpBooks.Charts;

    internal class RecentExpensesWidget : IWidget
    {
        private readonly RecentExpensesSettings settings;

        private FrameworkElement control;
        private EventProxy events;

        public RecentExpensesWidget(string settings)
        {
            this.settings = LoadSettings(settings);
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

        public FrameworkElement Create(ReadOnlyBook book, EventProxy events)
        {
            if (this.control != null)
            {
                throw new InvalidOperationException();
            }

            this.events = events;
            this.control = new PiePlot();

            this.PopulateControl(book);

            return this.control;
        }

        public void Refresh(ReadOnlyBook book, EventProxy events)
        {
            if (this.control == null)
            {
                throw new InvalidOperationException();
            }

            this.events = events;

            this.PopulateControl(book);
        }

        public void Dispose()
        {
            if (this.control != null)
            {
                this.control = null;
            }
        }

        private void PopulateControl(ReadOnlyBook book)
        {
            //var accounts = from a in book.Accounts
            //               let path = Account.GetAccountPath(a, this.settings.PathSeperator)
            //               where (from ap in this.settings.AccountPaths
            //                      where string.Equals(ap, path, StringComparison.OrdinalIgnoreCase)
            //                      select ap).Any()
            //               orderby path
            //               select a;

            //foreach (var account in accounts)
            //{
            //    var balance = book.GetAccountSplits(account).Sum(s => s.Amount);
            //}
        }

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((Grid)sender).Tag,
                };

                this.events.RaiseAccountSelected(sender, args);

                e.Handled = true;
            }
        }
    }
}

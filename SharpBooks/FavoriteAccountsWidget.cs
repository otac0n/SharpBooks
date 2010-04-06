//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountsWidget.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using SharpBooks.Plugins;
    using System.Threading;
    using System.Windows.Forms;

    internal class FavoriteAccountsWidget : IWidget
    {
        private Panel control;
        private EventProxy events;

        private readonly char pathSeperator;
        private readonly List<string> accountPaths;

        public FavoriteAccountsWidget(string settings)
        {
            this.pathSeperator = '\\';
            this.accountPaths = new List<string>();

            if (!string.IsNullOrEmpty(settings))
            {
                this.accountPaths.AddRange(settings.Split(','));
            }
        }

        public Control Create(ReadOnlyBook book, EventProxy events)
        {
            if (this.control != null)
            {
                throw new InvalidOperationException();
            }

            this.events = events;
            this.control = new Panel();

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
            this.control.Controls.Clear();

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
            var accounts = from a in book.Accounts
                           where this.accountPaths.Contains(Account.GetAccountPath(a, this.pathSeperator))
                           select a;

            //foreach (var account in accounts)
            //{
            //    var nameLabel = new Label
            //    {
            //        Text = account.Name,
            //    };

            //    var amountLabel = new Label
            //    {
            //        Text = "$0.00",
            //    };

            //    var grid = new Grid
            //    {
            //        Tag = account.AccountId,
            //    };
            //    grid.MouseLeftButtonDown += this.Account_MouseLeftButtonDown;
            //    grid.ColumnDefinitions.Add(new ColumnDefinition
            //    {
            //        Width = new GridLength(50, GridUnitType.Star),
            //    });
            //    grid.ColumnDefinitions.Add(new ColumnDefinition
            //    {
            //        Width = new GridLength(25, GridUnitType.Star),
            //    });
            //    Grid.SetColumn(nameLabel, 0);
            //    grid.Children.Add(nameLabel);
            //    Grid.SetColumn(amountLabel, 1);
            //    grid.Children.Add(amountLabel);

            //    this.control.Children.Add(grid);
            //}
        }

        private void Account_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((Panel)sender).Tag,
                };

                this.events.RaiseAccountSelected(sender, args);
            }
        }

        private class Settings
        {
            public char PathSeperator
            {
                get;
                set;
            }

            public List<string> Accounts
            {
                get;
                set;
            }
        }
    }
}

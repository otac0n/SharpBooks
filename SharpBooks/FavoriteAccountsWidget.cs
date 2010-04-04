﻿//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountsWidget.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using SharpBooks.Plugins;

    internal class FavoriteAccountsWidget : IWidget
    {
        private StackPanel control;
        private EventProxy events;
        private string settings;

        public FavoriteAccountsWidget(string settings)
        {
            this.settings = settings;
        }

        public FrameworkElement Create(ReadOnlyBook book, EventProxy events)
        {
            if (this.control != null)
            {
                throw new InvalidOperationException();
            }

            this.events = events;
            this.control = new StackPanel();

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
            this.control.Children.Clear();

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
            foreach (var account in book.Accounts)
            {
                var nameLabel = new Label
                {
                    Content = account.Name,
                };

                var amountLabel = new Label
                {
                    Content = "$0.00",
                };

                var grid = new Grid
                {
                    Tag = account.AccountId,
                };
                grid.MouseLeftButtonDown += this.Account_MouseLeftButtonDown;
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(50, GridUnitType.Star),
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(25, GridUnitType.Star),
                });
                Grid.SetColumn(nameLabel, 0);
                grid.Children.Add(nameLabel);
                Grid.SetColumn(amountLabel, 1);
                grid.Children.Add(amountLabel);

                this.control.Children.Add(grid);
            }
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
            }
        }

        private class Settings
        {
            public List<string> Accounts
            {
                get;
                set;
            }
        }
    }
}

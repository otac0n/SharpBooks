﻿//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountsWidget.cs" company="(none)">
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

    internal class FavoriteAccountsWidget : IWidget
    {
        private readonly FavoriteAccountSettings settings;

        private StackPanel control;
        private EventProxy events;

        public FavoriteAccountsWidget(string settings)
        {
            this.settings = LoadSettings(settings);
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
            var accounts = from a in book.Accounts
                           let path = Account.GetAccountPath(a, this.settings.PathSeperator)
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

                var accountImage = new Image
                {
                    Height = 16,
                    Width = 16,
                    Source = new BitmapImage(new Uri("pack://application:,,,/SharpBooks;component/resources/Coinstack.png")),
                    Margin = new Thickness(5.0d, 0.0d, 2.0d, 0.0d),
                };

                var nameLabel = new Label
                {
                    Content = account.Name,
                    Margin = new Thickness(2.0d, 0.0d, 0.0d, 0.0d),
                };

                var amountLabel = new Label
                {
                    Content = account.Security.FormatValue(balance),
                    Margin = new Thickness(2.0d, 0.0d, 2.0d, 0.0d),
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Foreground = balance >= 0 ? Brushes.Black : Brushes.Red,
                };

                var grid = new Grid
                {
                    Tag = account.AccountId,
                };
                grid.MouseLeftButtonDown += this.Account_MouseLeftButtonDown;
                grid.MouseLeftButtonUp += this.Account_MouseLeftButtonUp;

                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(23, GridUnitType.Pixel),
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Auto),
                });
                Grid.SetColumn(accountImage, 0);
                grid.Children.Add(accountImage);
                Grid.SetColumn(nameLabel, 1);
                grid.Children.Add(nameLabel);
                Grid.SetColumn(amountLabel, 2);
                grid.Children.Add(amountLabel);

                this.control.Children.Add(grid);
            }

            if (empty)
            {
                var emptyLabel = new TextBlock
                {
                    Text = "You have not selected any favorite accounts.",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };

                this.control.Children.Add(emptyLabel);
            }
        }

        private bool doubleClick;

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.doubleClick = e.ClickCount == 2;
        }

        private void Account_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.doubleClick)
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

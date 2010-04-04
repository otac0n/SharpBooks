//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountsWidget.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using SharpBooks.Plugins;
using System.Collections.Generic;

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
                var text = new TextBlock
                {
                    Text = account.Name,
                    Tag = account.AccountId,
                };

                text.MouseLeftButtonDown += this.Account_MouseLeftButtonDown;

                this.control.Children.Add(text);
            }
        }

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((TextBlock)sender).Tag,
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

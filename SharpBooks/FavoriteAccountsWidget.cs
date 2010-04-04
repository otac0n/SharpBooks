using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpBooks.Plugins;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace SharpBooks
{
    internal class FavoriteAccountsWidget : IWidget
    {
        private StackPanel control;
        private EventProxy events;

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
            var args = new AccountSelectedEventArgs
            {
                AccountId = (Guid)((TextBlock)sender).Tag,
            };

            this.events.RaiseAccountSelected(sender, args);
        }
    }
}

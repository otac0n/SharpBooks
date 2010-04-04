using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpBooks.Plugins;
using System.Windows.Controls;
using System.Windows;

namespace SharpBooks
{
    internal class FavoriteAccountsWidget : IWidget
    {
        private TextBlock control;

        public FrameworkElement Create(ReadOnlyBook book, object events)
        {
            this.control = new TextBlock
            {
                Text = "Hello, world!",
            };

            return this.control;
        }

        public void Refresh(ReadOnlyBook book)
        {
            if (this.control == null)
            {
                throw new InvalidOperationException();
            }

            this.control.Text = "Refreshed!";
        }

        public void Dispose()
        {
            if (this.control != null)
            {
                this.control = null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpBooks.Plugins;
using System.Windows.Controls;

namespace SharpBooks.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private IEnumerable<IPluginFactory> plugins;

        private Book book;
        private ReadOnlyBook readOnlyBook;

        internal Book Book
        {
            get
            {
                return this.book;
            }

            private set
            {
                this.book = value;

                this.ReadOnlyBook = this.Book.AsReadOnly();
            }
        }

        internal ReadOnlyBook ReadOnlyBook
        {
            get
            {
                return this.readOnlyBook;
            }

            private set
            {
                this.readOnlyBook = value;
            }
        }

        public MainViewModel()
        {
            this.Book = BuildFakeBook();
            this.plugins = LoadAllPlugins();
        }

        private static IEnumerable<IPluginFactory> LoadAllPlugins()
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                var appPath = System.IO.Path.GetDirectoryName(assembly.Location);

                return PluginLoader.LoadAllPlugins(appPath);
            }
            else
            {
                return null;
            }
        }

        private void AccountSelected(object sender, AccountSelectedEventArgs e)
        {
            //AccountsTabItem.IsSelected = true;
            //AccountsList.Visibility = Visibility.Hidden;
            //SplitList.Visibility = Visibility.Visible;

            var account = this.Book.Accounts.Where(a => a.AccountId == e.AccountId).Single();
            //SplitList.DataContext = from s in this.Book.GetAccountSplits(account)
            //                        select new SplitViewModel(s);
        }

        private WidgetContainer LoadWidget(string widgetName)
        {
            var widgetKey = "overview-widgets-" + widgetName;

            var factoryName = this.ReadOnlyBook.GetSetting(widgetKey + "-name");
            var factoryType = this.ReadOnlyBook.GetSetting(widgetKey + "-type");
            var widgetSettings = this.ReadOnlyBook.GetSetting(widgetKey + "-settings");

            if (!string.IsNullOrEmpty(factoryName) && !string.IsNullOrEmpty(factoryType))
            {
                var factory = (from p in this.plugins
                               let w = p as IWidgetFactory
                               where w != null
                               where w.GetType().AssemblyQualifiedName == factoryType
                               where w.Name == factoryName
                               select w).SingleOrDefault();

                var widget = factory.CreateInstance(this.ReadOnlyBook, widgetSettings);

                var events = new EventProxy(
                    this.AccountSelected);

                return new WidgetContainer
                {
                    Widget = widget.Create(this.ReadOnlyBook, events),
                    Title = factoryName,
                };
            }
            else
            {
                return new WidgetContainer
                {
                    Widget = new Label
                    {
                        Content = "Failed to load " + widgetName,
                    },
                    Title = string.IsNullOrEmpty(factoryName) ? widgetName : factoryName,
                    HasFailed = true,
                };
            }
        }

        private static Book BuildFakeBook()
        {
            var usd = new Security(Guid.NewGuid(), SecurityType.Currency, "United States dollar", "USD", "{0:$#,##0.00#;($#,##0.00#)}", 1000);
            var account1 = new Account(Guid.NewGuid(), usd, null, "Assets", 100);
            var account2 = new Account(Guid.NewGuid(), usd, account1, "My Bank Account", 100);
            var account3 = new Account(Guid.NewGuid(), usd, account1, "My Other Bank", 100);
            var account4 = new Account(Guid.NewGuid(), usd, null, "Liabilities", 100);
            var account5 = new Account(Guid.NewGuid(), usd, account4, "My Home Loan", 100);

            var transaction1 = new Transaction(Guid.NewGuid(), usd);
            using (var tlock = transaction1.Lock())
            {
                transaction1.SetDate(DateTime.Today.AddDays(-1), tlock);

                var split1 = transaction1.AddSplit(tlock);
                split1.SetAccount(account2, tlock);
                split1.SetAmount(3520, tlock);
                split1.SetTransactionAmount(3520, tlock);
                split1.SetDateCleared(DateTime.Today, tlock);

                var split2 = transaction1.AddSplit(tlock);
                split2.SetAccount(account3, tlock);
                split2.SetAmount(-3520, tlock);
                split2.SetTransactionAmount(-3520, tlock);
            }

            var transaction2 = new Transaction(Guid.NewGuid(), usd);
            using (var tlock = transaction2.Lock())
            {
                transaction2.SetDate(DateTime.Today.AddDays(-2), tlock);

                var split3 = transaction2.AddSplit(tlock);
                split3.SetAccount(account5, tlock);
                split3.SetAmount(1750, tlock);
                split3.SetTransactionAmount(1750, tlock);
                split3.SetDateCleared(DateTime.Today.AddDays(-1), tlock);

                var split4 = transaction2.AddSplit(tlock);
                split4.SetAccount(account2, tlock);
                split4.SetAmount(-1750, tlock);
                split4.SetTransactionAmount(-1750, tlock);
            }

            var book = new Book();
            book.AddSecurity(usd);
            book.AddAccount(account1);
            book.AddAccount(account2);
            book.AddAccount(account3);
            book.AddAccount(account4);
            book.AddAccount(account5);

            book.AddTransaction(transaction1);
            book.AddTransaction(transaction2);

            book.SetSetting("overview-layout", "[{\"ColumnWidth\":200,\"Widgets\":[{\"Name\":\"widget1\"},{\"Name\":\"widget2\"}]},{\"ColumnWidth\":250,\"Widgets\":[{\"Name\":\"widget3\"}]}]");
            book.SetSetting("overview-widgets-widget1-name", "Favorite Accounts");
            book.SetSetting("overview-widgets-widget1-type", "SharpBooks.StandardPlugins.FavoriteAccountsWidgetFactory, SharpBooks.StandardPlugins, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6fee4057cb920410");
            book.SetSetting("overview-widgets-widget1-settings", "{\"PathSeperator\":\"\\\\\",\"AccountPaths\":[\"Assets\\\\My Bank Account\",\"Assets\\\\My Other Bank\",\"Liabilities\\\\My Home Loan\"]}");
            book.SetSetting("overview-widgets-widget3-name", "Recent Expenses");
            book.SetSetting("overview-widgets-widget3-type", "SharpBooks.StandardPlugins.RecentExpensesWidgetFactory, SharpBooks.StandardPlugins, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6fee4057cb920410");

            return book;
        }
    }
}

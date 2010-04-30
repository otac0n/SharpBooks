namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using SharpBooks.Plugins;
    using SharpBooks.ViewModels;
    using Newtonsoft.Json;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private Book book;
        private ReadOnlyBook readOnlyBook;
        private IEnumerable<IPluginFactory> plugins;

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

        public MainView()
        {
            InitializeComponent();

            this.Book = BuildFakeBook();
            this.plugins = LoadAllPlugins();
            this.LoadAllWidgets();
            this.UpdateAccounts();
        }

        private class OverviewColumn
        {
            public int ColumnWidth { get; set; }
            public List<string> Widgets { get; set; }
        }

        private void LoadAllWidgets()
        {
            List<OverviewColumn> widgetsLayout = null;
            try
            {
                widgetsLayout = JsonConvert.DeserializeObject<List<OverviewColumn>>(this.Book.GetSetting("overview-layout"));
            }
            catch (JsonReaderException)
            {
            }

            if (widgetsLayout == null)
            {
                widgetsLayout = new List<OverviewColumn>();
                widgetsLayout.Add(new OverviewColumn
                {
                    ColumnWidth = 200,
                    Widgets = null,
                });
            }

            int columnNumber = 0;
            foreach (var widgetColumn in widgetsLayout)
            {
                var panel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                if (widgetColumn.Widgets != null)
                {
                    foreach (var widgetName in widgetColumn.Widgets)
                    {
                        var widget = this.LoadWidget(widgetName, !string.IsNullOrEmpty(widgetName));
                        panel.Children.Add(widget);
                    }
                }

                OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    MinWidth = 100.0d,
                    Width = new GridLength(widgetColumn.ColumnWidth, GridUnitType.Pixel),
                });

                var splitter = new GridSplitter
                {
                    Width = 3.0d,
                };

                Grid.SetColumn(panel, columnNumber);
                Grid.SetColumn(splitter, columnNumber);

                OverviewGrid.Children.Add(panel);
                OverviewGrid.Children.Add(splitter);

                columnNumber++;
            }

            OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0d, GridUnitType.Auto),
            });
        }

        private Control LoadWidget(string widgetName, bool expanded)
        {
            var widgetKey = "overview-widgets-" + widgetName;

            var factoryName = this.ReadOnlyBook.GetSetting(widgetKey + "-name");
            var factoryType = this.ReadOnlyBook.GetSetting(widgetKey + "-type");
            var widgetSettings = this.ReadOnlyBook.GetSetting(widgetKey + "-settings");

            FrameworkElement header;
            FrameworkElement content;

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

                content = widget.Create(this.ReadOnlyBook, events);
                header = new Label
                {
                    Content = factoryName,
                };
            }
            else
            {
                BitmapSource bitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                    System.Drawing.SystemIcons.Warning.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(16, 16));

                var image = new Image
                {
                    Source = bitmap,
                };

                var label = new Label
                {
                    Content = string.IsNullOrEmpty(factoryName) ? widgetName : factoryName,
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1.0d, GridUnitType.Star),
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1.0d, GridUnitType.Auto),
                });

                Grid.SetColumn(label, 0);
                Grid.SetColumn(image, 1);

                grid.Children.Add(label);
                grid.Children.Add(image);

                header = grid;

                content = new Label
                {
                    Content = "Failed to load " + widgetName,
                };

                expanded = false;
            }


            var expander = new Expander
            {
                IsExpanded = expanded,
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                Padding = new Thickness(2.0d),
                Margin = new Thickness(5.0d),
                Header = header,
                Content = content,
            };

            return expander;
        }

        private void UpdateAccounts()
        {
            var items = this.CreateAccountItems(this.Book, null);
            foreach (var item in items)
            {
                this.AccountsList.Items.Add(item);
            }
        }

        private IEnumerable<TreeViewItem> CreateAccountItems(Book book, Account parent)
        {
            var subAccounts = from a in book.Accounts
                              where a.ParentAccount == parent
                              orderby a.Name
                              select a;

            foreach (var a in subAccounts)
            {
                var item = new TreeViewItem();

                var subItems = this.CreateAccountItems(book, a);
                foreach (var subItem in subItems)
                {
                    item.Items.Add(subItem);
                }

                var panel = new StackPanel
                {
                    Tag = a.AccountId,
                    Orientation = Orientation.Horizontal,
                };
                panel.MouseLeftButtonDown += this.Account_MouseLeftButtonDown;

                var image = new Image
                {
                    Height = 16,
                    Source = new BitmapImage(new Uri("pack://application:,,,/SharpBooks;component/resources/Coinstack.png")),
                };

                var label = new Label
                {
                    Content = a.Name,
                };

                panel.Children.Add(image);
                panel.Children.Add(label);

                item.Header = panel;

                yield return item;
            }
        }

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((StackPanel)sender).Tag,
                };

                this.AccountSelected(sender, args);

                e.Handled = true;
            }
        }

        private void AccountSelected(object sender, AccountSelectedEventArgs e)
        {
            AccountsTabItem.IsSelected = true;
            AccountsList.Visibility = Visibility.Hidden;
            SplitList.Visibility = Visibility.Visible;
            
            var account = this.Book.Accounts.Where(a => a.AccountId == e.AccountId).Single();
            SplitList.DataContext = from s in this.Book.GetAccountSplits(account)
                                    select new SplitViewModel(s);
        }

        private static IEnumerable<IPluginFactory> LoadAllPlugins()
        {
            var appPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetEntryAssembly().Location);

            return PluginLoader.LoadAllPlugins(appPath);
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

            book.SetSetting("overview-layout", "[{\"ColumnWidth\":200,\"Widgets\":[\"widget1\",\"widget2\"]},{\"ColumnWidth\":250,\"Widgets\":[\"widget3\"]}]");
            book.SetSetting("overview-widgets-widget1-name", "Favorite Accounts");
            book.SetSetting("overview-widgets-widget1-type", "SharpBooks.StandardPlugins.FavoriteAccountsWidgetFactory, SharpBooks.StandardPlugins, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6fee4057cb920410");
            book.SetSetting("overview-widgets-widget1-settings", "{\"PathSeperator\":\"\\\\\",\"AccountPaths\":[\"Assets\\\\My Bank Account\",\"Assets\\\\My Other Bank\",\"Liabilities\\\\My Home Loan\"]}");
            book.SetSetting("overview-widgets-widget3-name", "Recent Expenses");
            book.SetSetting("overview-widgets-widget3-type", "SharpBooks.StandardPlugins.RecentExpensesWidgetFactory, SharpBooks.StandardPlugins, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6fee4057cb920410");

            return book;
        }
    }
}

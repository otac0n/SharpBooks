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

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private Book book;
        private IEnumerable<IPluginFactory> plugins;

        public MainView()
        {
            InitializeComponent();

            this.plugins = LoadAllPlugins();
            this.book = BuildFakeBook();

            this.UpdateAccounts();

            var widget = this.LoadWidget("widget1");
            StackPanel1.Children.Add(widget);
        }

        private Control LoadWidget(string widgetName)
        {
            var widgetKey = "overview-widgets-" + widgetName;

            var rob = this.book.AsReadOnly();

            var factoryName = rob.GetSetting(widgetKey + "-name");
            var factoryType = rob.GetSetting(widgetKey + "-type");
            var widgetSettings = rob.GetSetting(widgetKey + "-settings");

            var factory = (from p in plugins
                           let w = p as IWidgetFactory
                           where w != null
                           where w.GetType().AssemblyQualifiedName == factoryType
                           where w.Name == factoryName
                           select w).SingleOrDefault();

            var events = new EventProxy(
                this.AccountSelected);

            var widget = factory.CreateInstance(rob, widgetSettings);
            var expander = new Expander
            {
                IsExpanded = true,
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Padding = new Thickness(2.0d),
                Margin = new Thickness(5.0d),
                Header = factory.Name,
                Content = widget.Create(rob, events)
            };

            return expander;
        }

        private void UpdateAccounts()
        {
            var items = this.CreateAccountItems(this.book, null);
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
                panel.MouseLeftButtonDown += Account_MouseLeftButtonDown;

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
            
            var account = this.book.Accounts.Where(a => a.AccountId == e.AccountId).Single();
            SplitList.DataContext = this.book.GetAccountSplits(account);
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
                var split1 = transaction1.AddSplit(tlock);
                split1.SetAccount(account2, tlock);
                split1.SetAmount(3520, tlock);
                split1.SetTransactionAmount(3520, tlock);

                var split2 = transaction1.AddSplit(tlock);
                split2.SetAccount(account3, tlock);
                split2.SetAmount(-3520, tlock);
                split2.SetTransactionAmount(-3520, tlock);
            }

            var book = new Book();
            book.AddSecurity(usd);
            book.AddAccount(account1);
            book.AddAccount(account2);
            book.AddAccount(account3);
            book.AddAccount(account4);
            book.AddAccount(account5);

            book.AddTransaction(transaction1);

            book.SetSetting("overview-widgets-widget1-name", "Favorite Accounts");
            book.SetSetting("overview-widgets-widget1-type", "SharpBooks.StandardPlugins.FavoriteAccountsWidgetFactory, SharpBooks.StandardPlugins, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6fee4057cb920410");
            book.SetSetting("overview-widgets-widget1-settings", "{\"PathSeperator\":\"\\\\\",\"AccountPaths\":[\"Assets\\\\My Bank Account\",\"Assets\\\\My Other Bank\",\"Liabilities\\\\My Home Loan\"]}");

            return book;
        }
    }
}

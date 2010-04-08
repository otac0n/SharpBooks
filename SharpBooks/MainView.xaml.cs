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
        public MainView()
        {
            InitializeComponent();

            var b = BuildFakeBook();
            var rob = b.AsReadOnly();

            var events = new EventProxy(
                (sender, args) => MessageBox.Show(b.Accounts.Where(a => a.AccountId == args.AccountId).Single().Name + " Selected!"));

            var plugins = LoadAllPlugins();

            var factory = (from p in plugins
                           let w = p as IWidgetFactory
                           where w != null
                           where w.Name == "Favorite Accounts"
                           select w).SingleOrDefault();

            var settings = "{\"PathSeperator\":\"\\\\\",\"AccountPaths\":[\"Assets\\\\My Bank Account\",\"Assets\\\\My Other Bank\",\"Liabilities\\\\My Home Loan\"]}";

            var widget = factory.CreateInstance(rob, settings);
            var expander = new Expander
            {
                IsExpanded = true,
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                Padding = new Thickness(2.0d),
                Margin = new Thickness(5.0d),
                Header = factory.Name,
                Content = widget.Create(rob, events)
            };

            StackPanel1.Children.Add(expander);
        }

        private static IEnumerable<IPluginFactory> LoadAllPlugins()
        {
            var appPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetEntryAssembly().Location);

            return PluginLoader.LoadAllPlugins(appPath);
        }

        private static Book BuildFakeBook()
        {
            var usd = new Security(Guid.NewGuid(), SecurityType.Currency, "United States dollar", "USD", "{0:$#,##0.00#;($#,##0.00#);-$0-}", 1000);
            var account1 = new Account(Guid.NewGuid(), usd, null, "Assets", 100);
            var account2 = new Account(Guid.NewGuid(), usd, account1, "My Bank Account", 100);
            var account3 = new Account(Guid.NewGuid(), usd, account1, "My Other Bank", 100);
            var account4 = new Account(Guid.NewGuid(), usd, null, "Liabilities", 100);
            var account5 = new Account(Guid.NewGuid(), usd, account4, "My Home Loan", 100);

            var book = new Book();
            book.AddSecurity(usd);
            book.AddAccount(account1);
            book.AddAccount(account2);
            book.AddAccount(account3);
            book.AddAccount(account4);
            book.AddAccount(account5);
            return book;
        }
    }
}

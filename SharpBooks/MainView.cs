namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using SharpBooks.Plugins;

    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();

            var b = BuildFakeBook();
            var rob = b.AsReadOnly();

            var events = new EventProxy(
                (sender, args) => MessageBox.Show(b.Accounts.Where(a => a.AccountId == args.AccountId).Single().Name + " Selected!"));

            var factory = new FavoriteAccountsWidgetFactory();
            string settings = factory.Configure(rob, null);

            var widget = factory.CreateInstance(rob, settings);
            //var expander = new Expander
            //{
            //    IsExpanded = true,
            //    BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
            //    Padding = new Thickness(2.0d),
            //    Margin = new Thickness(5.0d),
            //    Header = factory.Name,
            //    Content = widget.Create(rob, events)
            //};

            //StackPanel1.Children.Add(expander);
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

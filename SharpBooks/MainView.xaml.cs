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
    using SharpBooks.Controllers;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainController controller = new MainController();

        public MainController Controller
        {
            get
            {
                return this.controller;
            }
        }

        public MainView()
        {
            InitializeComponent();
        }

        //private void RefreshOverview()
        //{
        //    foreach (var widget in this.widgets)
        //    {
        //        widget.Dispose();
        //    }

        //    this.widgets.Clear();

        //    var newWidgets = App.Controller.GetOverview();

        //    if (newWidgets != null)
        //    {

        //    }
        //}

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)(((StackPanel)sender).Tag),
                };

                //this.AccountSelected(sender, args);

                var account1 = this.Controller.Book.Accounts.Where(a => a.AccountId == args.AccountId).Single();
                var account2 = this.Controller.Book.Accounts.First();

                if (account1 != account2)
                {
                    var transaction = new Transaction(Guid.NewGuid(), account1.Security);
                    using (var lck = transaction.Lock())
                    {
                        var split1 = transaction.AddSplit(lck);
                        split1.SetAccount(account1, lck);
                        split1.SetAmount(100, lck);
                        split1.SetTransactionAmount(100, lck);

                        var split2 = transaction.AddSplit(lck);
                        split2.SetAccount(account2, lck);
                        split2.SetAmount(-100, lck);
                        split2.SetTransactionAmount(-100, lck);
                    }

                    this.Controller.AddTransaction(transaction);
                }

                e.Handled = true;
            }
        }
    }
}

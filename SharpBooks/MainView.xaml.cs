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
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window, INotifyPropertyChanged
    {
        private MainController controller = new MainController();
        private Account activeAccount;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainView()
        {
            InitializeComponent();

            this.AccountSelected += MainView_AccountSelected;
        }

        void MainView_AccountSelected(object sender, AccountSelectedEventArgs args)
        {
            this.ActiveAccount = this.Controller.Book.Accounts.Where(a => a.AccountId == args.AccountId).SingleOrDefault();
        }

        private delegate void AccountSelectedDelegate(object sender, AccountSelectedEventArgs args);

        private event AccountSelectedDelegate AccountSelected;

        public MainController Controller
        {
            get
            {
                return this.controller;
            }
        }

        public Account ActiveAccount
        {
            get
            {
                return this.activeAccount;
            }

            private set
            {
                this.activeAccount = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ActiveAccount"));
                }
            }
        }

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)(((StackPanel)sender).Tag),
                };

                RaiseAccountSelected(args);

                e.Handled = true;
            }
        }

        private void RaiseAccountSelected(AccountSelectedEventArgs args)
        {
            if (this.AccountSelected != null)
            {
                this.AccountSelected(this, args);
            }
        }
    }
}

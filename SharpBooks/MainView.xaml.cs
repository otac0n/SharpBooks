//-----------------------------------------------------------------------
// <copyright file="MainView.xaml.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using SharpBooks.Controllers;
    using SharpBooks.Plugins;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window, INotifyPropertyChanged
    {
        private MainController controller = new MainController();
        private Account activeAccount;

        public MainView()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                    AccountId = (Guid)((StackPanel)sender).Tag,
                };

                this.AccountSelected(args);

                e.Handled = true;
            }
        }

        private void AccountSelected(AccountSelectedEventArgs args)
        {
            this.ActiveAccount = this.Controller.Book.Accounts.Where(a => a.AccountId == args.AccountId).SingleOrDefault();
        }

        private void ReturnToAccounts_Click(object sender, RoutedEventArgs e)
        {
            this.ActiveAccount = null;
        }
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="MainView.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Forms;
    using SharpBooks.Controllers;
    using SharpBooks.Plugins;

    public partial class MainView : Form, INotifyPropertyChanged
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

        private void Account_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((Control)sender).Tag,
                };

                this.AccountSelected(args);
            }
        }

        private void AccountSelected(AccountSelectedEventArgs args)
        {
            this.ActiveAccount = this.Controller.Book.Accounts.Where(a => a.AccountId == args.AccountId).SingleOrDefault();
        }

        private void ReturnToAccounts_Click(object sender, EventArgs e)
        {
            this.ActiveAccount = null;
        }
    }
}

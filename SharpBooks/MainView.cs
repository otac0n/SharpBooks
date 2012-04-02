﻿//-----------------------------------------------------------------------
// <copyright file="MainView.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
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

    public partial class MainView : Form
    {
        private readonly MainController owner;

        public MainView(MainController owner)
        {
            this.owner = owner;
            this.owner.BookChanged += Owner_BookChanged;
            this.owner.ActiveAccountChanged += Owner_ActiveAccountChanged;

            this.InitializeComponent();

            this.Owner_BookChanged(this.owner, new EventArgs());
            this.Owner_ActiveAccountChanged(this.owner, new EventArgs());
        }

        public event EventHandler<AccountSelectedEventArgs> AccountSelected;

        public event EventHandler<EventArgs> AccountDeselected;

        private void New_Click(object sender, EventArgs e)
        {
            this.owner.New();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            this.owner.Open();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.owner.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.owner.Save(forceSaveAs: false);
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            this.owner.Save(forceSaveAs: true);
        }

        private void Account_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AccountSelected.SafeInvoke(this, () => new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((Control)sender).Tag,
                });
            }
        }

        private void ReturnToAccounts_Click(object sender, EventArgs e)
        {
            this.AccountDeselected.SafeInvoke(this, () => new EventArgs());
        }

        private void Owner_BookChanged(object sender, EventArgs e)
        {
            var book = this.owner.Book;
            this.accountTree.Book = book;

            bool isNull = book == null;
            this.tabView.Visible = !isNull;
        }

        private void Owner_ActiveAccountChanged(object sender, EventArgs e)
        {
            var activeAccount = this.owner.ActiveAccount;
            this.accountRegister.SetAccount(activeAccount, this.owner.Book);

            bool isNull = activeAccount == null;
            this.accountViewContainer.Visible = !isNull;
            this.accountTree.Visible = isNull;
        }

        private void AccountTree_AccountSelected(object sender, AccountSelectedEventArgs e)
        {
            this.AccountSelected.SafeInvoke(this, () => e);
        }

        private void AccountTree_NewAccountRequested(object sender, NewAccountRequestedEventArgs e)
        {
            this.owner.NewAccount(e.ParentAccountId);
        }
    }
}

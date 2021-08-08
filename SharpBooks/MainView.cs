// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Windows.Forms;
    using SharpBooks.Controllers;
    using SharpBooks.Plugins;

    public partial class MainView : Form
    {
        private readonly MainController owner;

        public MainView(MainController owner)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.owner.BookChanged += this.Owner_BookChanged;
            this.owner.ActiveAccountChanged += this.Owner_ActiveAccountChanged;

            this.InitializeComponent();

            this.Owner_BookChanged(this.owner, EventArgs.Empty);
            this.Owner_ActiveAccountChanged(this.owner, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> AccountDeselected;

        public event EventHandler<AccountSelectedEventArgs> AccountSelected;

        private void Account_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AccountSelected?.Invoke(this, new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((Control)sender).Tag,
                });
            }
        }

        private void AccountRegister_TransactionCreated(object sender, UI.TransactionCreatedEventArgs e)
        {
            this.owner.AddTransaction(e.NewTransaction);
        }

        private void AccountRegister_TransactionUpdated(object sender, UI.TransactionUpdatedEventArgs e)
        {
            this.owner.UpdateTransaction(e.OldTransaction, e.NewTransaction);
        }

        private void AccountTree_AccountDeleteRequested(object sender, AccountDeleteRequestedEventArgs e)
        {
            this.owner.DeleteAccount(e.AccountId);
        }

        private void AccountTree_AccountSelected(object sender, AccountSelectedEventArgs e)
        {
            this.AccountSelected?.Invoke(this, e);
        }

        private void AccountTree_NewAccountRequested(object sender, NewAccountRequestedEventArgs e)
        {
            this.owner.NewAccount(e.ParentAccountId);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.owner.Close();
        }

        private void New_Click(object sender, EventArgs e)
        {
            this.owner.New();
        }

        private void NewTransactionButton_Click(object sender, EventArgs e)
        {
            this.accountRegister.NewTransaction();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            this.owner.Open();
        }

        private void Owner_ActiveAccountChanged(object sender, EventArgs e)
        {
            var activeAccount = this.owner.ActiveAccount;
            this.accountRegister.SetAccount(activeAccount, this.owner.Book);

            var isNull = activeAccount == null;
            this.accountViewContainer.Visible = !isNull;
            this.accountTree.Visible = isNull;
        }

        private void Owner_BookChanged(object sender, EventArgs e)
        {
            var book = this.owner.Book;
            this.accountTree.Book = book;

            var isNull = book == null;
            this.tabView.Visible = !isNull;
        }

        private void ReturnToAccounts_Click(object sender, EventArgs e)
        {
            this.AccountDeselected?.Invoke(this, EventArgs.Empty);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.owner.Save(forceSaveAs: false);
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            this.owner.Save(forceSaveAs: true);
        }
    }
}

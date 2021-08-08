// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public partial class AccountRegister : UserControl
    {
        private readonly HeaderControl.ColumnHeader descriptionColumn;
        private Account account;
        private ReadOnlyBook book;
        private bool transactionIsNew;

        public AccountRegister()
        {
            this.InitializeComponent();

            this.descriptionColumn = new HeaderControl.ColumnHeader
            {
                Text = "Description",
                Width = 200,
                CanResize = false,
            };

            this.headers.Columns.Add("Date", 100);
            this.headers.Columns.Add("Number", 50);
            this.headers.Columns.Add(this.descriptionColumn);
            this.headers.Columns.Add("Account", 100);
            this.headers.Columns.Add("Deposit", 60);
            this.headers.Columns.Add("Withdrawal", 60);
            this.headers.Columns.Add("Balance", 110);
        }

        public event EventHandler<TransactionCreatedEventArgs> TransactionCreated;

        public event EventHandler<TransactionUpdatedEventArgs> TransactionUpdated;

        public void NewTransaction()
        {
            var transaction = new Transaction(Guid.NewGuid(), this.book.Securities.First());
            transaction.Date = DateTime.Today.ToUniversalTime();
            var split1 = transaction.AddSplit();
            var split2 = transaction.AddSplit();

            split1.Account = this.account;
            split1.Security = transaction.BaseSecurity;
            split2.Security = transaction.BaseSecurity;

            this.transactionEditor.SetSplit(transaction.Splits[0]);
            this.transactionIsNew = true;
        }

        public void SetAccount(Account account, ReadOnlyBook book)
        {
            this.book = book;
            this.account = account;
            this.splitsView.SetAccount(account, book);
            this.transactionEditor.SetBook(book);
        }

        internal Rectangle[] GetColumnBounds()
        {
            return this.headers.GetColumnBounds();
        }

        private void Headers_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            this.Refresh();
        }

        private void Splits_DesiresOffset(object sender, DesiresOffsetEventArgs e)
        {
            this.vScroll.Value = -e.DesiredOffset.Y;
        }

        private void Splits_MouseWheel(object sender, MouseEventArgs e)
        {
            this.vScroll.Value = (this.vScroll.Value - e.Delta).Clamp(0, this.vScroll.Maximum);
        }

        private void Splits_ScrollableSizeChanged(object sender, EventArgs e)
        {
            var s = this.splitsView.ScrollSize;
            this.vScroll.Visible = s.Height != 0;
            this.vScroll.Maximum = s.Height;
        }

        private void Splits_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.transactionEditor.SetSplit(this.splitsView.SelectedSplit);
            this.transactionIsNew = false;
        }

        private void TransactionEditor_TransactionUpdated(object sender, TransactionUpdatedEventArgs e)
        {
            if (this.transactionIsNew)
            {
                this.TransactionCreated?.Invoke(this, new TransactionCreatedEventArgs(e.NewTransaction));
            }
            else
            {
                this.TransactionUpdated?.Invoke(this, e);
            }
        }

        private void VScroll_ValueChanged(object sender, EventArgs e)
        {
            this.splitsView.Offset = new Point(0, -this.vScroll.Value);
        }
    }
}

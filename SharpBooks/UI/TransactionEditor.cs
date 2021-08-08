//-----------------------------------------------------------------------
// <copyright file="TransactionEditor.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.UI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Forms;

    public partial class TransactionEditor : UserControl
    {
        private ReadOnlyBook book;
        private Transaction originalTransaction;
        private Split otherSplit;
        private Split split;
        private bool suppressUpdates;
        private Transaction transaction;

        public TransactionEditor()
        {
            InitializeComponent();
        }

        public event EventHandler<TransactionUpdatedEventArgs> TransactionUpdated;

        public void SetBook(ReadOnlyBook book)
        {
            this.SetSplit(null);
            this.book = book;
            this.accountComboBox.Items.Clear();
            if (this.book != null)
            {
                this.accountComboBox.Items.AddRange(this.book.Accounts.ToArray());
            }
        }

        internal void SetSplit(Split split)
        {
            this.split = null;
            this.otherSplit = null;
            this.transaction = null;
            this.originalTransaction = null;

            if (split != null)
            {
                Debug.Assert(
                    split.Transaction.Splits.Count == 2 &&
                    split.Transaction.Splits.All(s => s.Security == split.Transaction.BaseSecurity),
                    "The transaction is too complex for the simple transaction editor.");

                this.originalTransaction = split.Transaction;
                this.transaction = split.Transaction.Copy();
                this.split = this.transaction.Splits[this.originalTransaction.Splits.IndexOf(split)];
                this.otherSplit = this.transaction.Splits.Where(s => s != this.split).Single();
            }

            ResetControls();
        }

        private void accountComboBox_Validated(object sender, EventArgs e)
        {
            this.otherSplit.Account = (Account)this.accountComboBox.SelectedItem;
            ResetControls();
        }

        private void accountComboBox_Validating(object sender, CancelEventArgs e)
        {
            this.errorProvider.SetError(this.accountComboBox, null);

            if (this.accountComboBox.SelectedItem == null &&
                !string.IsNullOrEmpty(this.accountComboBox.Text))
            {
                var match = this.accountComboBox.Items.Cast<Account>().Where(a => a.ToString() == this.accountComboBox.Text).FirstOrDefault();
                if (match != null)
                {
                    this.accountComboBox.SelectedItem = match;
                }
                else
                {
                    this.errorProvider.SetError(this.accountComboBox, "You must choose an account from the list.");
                    e.Cancel = true;
                }
            }
        }

        private void amountBox_Validated(object sender, EventArgs e)
        {
            var total = this.split.Security.ParseValue(this.depositTextBox.Text) - this.split.Security.ParseValue(this.withdrawalTextBox.Text);

            this.split.TransactionAmount = this.split.Amount = total;
            this.otherSplit.TransactionAmount = this.otherSplit.Amount = -total;

            ResetControls();
        }

        private void amountBox_Validating(object sender, CancelEventArgs e)
        {
            var box = (TextBox)sender;

            long value;
            if (!this.split.Security.TryParseValue(box.Text, out value))
            {
                this.errorProvider.SetError(box, "You must enter a valid amount in " + this.split.Security.Name + ".");
                e.Cancel = true;
            }
        }

        private void clearDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!this.suppressUpdates)
            {
                this.split.DateCleared = this.clearDatePicker.Checked ? this.clearDatePicker.Value.ToUniversalTime() : (DateTime?)null;
            }
        }

        private void ResetControls()
        {
            this.suppressUpdates = true;

            this.depositTextBox.Text = "";
            this.withdrawalTextBox.Text = "";
            this.descriptionTextBox.Text = "";
            this.Enabled = (split != null);

            if (this.split == null)
            {
                this.transactionDatePicker.Value = DateTime.Today;
                this.clearDatePicker.Value = DateTime.Today;
                this.clearDatePicker.Checked = false;

                this.accountComboBox.SelectedItem = null;
            }
            else
            {
                this.transactionDatePicker.Value = this.split.Transaction.Date.ToLocalTime();
                this.clearDatePicker.Value = (this.split.DateCleared ?? this.split.Transaction.Date).ToLocalTime();
                this.clearDatePicker.Checked = this.split.DateCleared.HasValue;

                if (this.split.Amount > 0)
                {
                    this.depositTextBox.Text = this.split.Security.FormatValue(split.Amount);
                }
                else if (this.split.Amount < 0)
                {
                    this.withdrawalTextBox.Text = this.split.Security.FormatValue(-split.Amount);
                }

                this.accountComboBox.SelectedItem = this.otherSplit.Account;
            }

            this.suppressUpdates = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var errors = this.transaction.RuleViolations.ToList();
            if (errors.Any())
            {
                MessageBox.Show("There were errors validating the transaction:" + Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, errors.Select(err => err.Message)));
            }
            else
            {
                var oldTransaction = this.originalTransaction;
                var newTransaction = this.transaction;
                var split = this.split;

                this.SetSplit(null);
                this.TransactionUpdated.SafeInvoke(this, new TransactionUpdatedEventArgs(oldTransaction, newTransaction));
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void transactionDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!this.suppressUpdates)
            {
                this.transaction.Date = this.transactionDatePicker.Value.ToUniversalTime();
            }
        }
    }
}

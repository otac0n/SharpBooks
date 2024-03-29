// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using SharpBooks.Controllers;

    public partial class EditAccountView : Form
    {
        private readonly Account originalAccount;
        private readonly MainController owner;

        public EditAccountView(MainController owner, Account account)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.originalAccount = account ?? throw new ArgumentNullException(nameof(account));

            this.InitializeComponent();

            this.securityComboBox.Items.Add(new SecurityOption(null));
            this.securityComboBox.Items.AddRange(this.owner.Book.Securities.Select(s => new SecurityOption(s)).ToArray());

            this.nameTextBox.Text = this.originalAccount.Name;
            this.balanceAccountRadio.Checked = this.originalAccount.AccountType == AccountType.Balance;
            this.groupingAccountRadio.Checked = this.originalAccount.AccountType == AccountType.Grouping;
            this.securityComboBox.SelectedItem = this.securityComboBox.Items.Cast<SecurityOption>().Single(so => so.Security == this.originalAccount.Security);
            this.fractionTextBox.Text = this.originalAccount.Security == null ? string.Empty : this.originalAccount.Security.FormatValue(this.originalAccount.Security.FractionTraded / this.originalAccount.SmallestFraction.Value);
        }

        public Account NewAccount { get; private set; }

        private void FractionTextBox_Validating(object sender, CancelEventArgs e)
        {
            var box = (TextBox)sender;
            this.errorProvider.SetError(box, null);

            var security = ((SecurityOption)this.securityComboBox.SelectedItem).Security;
            if (!security.TryParseValue(box.Text, out var fraction))
            {
                this.errorProvider.SetError(box, "You must enter a valid value.");
                e.Cancel = true;
            }
            else if (fraction <= 0)
            {
                this.errorProvider.SetError(box, "You must enter a value greater than zero.");
                e.Cancel = true;
            }
            else
            {
                box.Text = security.FormatValue(fraction);
            }
        }

        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {
            var box = (TextBox)sender;
            this.errorProvider.SetError(box, null);

            if (string.IsNullOrEmpty(box.Text))
            {
                this.errorProvider.SetError(box, "You must specify an account name.");
                e.Cancel = true;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            var security = ((SecurityOption)this.securityComboBox.SelectedItem).Security;

            this.NewAccount = new Account(
                this.originalAccount.AccountId,
                this.balanceAccountRadio.Checked ? AccountType.Balance : AccountType.Grouping,
                security,
                this.originalAccount.ParentAccount,
                this.nameTextBox.Text,
                security != null ? (int)(security.FractionTraded / security.ParseValue(this.fractionTextBox.Text)) : (int?)null);
        }

        private void SecurityComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var item = (SecurityOption)this.securityComboBox.SelectedItem;

            if (item.Security == null)
            {
                this.fractionTextBox.Enabled = false;
                this.fractionTextBox.Text = string.Empty;
            }
            else
            {
                this.fractionTextBox.Enabled = true;
                this.fractionTextBox.Text = item.Security.FormatValue(1);
            }
        }

        private class SecurityOption
        {
            public SecurityOption(Security security)
            {
                this.Security = security;
            }

            public Security Security { get; }

            public override string ToString()
            {
                return this.Security == null
                    ? "(any currency)"
                    : this.Security.Name + " (" + this.Security.Symbol + ")";
            }
        }
    }
}

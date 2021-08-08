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
        private Account newAccount;

        public EditAccountView(MainController owner, Account account)
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            this.owner = owner;
            this.originalAccount = account;

            this.InitializeComponent();

            this.securityComboBox.Items.Add(new SecurityOption(null));
            this.securityComboBox.Items.AddRange(this.owner.Book.Securities.Select(s => new SecurityOption(s)).ToArray());

            this.nameTextBox.Text = this.originalAccount.Name;
            this.balanceAccountRadio.Checked = this.originalAccount.AccountType == AccountType.Balance;
            this.groupingAccountRadio.Checked = this.originalAccount.AccountType == AccountType.Grouping;
            this.securityComboBox.SelectedItem = this.securityComboBox.Items.Cast<SecurityOption>().Where(so => so.Security == this.originalAccount.Security).Single();
            this.fractionTextBox.Text = this.originalAccount.Security == null ? string.Empty : this.originalAccount.Security.FormatValue(this.originalAccount.Security.FractionTraded / this.originalAccount.SmallestFraction.Value);
        }

        public Account NewAccount
        {
            get
            {
                return this.newAccount;
            }
        }

        private void fractionTextBox_Validating(object sender, CancelEventArgs e)
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

        private void nameTextBox_Validating(object sender, CancelEventArgs e)
        {
            var box = (TextBox)sender;
            this.errorProvider.SetError(box, null);

            if (string.IsNullOrEmpty(box.Text))
            {
                this.errorProvider.SetError(box, "You must specify an account name.");
                e.Cancel = true;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var security = ((SecurityOption)this.securityComboBox.SelectedItem).Security;

            this.newAccount = new Account(
                this.originalAccount.AccountId,
                this.balanceAccountRadio.Checked ? AccountType.Balance : AccountType.Grouping,
                security,
                this.originalAccount.ParentAccount,
                this.nameTextBox.Text,
                security != null ? (int)(security.FractionTraded / security.ParseValue(this.fractionTextBox.Text)) : (int?)null);
        }

        private void securityComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var item = (SecurityOption)this.securityComboBox.SelectedItem;

            if (item.Security == null)
            {
                this.fractionTextBox.Enabled = false;
                this.fractionTextBox.Text = "";
            }
            else
            {
                this.fractionTextBox.Enabled = true;
                this.fractionTextBox.Text = item.Security.FormatValue(1);
            }
        }

        private class SecurityOption
        {
            private Security security;

            public SecurityOption(Security security)
            {
                this.security = security;
            }

            public Security Security
            {
                get { return this.security; }
            }

            public override string ToString()
            {
                return this.security == null
                    ? "(any currency)"
                    : this.security.Name + " (" + this.security.Symbol + ")";
            }
        }
    }
}

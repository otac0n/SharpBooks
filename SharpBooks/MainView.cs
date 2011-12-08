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

    public partial class MainView : Form
    {
        private readonly MainController owner;

        public MainView(MainController owner)
        {
            this.owner = owner;
            this.owner.ActiveAccountChanged += Owner_ActiveAccountChanged;

            InitializeComponent();
        }

        public event EventHandler<AccountSelectedEventArgs> AccountSelected;
        public event EventHandler<EventArgs> AccountDeselected;

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

        private void Owner_ActiveAccountChanged(object sender, EventArgs e)
        {
            var activeAccount = this.owner.ActiveAccount;
            if (activeAccount == null)
            {
                // TODO: Hide and clear-out the active account window.
            }
            else
            {
                // TODO: Populate and show the active account window.
            }
        }
    }
}

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

            this.InitializeComponent();
            this.BuildPersistenceMenus();
        }

        private void BuildPersistenceMenus()
        {
            foreach (var ps in this.owner.GetPersistenceStrategies().OrderBy(ps => ps.Name))
            {
                var openItem = new ToolStripMenuItem();
                var saveAsItem = new ToolStripMenuItem();

                openItem.Text = saveAsItem.Text = ps.Name;
                openItem.Tag = saveAsItem.Tag = ps;

                openItem.Click += Open_Click;
                saveAsItem.Click += SaveAs_Click;

                this.openToolStripMenuItem.DropDownItems.Add(openItem);
                this.saveAsToolStripMenuItem.DropDownItems.Add(saveAsItem);
            }
        }

        public event EventHandler<AccountSelectedEventArgs> AccountSelected;
        public event EventHandler<EventArgs> AccountDeselected;

        private void New_Click(object sender, EventArgs e)
        {
            this.owner.New();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            var factory = (IPersistenceStrategyFactory)((ToolStripItem)sender).Tag;
            this.owner.Open(factory);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.owner.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.owner.Save();
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            var factory = (IPersistenceStrategyFactory)((ToolStripItem)sender).Tag;
            this.owner.SaveAs(factory);
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

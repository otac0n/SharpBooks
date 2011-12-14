﻿//-----------------------------------------------------------------------
// <copyright file="AccountRegister.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public partial class AccountRegister : UserControl
    {
        public AccountRegister()
        {
            InitializeComponent();

            this.headers.Columns.Add("Date", 100);
            this.headers.Columns.Add("Number", 50);
            this.headers.Columns.Add("Description", 200);
            this.headers.Columns.Add("Account", 100);
            this.headers.Columns.Add("Deposit", 60);
            this.headers.Columns.Add("Withdrawal", 60);
            this.headers.Columns.Add("Balance", 110);
        }

        public void SetAccount(Account account, ReadOnlyBook book)
        {
            this.splitsView.SetAccount(account, book);
        }

        internal Rectangle[] GetColumnBounds()
        {
            return this.headers.GetColumnBounds();
        }

        private void Headers_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            this.Refresh();
        }

        private void VScroll_ValueChanged(object sender, EventArgs e)
        {
            //if (!this.lockOffset)
            //{
            //    var c = this.splits.Count;
            //    var itemHeight = this.GetItemHeight();
            //    var listHeight = c * itemHeight;

            //    var v = this.vScroll.Value;
            //    this.offset = -(int)(((long)v * listHeight) / c);
            //    this.Invalidate();
            //}
        }
    }
}

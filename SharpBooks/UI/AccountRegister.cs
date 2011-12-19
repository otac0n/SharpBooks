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
        private readonly HeaderControl.ColumnHeader descriptionColumn;

        public AccountRegister()
        {
            InitializeComponent();

            this.descriptionColumn = new HeaderControl.ColumnHeader
            {
                Text = "Description",
                Width = 200,
                CanResize = false,
            };

            this.headers.Columns.Add("Date", 100);
            this.headers.Columns.Add("Number", 50);
            this.headers.Columns.Add(descriptionColumn);
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
            this.splitsView.Offset = new Point(0, -this.vScroll.Value);
        }

        private void Splits_ScrollableSizeChanged(object sender, EventArgs e)
        {
            var s = this.splitsView.ScrollSize;
            this.vScroll.Visible = s.Height != 0;
            this.vScroll.Maximum = s.Height;
        }

        private void Splits_MouseWheel(object sender, MouseEventArgs e)
        {
            this.vScroll.Value = (this.vScroll.Value - e.Delta).Clamp(0, this.vScroll.Maximum);
        }

        private void Splits_DesiresOffset(object sender, DesiresOffsetEventArgs e)
        {
            this.vScroll.Value = -e.DesiredOffset.Y;
        }
    }
}

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
        private ReadOnlyBook book;
        private Account account;

        private SortedList<Split> splits;

        private int offset = 0;
        private bool lockOffset = false;

        public AccountRegister()
        {
            this.AlternatingBackColor = SystemColors.ControlLight;
            this.BackColor = SystemColors.Window;

            InitializeComponent();

            this.headers.Columns.Add("Date", 100);
            this.headers.Columns.Add("Number", 50);
            this.headers.Columns.Add("Description", 200);
            this.headers.Columns.Add("Account", 100);
            this.headers.Columns.Add("Deposit", 60);
            this.headers.Columns.Add("Withdrawal", 60);
            this.headers.Columns.Add("Balance", 110);
        }

        [Browsable(false)]
        public Account Account
        {
            get { return this.Account; }
        }

        public Color AlternatingBackColor
        {
            get;

            set;
        }

        public void SetAccount(Account account, ReadOnlyBook book)
        {
            if (this.account != account)
            {
                this.splits = null;
                this.account = account;
                this.book = book;
                this.InitializeAccount();
            }
        }

        private void InitializeAccount()
        {
            if (this.account != null)
            {
                var itemHeight = this.GetItemHeight();

                this.splits = new SortedList<Split>(this.book.GetAccountSplits(this.account), new SplitComparer());
                this.vScroll.Value = 0;
                this.vScroll.Maximum = this.splits.Count;

                this.Invalidate();
            }
        }

        private int GetItemHeight()
        {
            const int bottomPixelLine = 1;
            return this.Padding.Top + this.Font.Height + this.Padding.Bottom + bottomPixelLine;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var s = this.splits;
            if (s != null && !e.ClipRectangle.IsEmpty)
            {
                const int bottomPixelLine = 1;
                const TextFormatFlags BaseFormat = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;
                var g = e.Graphics;
                var itemHeight = this.GetItemHeight();
                var offsetY = this.offset;
                var offsetX = 0;
                var c = s.Count;
                var listWidth = this.ClientSize.Width;
                var listHeight = c * itemHeight;
                var textPadding = this.Padding.Top;
                var headerHeight = this.headers.Height;

                var columnBounds = this.headers.GetColumnBounds();


                g.Clear(SystemColors.AppWorkspace);

                using (var background = new SolidBrush(this.BackColor))
                {
                    g.FillRectangle(background, offsetX, offsetY, listWidth, headerHeight + listHeight);
                }

                using (var background = new SolidBrush(this.AlternatingBackColor))
                {
                    var firstVisible = (-offsetY - headerHeight) / itemHeight;
                    firstVisible = firstVisible < 0 ? 0 : firstVisible;

                    var count = (this.ClientSize.Height + itemHeight - 1) / itemHeight;
                    var lastVisible = firstVisible + count + 1;
                    lastVisible = lastVisible > c ? c : lastVisible;

                    for (int i = firstVisible; i < lastVisible; i++)
                    {
                        var rowTop = headerHeight + offsetY + i * itemHeight;
                        var textTop = rowTop + textPadding;
                        var rowBottomPixel = rowTop + itemHeight - bottomPixelLine;
                        var alternatingRow = i % 2 == 1;

                        var split = s[i];
                        var textParts = new[]
                            {
                                (split.DateCleared ?? split.Transaction.Date).ToShortDateString(),
                                "9999",
                                "TODO: This is a placeholder description.  The real description should be loaded from the transaction metadata.",
                                "TODO: Account goes here.",
                                split.Amount <= 0 ? split.Security.FormatValue(-split.Amount) : "",
                                split.Amount >= 0 ? split.Security.FormatValue(split.Amount) : "",
                                "TODO: Balance.",
                            };

                        if (alternatingRow)
                        {
                            g.FillRectangle(background, offsetX, rowTop, listWidth, itemHeight);
                        }

                        for (int j = 0; j < textParts.Length; j++)
                        {
                            var col = columnBounds[j];
                            TextRenderer.DrawText(g, textParts[j], this.Font, new Rectangle(col.X, rowTop, col.Width, itemHeight), this.ForeColor, BaseFormat | TextFormatFlags.Left);
                        }

                        g.DrawLine(SystemPens.WindowFrame, offsetX, rowBottomPixel, offsetX + listWidth, rowBottomPixel);
                    }
                }
            }
        }

        private class SplitComparer : IComparer<Split>
        {
            public int Compare(Split x, Split y)
            {
                int c;
                var date1 = x.DateCleared ?? x.Transaction.Date;
                var date2 = y.DateCleared ?? y.Transaction.Date;

                c = date1.CompareTo(date2);
                if (c != 0) return c;

                if (x.Security != y.Security)
                {
                    c = x.Security.Symbol.CompareTo(y.Security.Symbol);
                    if (c != 0) return c;
                }

                return x.Amount.CompareTo(y.Amount);
            }
        }

        private void Headers_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            this.Invalidate();
        }

        private void VScroll_ValueChanged(object sender, EventArgs e)
        {
            if (!this.lockOffset)
            {
                var c = this.splits.Count;
                var itemHeight = this.GetItemHeight();
                var listHeight = c * itemHeight;

                var v = this.vScroll.Value;
                this.offset = -(int)(((long)v * listHeight) / c);
                this.Invalidate();
            }
        }
    }
}

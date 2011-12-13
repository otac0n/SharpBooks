﻿//-----------------------------------------------------------------------
// <copyright file="AccountRegister.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class AccountRegister : UserControl
    {
        private ReadOnlyBook book;
        private Account account;

        private SortedList<Split> splits;

        public AccountRegister()
        {
            this.AlternatingBackColor = SystemColors.ControlLight;
            this.BackColor = SystemColors.Window;
            this.Padding = new Padding(3);

            InitializeComponent();
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
                const int topPixelLine = 1;
                const int bottomPixelLine = 1;
                var itemHeight = this.Padding.Top + this.Font.Height + this.Padding.Bottom + bottomPixelLine;

                this.splits = new SortedList<Split>(this.book.GetAccountSplits(this.account), new SplitComparer());
                this.AutoScrollMinSize = new Size(0, topPixelLine + this.splits.Count * itemHeight);
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var s = this.splits;
            if (s != null && !e.ClipRectangle.IsEmpty)
            {
                const int topPixelLine = 1;
                const int bottomPixelLine = 1;
                var g = e.Graphics;
                var itemHeight = (this.Padding.Top + this.Font.Height + this.Padding.Bottom + bottomPixelLine);
                var offset = this.AutoScrollPosition;
                var c = s.Count;
                var listWidth = this.ClientSize.Width;
                var listHeight = c * itemHeight;
                var textPadding = this.Padding.Top;


                g.Clear(SystemColors.AppWorkspace);

                using (var background = new SolidBrush(this.BackColor))
                {
                    g.FillRectangle(background, offset.X, offset.Y, listWidth, listHeight + topPixelLine);
                    g.DrawLine(SystemPens.WindowFrame, offset.X, offset.Y, offset.X + listWidth, offset.Y);
                }

                using (var background = new SolidBrush(this.AlternatingBackColor))
                {
                    using (var brush = new SolidBrush(this.ForeColor))
                    {
                        var firstVisible = (-offset.Y - topPixelLine) / itemHeight;
                        firstVisible = firstVisible < 0 ? 0 : firstVisible;

                        var count = (this.ClientSize.Height + itemHeight - 1) / itemHeight;
                        var lastVisible = firstVisible + count + 1;
                        lastVisible = lastVisible > c ? c : lastVisible;

                        for (int i = firstVisible; i < lastVisible; i++)
                        {
                            var rowTop = topPixelLine + offset.Y + i * itemHeight;
                            var textTop = rowTop + textPadding;
                            var rowBottomPixel = rowTop + itemHeight - bottomPixelLine;
                            var alternatingRow = i % 2 == 1;

                            var split = s[i];
                            var textParts = new[]
                            {
                                (split.DateCleared ?? split.Transaction.Date).ToShortDateString(),
                                split.Amount <= 0 ? split.Security.FormatValue(-split.Amount) : "",
                                split.Amount >= 0 ? split.Security.FormatValue(split.Amount) : "",
                            };

                            if (alternatingRow)
                            {
                                g.FillRectangle(background, offset.X, rowTop, listWidth, itemHeight);
                            }

                            for (int j = 0; j < textParts.Length; j++)
                            {
                                g.DrawString(textParts[j], this.Font, brush, offset.X + j * 100, textTop);
                            }

                            g.DrawLine(SystemPens.WindowFrame, offset.X, rowBottomPixel, offset.X + listWidth, rowBottomPixel);
                        }
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
    }
}

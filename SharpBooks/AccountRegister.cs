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
                this.splits = new SortedList<Split>(this.book.GetAccountSplits(this.account), new SplitComparer());
                this.AutoScrollMinSize = new Size(0, this.splits.Count * (int)(this.Font.SizeInPoints * 2.0F));
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var s = this.splits;
            if (s != null && !e.ClipRectangle.IsEmpty)
            {
                var g = e.Graphics;
                var itemHeight = (int)(this.Font.SizeInPoints * 2.0F);
                var offset = this.AutoScrollPosition;
                var c = s.Count;
                var listWidth = this.ClientSize.Width;
                var listHeight = c * itemHeight;


                g.Clear(SystemColors.AppWorkspace);

                using (var background = new SolidBrush(this.BackColor))
                {
                    g.FillRectangle(background, offset.X, offset.Y, listWidth, listHeight);
                }

                using (var background = new SolidBrush(this.AlternatingBackColor))
                {
                    using (var brush = new SolidBrush(this.ForeColor))
                    {
                        for (int i = 0; i < c; i++)
                        {
                            var split = s[i];
                            var text = split.Security.FormatValue(split.Amount);

                            if (i % 2 == 1)
                            {
                                g.FillRectangle(background, offset.X, offset.Y + i * itemHeight, listWidth, itemHeight);
                            }

                            g.DrawString(text, this.Font, brush, offset.X, offset.Y + i * itemHeight);
                            g.DrawLine(SystemPens.WindowFrame,
                                offset.X, offset.Y + i * itemHeight + itemHeight - 1,
                                offset.X + listWidth, offset.Y + i * itemHeight + itemHeight - 1);
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

                c = x.Security.Symbol.CompareTo(y.Security.Symbol);
                if (c != 0) return c;

                return x.Amount.CompareTo(y.Amount);
            }
        }
    }
}

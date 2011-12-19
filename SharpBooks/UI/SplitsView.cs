﻿//-----------------------------------------------------------------------
// <copyright file="SplitsView.cs" company="(none)">
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

    internal partial class SplitsView : UserControl
    {
        private SortedList<Split> splits;

        private ReadOnlyBook book;
        private Account account;

        private Point offset;

        public SplitsView()
        {
            this.splits = new SortedList<Split>(new SplitComparer());
            InitializeComponent();
        }

        public event EventHandler<EventArgs> ScrollableSizeChanged;

        public Color AlternatingBackColor
        {
            get;

            set;
        }

        public Point Offset
        {
            get { return this.offset; }

            set
            {
                this.offset = value;
                this.Invalidate();
            }
        }

        public Size ScrollSize
        {
            get
            {
                var itemHeight = this.GetItemHeight();
                var c = this.splits.Count;
                var listHeight = c * itemHeight;

                var scrollableSize = listHeight - this.ClientSize.Height;
                if (scrollableSize < 0)
                {
                    scrollableSize = 0;
                }

                return new Size(0, scrollableSize);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            this.OnScrollableSizeChanged();
        }

        private void OnScrollableSizeChanged()
        {
            this.OnScrollableSizeChanged(new EventArgs());
        }

        protected virtual void OnScrollableSizeChanged(EventArgs e)
        {
            this.ScrollableSizeChanged.SafeInvoke(this, e);
        }

        public void SetAccount(Account account, ReadOnlyBook book)
        {
            if (this.account != account)
            {
                this.splits.Clear();
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

                this.splits.AddRange(this.book.GetAccountSplits(this.account));
                this.OnScrollableSizeChanged();
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
                var offsetY = this.offset.Y;
                var offsetX = this.offset.X;
                var c = s.Count;
                var listWidth = this.ClientSize.Width;
                var listHeight = c * itemHeight;
                var textPadding = this.Padding.Top;

                var columnBounds = ((AccountRegister)this.Parent).GetColumnBounds();

                using (var background = new SolidBrush(this.AlternatingBackColor))
                {
                    var firstVisible = -offsetY / itemHeight;
                    firstVisible = firstVisible < 0 ? 0 : firstVisible;

                    var count = (this.ClientSize.Height + itemHeight - 1) / itemHeight;
                    var lastVisible = firstVisible + count + 1;
                    lastVisible = lastVisible > c ? c : lastVisible;

                    for (int i = firstVisible; i < lastVisible; i++)
                    {
                        var rowTop = offsetY + i * itemHeight;
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
    }
}

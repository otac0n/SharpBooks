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
        private readonly SortedList<Split> splits;

        private ReadOnlyBook book;
        private Account account;

        private Point offset;

        private int hoverIndex = -1;

        public SplitsView()
        {
            this.splits = new SortedList<Split>(new SplitComparer());
            InitializeComponent();
        }

        public event EventHandler<EventArgs> ScrollableSizeChanged;

        [Browsable(true)]
        public new event MouseEventHandler MouseWheel
        {
            add { base.MouseWheel += value; }

            remove { base.MouseWheel -= value; }
        }

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
            var register = this.Parent as AccountRegister;
            if (register == null)
            {
                base.OnPaint(e);
                return;
            }

            var s = this.splits;
            if (s != null && !e.ClipRectangle.IsEmpty)
            {
                var g = e.Graphics;
                var itemHeight = this.GetItemHeight();
                var offsetY = this.offset.Y;
                var offsetX = this.offset.X;
                var c = s.Count;
                var listWidth = this.ClientSize.Width;
                var listHeight = c * itemHeight;
                var textPadding = this.Padding.Top;

                var columnBounds = register.GetColumnBounds();

                using (var background = new SolidBrush(this.AlternatingBackColor))
                {
                    var firstVisible = this.GetVirtualRow(e.ClipRectangle.Top);
                    firstVisible = firstVisible < 0 ? 0 : firstVisible;

                    var lastVisible = this.GetVirtualRow(e.ClipRectangle.Bottom - 1);
                    lastVisible = lastVisible >= c ? c - 1 : lastVisible;

                    for (int i = firstVisible; i <= lastVisible; i++)
                    {
                        var alternatingRow = i % 2 == 1;

                        var rowRectangle = GetRowRectangle(i, itemHeight, offsetY, offsetX, listWidth, textPadding);

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

                        var textRectangles = new Rectangle[textParts.Length];
                        for (int j = 0; j < columnBounds.Length; j++)
                        {
                            var col = columnBounds[j];
                            textRectangles[j] = new Rectangle(offsetX + col.X, rowRectangle.Top, col.Width, itemHeight);
                        }

                        ListItemRenderer.RenderItems(
                            g,
                            rowRectangle,
                            alternatingRow ? background : null,
                            textRectangles,
                            textParts,
                            this.Font,
                            i == this.hoverIndex ? ListViewItemState.Hot : ListViewItemState.Normal);
                    }
                }
            }

            base.OnPaint(e);
        }

        private Rectangle GetRowRectangle(int i)
        {
            var itemHeight = this.GetItemHeight();
            var listWidth = this.ClientSize.Width;
            var textPadding = this.Padding.Top;
            return GetRowRectangle(i, itemHeight, this.offset.Y, this.offset.X, listWidth, textPadding);
        }

        private static Rectangle GetRowRectangle(int i, int itemHeight, int offsetY, int offsetX, int listWidth, int textPadding)
        {
            var rowTop = offsetY + i * itemHeight;
            var textTop = rowTop + textPadding;
            var rowRectangle = new Rectangle(offsetX, rowTop, listWidth, itemHeight);
            return rowRectangle;
        }

        private int GetVirtualRow(Point point)
        {
            return this.GetVirtualRow(point.Y);
        }

        private int GetVirtualRow(int y)
        {
            var itemHeight = this.GetItemHeight();
            var offsetY = this.offset.Y;
            return (y - offsetY) / itemHeight;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.UpdateHover(e);

            base.OnMouseMove(e);
        }

        private void UpdateHover(MouseEventArgs e)
        {
            var hoverIndex = this.GetVirtualRow(e.Location);
            if (hoverIndex < 0 ||
                hoverIndex >= this.splits.Count)
            {
                hoverIndex = -1;
            }

            if (this.hoverIndex != hoverIndex)
            {
                this.Invalidate(
                    this.GetRowRectangle(this.hoverIndex));
                this.Invalidate(
                    this.GetRowRectangle(hoverIndex));

                this.hoverIndex = hoverIndex;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.hoverIndex != -1)
            {
                this.Invalidate(
                    this.GetRowRectangle(this.hoverIndex));

                this.hoverIndex = -1;
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            this.UpdateHover(e);
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

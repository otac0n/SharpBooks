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
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    internal partial class SplitsView : UserControl
    {
        private readonly SortedList<Split> splits;

        private ReadOnlyBook book;
        private Account account;

        private Point offset;

        private int hoverIndex = -1;
        private int selectedIndex = -1;

        public SplitsView()
        {
            this.splits = new SortedList<Split>(new SplitComparer());
            this.AlternatingBackColor = Color.WhiteSmoke;
            InitializeComponent();
        }

        public event EventHandler<EventArgs> ScrollableSizeChanged;
        public event EventHandler<DesiresOffsetEventArgs> DesiresOffset;

        [Browsable(true)]
        public new event MouseEventHandler MouseWheel
        {
            add { base.MouseWheel += value; }

            remove { base.MouseWheel -= value; }
        }

        [Browsable(true)]
        public Color AlternatingBackColor { get; set; }

        public Point Offset
        {
            get { return this.offset; }

            set
            {
                this.offset = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public int SelectedIndex
        {
            get { return this.selectedIndex; }

            set
            {
                if (value < 0 ||
                    value >= this.splits.Count)
                {
                    value = -1;
                }


                if (value != this.selectedIndex)
                {
                    this.InvalidateRows(this.selectedIndex, value);

                    this.selectedIndex = value;
                }
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

        public void EnsureSelectionVisible()
        {
            if (this.selectedIndex == -1)
            {
                return;
            }

            var rect = this.GetItemRectangle(this.selectedIndex);
            var desiredOffset = new Point();

            var bottomOff = this.ClientSize.Height - rect.Bottom;
            if (bottomOff < 0)
            {
                desiredOffset.Y += bottomOff;
            }

            var topOff = rect.Top + desiredOffset.Y;
            if (topOff < 0)
            {
                desiredOffset.Y -= topOff;
            }

            desiredOffset.Offset(this.offset);
            this.DesiresOffset.SafeInvoke(this, new DesiresOffsetEventArgs(desiredOffset));
        }

        public void BeginEdit(int index)
        {
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

        private int GetItemsPerPage()
        {
            var size = this.GetItemHeight();
            return this.ClientSize.Height / size;
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

                        var rowRectangle = GetItemRectangle(i, itemHeight, offsetY, offsetX, listWidth, textPadding);

                        var split = s[i];
                        var textParts = new[]
                        {
                            (split.DateCleared ?? split.Transaction.Date).ToShortDateString(),
                            "9999",
                            "TODO: This is a placeholder description.  The real description should be loaded from the transaction metadata.",
                            split.Transaction.Splits.Where(sp => sp != split).Select(sp => sp.Account.Name).SingleOrDefault() ?? "-- Split --",
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

                        var state = ListViewItemState.Normal;

                        if (i == this.selectedIndex)
                        {
                            state = this.Focused || i == this.hoverIndex
                                ? ListViewItemState.Selected
                                : ListViewItemState.SelectedNotFocus;
                        }
                        else if (i == this.hoverIndex)
                        {
                            state = ListViewItemState.Hot;
                        }

                        ListItemRenderer.RenderItems(
                            g,
                            rowRectangle,
                            alternatingRow ? background : null,
                            textRectangles,
                            textParts,
                            this.Font,
                            state);
                    }
                }
            }

            base.OnPaint(e);
        }

        private Rectangle GetItemRectangle(int i)
        {
            var itemHeight = this.GetItemHeight();
            var listWidth = this.ClientSize.Width;
            var textPadding = this.Padding.Top;
            return GetItemRectangle(i, itemHeight, this.offset.Y, this.offset.X, listWidth, textPadding);
        }

        private static Rectangle GetItemRectangle(int i, int itemHeight, int offsetY, int offsetX, int listWidth, int textPadding)
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
                this.InvalidateRows(this.hoverIndex, hoverIndex);

                this.hoverIndex = hoverIndex;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.hoverIndex != -1)
            {
                this.InvalidateRows(this.hoverIndex);

                this.hoverIndex = -1;
            }

            base.OnMouseLeave(e);
        }

        private void InvalidateRows(params int[] rows)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                var r = rows[i];
                if (r != -1)
                {
                    this.Invalidate(this.GetItemRectangle(r));
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            this.UpdateHover(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.SelectedIndex = this.GetVirtualRow(e.Location);
            this.EnsureSelectionVisible();

            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (this.selectedIndex != -1)
            {
                this.BeginEdit(this.selectedIndex);
            }

            base.OnMouseDoubleClick(e);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Down:
                    this.SelectedIndex = (this.selectedIndex + 1).Clamp(0, this.splits.Count - 1);
                    this.EnsureSelectionVisible();
                    return true;

                case Keys.Up:
                    this.SelectedIndex = (this.selectedIndex - 1).Clamp(0, this.splits.Count - 1);
                    this.EnsureSelectionVisible();
                    return true;

                case Keys.PageDown:
                    this.SelectedIndex = (this.selectedIndex + (this.GetItemsPerPage() - 1)).Clamp(0, this.splits.Count - 1);
                    this.EnsureSelectionVisible();
                    return true;

                case Keys.PageUp:
                    this.SelectedIndex = (this.selectedIndex - (this.GetItemsPerPage() - 1)).Clamp(0, this.splits.Count - 1);
                    this.EnsureSelectionVisible();
                    return true;

                case Keys.Control | Keys.End:
                    this.SelectedIndex = this.splits.Count - 1;
                    this.EnsureSelectionVisible();
                    return true;

                case Keys.Control | Keys.Home:
                    this.SelectedIndex = 0;
                    this.EnsureSelectionVisible();
                    return true;

                case Keys.Enter:
                    if (this.SelectedIndex != -1)
                    {
                        this.BeginEdit(this.selectedIndex);
                    }

                    return true;

                case Keys.Left:
                case Keys.Right:
                    return true;
            }

            return base.ProcessDialogKey(keyData);
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

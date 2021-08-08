// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public partial class HeaderControl : UserControl
    {
        private readonly List<ColumnHeader> columns = new List<ColumnHeader>();

        private int hoverColumn = -1;
        private bool hoverResize = true;

        private MouseEventArgs lastMouseMove;
        private int originalWidth;
        private Point resizeSource;
        private bool resizing = false;

        public HeaderControl()
        {
            this.CellPadding = 3;
            this.Columns = new ColumnHeaderCollection(this);
            this.InitializeComponent();
        }

        public event EventHandler<ColumnWidthChangedEventArgs> ColumnWidthChanged;

        public int CellPadding { get; set; }

        public ColumnHeaderCollection Columns { get; }

        public Rectangle[] GetColumnBounds()
        {
            var bounds = new Rectangle[this.columns.Count];

            var height = this.ClientSize.Height;
            var left = 0;

            for (var i = 0; i < this.columns.Count; i++)
            {
                var w = this.columns[i].Width;
                bounds[i] = new Rectangle(left, 0, w, height);
                left += w;
            }

            return bounds;
        }

        internal void ColumnCanResizeChanged(ColumnHeader header)
        {
            var index = this.columns.IndexOf(header);
            if (this.hoverColumn == index)
            {
                if (this.resizing && !header.CanResize)
                {
                    this.CancelResize();
                }

                this.OnMouseMove(this.lastMouseMove);
            }
        }

        internal void ColumnContentChanged(ColumnHeader header)
        {
            this.Invalidate();
        }

        internal void SetColumnWidth(ColumnHeader header, int width)
        {
            if (width < header.MinWidth)
            {
                width = header.MinWidth;
            }

            header.width = width;
            this.ColumnWidthChanged?.Invoke(this, new ColumnWidthChangedEventArgs(this.columns.IndexOf(header)));
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.hoverResize)
            {
                this.StartResize(e);
            }
            else if (e.Button == MouseButtons.Right && this.resizing)
            {
                this.CancelResize();
                this.OnMouseMove(e);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.UpdateHover(-1, false);

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.lastMouseMove = e;

            if (!this.resizing)
            {
                var column = this.FindHoveredColumn(e, out var showResize);

                this.UpdateHover(column, showResize);
            }
            else
            {
                this.DoResize(e);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.resizing = false;
            }

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const TextFormatFlags BaseFormat = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;

            var g = e.Graphics;
            var height = this.ClientSize.Height;
            var width = this.ClientSize.Width;

            var left = 0;
            for (var i = 0; i < this.columns.Count; i++)
            {
                var col = this.columns[i];
                var w = col.Width;
                var rect = new Rectangle(left, 0, w, height);
                left += w;

                var format = BaseFormat;
                switch (col.TextAlign)
                {
                    case HorizontalAlign.Center:
                        format |= TextFormatFlags.HorizontalCenter;
                        break;

                    case HorizontalAlign.Left:
                        format |= TextFormatFlags.Left;
                        break;

                    case HorizontalAlign.Right:
                        format |= TextFormatFlags.Right;
                        break;
                }

                HeaderRenderer.DrawHeader(g, rect, col.Text, this.Font, format, i == this.hoverColumn ? HeaderItemState.Hot : HeaderItemState.Normal);
            }

            if (left < width)
            {
                HeaderRenderer.DrawHeader(g, new Rectangle(left, 0, width - left, height), HeaderItemState.Normal);
            }
        }

        private void CancelResize()
        {
            this.resizing = false;
            this.columns[this.hoverColumn].Width = this.originalWidth;
            this.ColumnWidthChanged?.Invoke(this, new ColumnWidthChangedEventArgs(this.hoverColumn));
        }

        private void ClearColumns()
        {
            foreach (var column in this.columns)
            {
                column.headerControl = null;
            }

            this.columns.Clear();
        }

        private void DoResize(MouseEventArgs e)
        {
            var offset = e.Location.X - this.resizeSource.X;
            var desiredWidth = this.originalWidth + offset;
            if (desiredWidth < 0)
            {
                desiredWidth = 0;
            }

            this.SetColumnWidth(this.columns[this.hoverColumn], desiredWidth);
        }

        private int FindHoveredColumn(MouseEventArgs e, out bool showResize)
        {
            var hoverColumn = -1;
            var hoverResize = -1;

            var bounds = this.GetColumnBounds();
            var padding = this.CellPadding;

            for (var i = 0; i < bounds.Length; i++)
            {
                var rect = bounds[i];
                var loc = e.Location;

                if (hoverColumn == -1)
                {
                    if (rect.Contains(loc))
                    {
                        hoverColumn = i;
                    }
                }

                if (this.columns[i].CanResize)
                {
                    rect = new Rectangle(rect.Right - 1 - padding, rect.Y, 2 * padding + 1, rect.Height);
                    if (rect.Contains(loc))
                    {
                        hoverResize = i;
                    }
                }
            }

            showResize = hoverResize != -1;
            return showResize ? hoverResize : hoverColumn;
        }

        private void InsertColumn(int index, ColumnHeader header)
        {
            if (index < 0 || index > this.columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (header.headerControl != null)
            {
                throw new ArgumentException("Only one HeaderControl may contain a given ColumnHeader.", nameof(header));
            }

            this.columns.Insert(index, header);
            header.headerControl = this;
        }

        private bool RemoveColumn(int index)
        {
            if (index < 0 || index >= this.columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var header = this.columns[index];
            this.columns.RemoveAt(index);
            header.headerControl = null;

            return true;
        }

        private void StartResize(MouseEventArgs e)
        {
            this.resizing = true;
            this.resizeSource = e.Location;
            this.originalWidth = this.columns[this.hoverColumn].Width;
        }

        private void UpdateHover(int hoverColumn, bool showResize)
        {
            if (this.hoverColumn != hoverColumn)
            {
                this.hoverColumn = hoverColumn;
                this.Invalidate();
            }

            this.hoverResize = showResize;
            this.Cursor = showResize ? Cursors.VSplit : Cursors.Arrow;
        }

        public class ColumnHeader
        {
            private bool canResize = true;
            private HeaderControl headerControl;
            private int minWidth = 10;
            private string text;
            private HorizontalAlign textAlign = HorizontalAlign.Left;
            private int width = 60;

            public bool CanResize
            {
                get => this.canResize;

                set
                {
                    this.canResize = value;
                    this.CanResizeChanged();
                }
            }

            public HeaderControl HeaderControl => this.headerControl;

            public int Index =>
                this.headerControl != null
                    ? this.headerControl.columns.IndexOf(this)
                    : -1;

            public int MinWidth
            {
                get => this.minWidth;

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    this.minWidth = value;
                    if (this.width < this.minWidth)
                    {
                        this.SetColumnWidth(this.minWidth);
                    }
                }
            }

            public object Tag { get; set; }

            public string Text
            {
                get => this.text ?? "ColumnHeader";

                set
                {
                    this.text = value ?? string.Empty;

                    this.ColumnContentChanged();
                }
            }

            public HorizontalAlign TextAlign
            {
                get => this.textAlign;

                set
                {
                    var val = (int)value;
                    if (val < 0 || val > 2)
                    {
                        throw new InvalidEnumArgumentException("value", val, typeof(HorizontalAlign));
                    }

                    this.textAlign = value;
                    this.ColumnContentChanged();
                }
            }

            public int Width
            {
                get => this.width;

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    this.SetColumnWidth(value < this.minWidth ? this.minWidth : value);
                }
            }

            private void CanResizeChanged()
            {
                if (this.headerControl != null)
                {
                    this.headerControl.ColumnCanResizeChanged(this);
                }
            }

            private void ColumnContentChanged()
            {
                if (this.headerControl != null)
                {
                    this.headerControl.ColumnContentChanged(this);
                }
            }

            private void SetColumnWidth(int width)
            {
                if (this.headerControl != null)
                {
                    this.headerControl.SetColumnWidth(this, width);
                }
                else
                {
                    if (width < this.minWidth)
                    {
                        width = this.minWidth;
                    }

                    this.width = width;
                }
            }
        }

        public class ColumnHeaderCollection : ICollection<ColumnHeader>
        {
            private HeaderControl owner;

            internal ColumnHeaderCollection(HeaderControl owner)
            {
                this.owner = owner;
            }

            public int Count => this.owner.columns.Count;

            public bool IsReadOnly => false;

            public ColumnHeader this[int index] => this.owner.columns[index];

            public void Add(ColumnHeader header)
            {
                this.owner.InsertColumn(this.Count, header);
            }

            public ColumnHeader Add(string text, int width)
            {
                var header = new ColumnHeader
                {
                    Text = text,
                    Width = width,
                };

                this.Add(header);

                return header;
            }

            public void Clear()
            {
                this.owner.ClearColumns();
            }

            public bool Contains(ColumnHeader header)
            {
                return this.owner.columns.Contains(header);
            }

            public void CopyTo(ColumnHeader[] array, int arrayIndex)
            {
                this.owner.columns.CopyTo(array, arrayIndex);
            }

            public IEnumerator<ColumnHeader> GetEnumerator()
            {
                return this.owner.columns.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.owner.columns.GetEnumerator();
            }

            public int IndexOf(ColumnHeader header)
            {
                return this.owner.columns.IndexOf(header);
            }

            public void Insert(int index, ColumnHeader header)
            {
                this.owner.InsertColumn(index, header);
            }

            public bool Remove(ColumnHeader header)
            {
                var index = this.IndexOf(header);
                return index != -1 && this.owner.RemoveColumn(index);
            }

            public void RemoveAt(int index)
            {
                this.owner.RemoveColumn(index);
            }
        }
    }
}

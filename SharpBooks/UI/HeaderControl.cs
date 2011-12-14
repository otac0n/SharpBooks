using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections;

namespace SharpBooks.UI
{
    public partial class HeaderControl : UserControl
    {
        private readonly ColumnHeaderCollection columnHeaders;
        private readonly List<ColumnHeader> columns = new List<ColumnHeader>();

        private VisualStyleRenderer normalRenderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);
        private VisualStyleRenderer hotRenderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Hot);
        private VisualStyleRenderer pressedRenderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Pressed);

        public HeaderControl()
        {
            this.CellPadding = 3;
            this.columnHeaders = new ColumnHeaderCollection(this);
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const TextFormatFlags BaseFormat = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;

            var g = e.Graphics;
            var height = this.ClientSize.Height;
            var width = this.ClientSize.Width;

            int left = 0;
            for (int i = 0; i < this.columns.Count; i++)
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

                normalRenderer.DrawBackground(g, rect);
                rect.Inflate(-this.CellPadding, -this.CellPadding);
                normalRenderer.DrawText(g, rect, col.Text, false, format);
            }

            if (left < width)
            {
                normalRenderer.DrawBackground(g, new Rectangle(left, 0, width - left, height));
            }
        }

        public ColumnHeaderCollection Columns
        {
            get { return this.columnHeaders; }
        }

        public int CellPadding { get; set; }

        private void HeaderControl_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var columnHeader in this.Columns)
            {
                this.Cursor = Cursors.VSplit;
            }
        }

        private void InsertColumn(int index, ColumnHeader header)
        {
            if (index < 0 || index > this.columns.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (header.headerControl != null)
            {
                throw new ArgumentException("Only one HeaderControl may contain a given ColumnHeader.", "header");
            }

            this.columns.Insert(index, header);
            header.headerControl = this;
        }

        private void ClearColumns()
        {
            foreach (var column in this.columns)
            {
                column.headerControl = null;
            }

            this.columns.Clear();
        }

        private bool RemoveColumn(int index)
        {
            if (index < 0 || index >= this.columns.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            var header = this.columns[index];
            this.columns.RemoveAt(index);
            header.headerControl = null;

            return true;
        }

        public class ColumnHeader
        {
            internal HeaderControl headerControl;
            private int width = 60;
            private string text;
            private HorizontalAlign textAlign = HorizontalAlign.Left;

            public int Width
            {
                get { return this.width; }

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }

                    this.width = value;
                    this.NotifyHeaderControl();
                }
            }

            public HeaderControl HeaderControl
            {
                get { return this.headerControl; }
            }

            public int Index
            {
                get
                {
                    return this.headerControl != null
                        ? this.headerControl.columns.IndexOf(this)
                        : -1;
                }
            }

            public object Tag { get; set; }

            public string Text
            {
                get { return this.text ?? "ColumnHeader"; }

                set
                {
                    this.text = value ?? "";
                    this.NotifyHeaderControl();
                }
            }

            public HorizontalAlign TextAlign
            {
                get { return this.textAlign; }

                set
                {
                    var val = (int)value;
                    if (val < 0 || val > 2)
                    {
                        throw new InvalidEnumArgumentException("value", val, typeof(HorizontalAlign));
                    }

                    this.textAlign = value;
                    this.NotifyHeaderControl();
                }
            }

            private void NotifyHeaderControl()
            {
                if (this.headerControl != null)
                {
                    this.headerControl.Invalidate();
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

            public void Add(ColumnHeader header)
            {
                this.owner.InsertColumn(this.Count, header);
            }

            public void Add(string text, int width)
            {
                var header = new ColumnHeader
                {
                    Text = text,
                    Width = width,
                };

                this.Add(header);
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

            public int Count
            {
                get { return this.owner.columns.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(ColumnHeader header)
            {
                var index = this.IndexOf(header);
                return index != -1 && this.owner.RemoveColumn(index);
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

            public void RemoveAt(int index)
            {
                this.owner.RemoveColumn(index);
            }

            public ColumnHeader this[int index]
            {
                get
                {
                    return this.owner.columns[index];
                }
            }
        }
    }
}

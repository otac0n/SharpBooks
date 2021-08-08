// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using SharpBooks.Plugins;

    public partial class AccountTree : UserControl
    {
        private readonly Dictionary<Account, TreeNode> nodeLookup = new Dictionary<Account, TreeNode>();

        private ReadOnlyBook book;

        public AccountTree()
        {
            InitializeComponent();
        }

        public event EventHandler<AccountDeleteRequestedEventArgs> AccountDeleteRequested;

        public event EventHandler<AccountSelectedEventArgs> AccountSelected;

        public event EventHandler<NewAccountRequestedEventArgs> NewAccountRequested;

        [Browsable(false)]
        public ReadOnlyBook Book
        {
            get
            {
                return this.book;
            }

            set
            {
                if (this.book != value)
                {
                    this.DetachBookHandlers();

                    this.tree.Nodes.Clear();
                    this.nodeLookup.Clear();

                    this.book = value;
                    this.AttachBookHandlers();
                    this.InitializeBook();
                }
            }
        }

        public ImageList ImageList
        {
            get { return this.tree.ImageList; }
            set { this.tree.ImageList = value; }
        }

        private void AttachBookHandlers()
        {
            if (this.book != null)
            {
                this.book.AccountAdded += book_AccountAdded;
                this.book.AccountRemoved += book_AccountRemoved;
            }
        }

        private void book_AccountAdded(object sender, Events.AccountAddedEventArgs e)
        {
            var a = e.Account;

            var node = new TreeNode  // TODO: Determine the icon from the metadata.
            {
                Text = a.Name,
                Name = a.Name,
                Tag = a,
            };
            this.nodeLookup.Add(a, node);

            // TODO: Insert the book in the correct order.
            if (a.ParentAccount != null)
            {
                var parentNode = this.nodeLookup[a.ParentAccount];
                parentNode.Nodes.Add(node);
            }
            else
            {
                this.tree.Nodes.Add(node);
            }
        }

        private void book_AccountRemoved(object sender, Events.AccountRemovedEventArgs e)
        {
            var node = this.nodeLookup[e.Account];
            this.nodeLookup.Remove(e.Account);

            if (node.Parent != null)
            {
                node.Parent.Nodes.Remove(node);
            }
            else
            {
                this.tree.Nodes.Remove(node);
            }
        }

        private TreeNode[] BuildTreeNodes(Account parentAccount, ILookup<Account, Account> accounts)
        {
            var nodes = new List<TreeNode>();

            foreach (var a in accounts[parentAccount])  // TODO: Order by "order" in the metadata.
            {
                var node = new TreeNode  // TODO: Determine the icon from the metadata.
                {
                    Text = a.Name,
                    Name = a.Name,
                    Tag = a,
                };
                this.nodeLookup.Add(a, node);

                node.Nodes.AddRange(BuildTreeNodes(a, accounts));

                nodes.Add(node);
            }

            return nodes.ToArray();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var account = (Account)this.tree.SelectedNode.Tag;

            this.AccountDeleteRequested.SafeInvoke(this, () => new AccountDeleteRequestedEventArgs { AccountId = account.AccountId });
        }

        private void DetachBookHandlers()
        {
            if (this.book != null)
            {
                this.book.AccountAdded -= book_AccountAdded;
                this.book.AccountRemoved -= book_AccountRemoved;
            }
        }

        private void InitializeBook()
        {
            if (this.book != null)
            {
                var accounts = this.book.Accounts.ToLookup(a => a.ParentAccount);

                this.tree.Nodes.AddRange(BuildTreeNodes(null, accounts));
            }
        }

        private void newAccount_Click(object sender, EventArgs e)
        {
            var parentAccount = (Account)this.tree.SelectedNode.Tag;

            this.NewAccountRequested.SafeInvoke(this, () => new NewAccountRequestedEventArgs { ParentAccountId = parentAccount.AccountId });
        }

        private void open_Click(object sender, EventArgs e)
        {
            var account = (Account)this.tree.SelectedNode.Tag;

            this.AccountSelected.SafeInvoke(this, () => new AccountSelectedEventArgs { AccountId = account.AccountId });
        }

        private void tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var g = e.Graphics;
            var node = e.Node;
            var bounds = e.Bounds;
            var state = e.State;

            var balance = ((Account)node.Tag).TotalBalance;
            var amountText = balance.ToString();

            var isFocused = (state & TreeNodeStates.Focused) == TreeNodeStates.Focused;
            var thisFocused = node.TreeView.Focused;
            var font = (node.NodeFont != null) ? node.NodeFont : node.TreeView.Font;

            var textSize = TextRenderer.MeasureText(amountText, font);
            var amountBounds = new Rectangle(this.tree.ClientSize.Width - textSize.Width, bounds.Top, textSize.Width, bounds.Height);

            using (var backBrush = new SolidBrush(this.tree.BackColor))
            {
                var foreColor = (node.ForeColor != Color.Empty) ? node.ForeColor : this.tree.ForeColor;

                var background = isFocused && thisFocused ? SystemBrushes.Highlight : backBrush;
                var foreground = isFocused && thisFocused ? SystemColors.HighlightText : foreColor;

                g.FillRectangle(background, bounds);
                TextRenderer.DrawText(g, e.Node.Text, font, bounds, foreground, TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter);

                g.FillRectangle(background, amountBounds);
                TextRenderer.DrawText(g, amountText, font, amountBounds, foreground, TextFormatFlags.Right | TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter);
            }
        }

        private void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.tree.SelectedNode = e.Node;
                this.nodeContextMenu.Show(this.tree.PointToScreen(e.Location));
            }
        }

        private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var account = (Account)e.Node.Tag;

                this.AccountSelected.SafeInvoke(this, () => new AccountSelectedEventArgs { AccountId = account.AccountId });
            }
        }
    }
}

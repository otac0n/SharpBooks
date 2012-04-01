﻿﻿//-----------------------------------------------------------------------
// <copyright file="AccountTree.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
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
    using System.Text;
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

        public event EventHandler<AccountSelectedEventArgs> AccountSelected;

        [Browsable(false)]
        public ReadOnlyBook Book
        {
            get { return this.book; }

            set
            {
                if (this.book != value)
                {
                    this.tree.Nodes.Clear();
                    this.nodeLookup.Clear();

                    this.book = value;
                    this.InitializeBook();
                }
            }
        }

        public ImageList ImageList
        {
            get { return this.tree.ImageList; }
            set { this.tree.ImageList = value; }
        }

        private void InitializeBook()
        {
            if (this.book != null)
            {
                var accounts = this.book.Accounts.ToLookup(a => a.ParentAccount);

                this.tree.Nodes.AddRange(BuildTreeNodes(null, accounts));
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

                node.Nodes.AddRange(BuildTreeNodes(a, accounts));

                nodes.Add(node);
            }

            return nodes.ToArray();
        }

        private void tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var g = e.Graphics;
            var node = e.Node;
            var bounds = e.Bounds;
            var state = e.State;

            var balance = ((Account)node.Tag).TotalBalance;
            var amountText = balance.ToString();

            bool isFocused = (state & TreeNodeStates.Focused) == TreeNodeStates.Focused;
            bool thisFocused = node.TreeView.Focused;
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

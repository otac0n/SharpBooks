// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml.Linq;
    using SharpBooks.Controllers;

    public partial class NewBookWizard : Form
    {
        private readonly MainController owner;

        public NewBookWizard(MainController owner)
        {
            this.owner = owner;

            this.InitializeComponent();

            this.LoadCurrencies();
            this.SetDefaultCurrency();
            this.LoadDefaultAccounts();
        }

        public Book NewBook { get; private set; }

        private static void AddBookAccounts(TreeNode node, Book book, Account parent)
        {
            if (node.Checked)
            {
                var account = new Account(Guid.NewGuid(), (AccountType)node.Tag, null, parent, node.Text, null);
                book.AddAccount(account);

                foreach (TreeNode child in node.Nodes)
                {
                    AddBookAccounts(child, book, account);
                }
            }
        }

        private static XDocument LoadResourceDocument(string resourceName)
        {
            using (var accountStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SharpBooks.Resources." + resourceName))
            {
                return XDocument.Load(accountStream);
            }
        }

        private void AccountsTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var ck = e.Node.Checked;

            this.SetNodeChildrenChecked(e.Node, ck);
        }

        private TreeNode[] GetTreeNodes(XElement element)
        {
            var nodes = new List<TreeNode>();

            foreach (var a in element.Elements("Account"))
            {
                var type = AccountType.Balance;

                var groupOnlyAttr = a.Attribute("groupOnly");
                if (groupOnlyAttr != null && (bool)groupOnlyAttr)
                {
                    type = AccountType.Grouping;
                }

                var node = new TreeNode
                {
                    Checked = true,
                    Text = (string)a.Attribute("name"),
                    Tag = type,
                };

                node.Nodes.AddRange(this.GetTreeNodes(a));

                nodes.Add(node);
            }

            return nodes.ToArray();
        }

        private void LoadCurrencies()
        {
            var doc = LoadResourceDocument("ISO4217.xml");

            var currencies = from currency in doc.Element("Currencies").Elements("Currency")
                             let code = (string)currency.Attribute("code")
                             let name = (string)currency.Attribute("name")
                             let symbol = (string)currency.Attribute("symbol")
                             let fraction = (int)currency.Attribute("fraction")
                             orderby symbol
                             select new Security(
                                 Guid.NewGuid(),
                                 SecurityType.Currency,
                                 name,
                                 code,
                                 new CurrencyFormat(currencySymbol: symbol),
                                 fraction);

            foreach (var sec in currencies)
            {
                var item = new ListViewItem(new[] { sec.Symbol, sec.Name })
                {
                    Tag = sec,
                };

                this.currencyList.Items.Add(item);
            }

            this.currencyList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void LoadDefaultAccounts()
        {
            var doc = LoadResourceDocument("NewBookAccounts.xml");

            this.accountsTree.Nodes.AddRange(this.GetTreeNodes(doc.Element("Accounts")));
            this.accountsTree.ExpandAll();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            var book = new Book();

            foreach (ListViewItem item in this.currencyList.Items)
            {
                if (item.Checked)
                {
                    var security = (Security)item.Tag;
                    book.AddSecurity(security);
                }
            }

            foreach (TreeNode node in this.accountsTree.Nodes)
            {
                AddBookAccounts(node, book, null);
            }

            this.NewBook = book;
        }

        private void SetDefaultCurrency()
        {
            var currentUIRegionInfo = new RegionInfo(CultureInfo.CurrentUICulture.LCID);
            var localCurrencySymbol = currentUIRegionInfo.ISOCurrencySymbol;

            var localCurrencyRow = (from ListViewItem item in this.currencyList.Items
                                    where ((Security)item.Tag).Symbol == localCurrencySymbol
                                    select item).FirstOrDefault();

            if (localCurrencyRow != null)
            {
                localCurrencyRow.Checked = true;
                localCurrencyRow.EnsureVisible();
            }
        }

        private void SetNodeChildrenChecked(TreeNode treeNode, bool ck)
        {
            foreach (TreeNode child in treeNode.Nodes)
            {
                child.Checked = ck;
                this.SetNodeChildrenChecked(child, ck);
            }
        }
    }
}

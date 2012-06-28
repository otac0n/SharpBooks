namespace SharpBooks
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.MenuStrip mainMenu;
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
            System.Windows.Forms.Button returnToAccountsButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.tabView = new System.Windows.Forms.TabControl();
            this.OverviewTabPage = new System.Windows.Forms.TabPage();
            this.AccountsTabPage = new System.Windows.Forms.TabPage();
            this.accountViewContainer = new System.Windows.Forms.Panel();
            this.accountRegister = new SharpBooks.UI.AccountRegister();
            this.accountViewActions = new System.Windows.Forms.Panel();
            this.newTransactionButton = new System.Windows.Forms.Button();
            this.accountTree = new SharpBooks.UI.AccountTree();
            this.accountImages = new System.Windows.Forms.ImageList(this.components);
            this.AccountsList = new System.Windows.Forms.TreeView();
            this.PaymentsTabPage = new System.Windows.Forms.TabPage();
            this.InvestmentsTabItem = new System.Windows.Forms.TabPage();
            mainMenu = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            returnToAccountsButton = new System.Windows.Forms.Button();
            mainMenu.SuspendLayout();
            this.tabView.SuspendLayout();
            this.AccountsTabPage.SuspendLayout();
            this.accountViewContainer.SuspendLayout();
            this.accountViewActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileToolStripMenuItem});
            mainMenu.Location = new System.Drawing.Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new System.Drawing.Size(784, 24);
            mainMenu.TabIndex = 0;
            mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            newToolStripMenuItem,
            openToolStripMenuItem,
            closeToolStripMenuItem,
            saveToolStripMenuItem,
            saveAsToolStripMenuItem});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            newToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            newToolStripMenuItem.Text = "&New";
            newToolStripMenuItem.Click += new System.EventHandler(this.New_Click);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            openToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Click += new System.EventHandler(this.Open_Click);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            closeToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            closeToolStripMenuItem.Text = "&Close";
            closeToolStripMenuItem.Click += new System.EventHandler(this.Close_Click);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += new System.EventHandler(this.Save_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            saveAsToolStripMenuItem.Text = "Save &As";
            saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs_Click);
            // 
            // returnToAccountsButton
            // 
            returnToAccountsButton.Location = new System.Drawing.Point(0, 0);
            returnToAccountsButton.Name = "returnToAccountsButton";
            returnToAccountsButton.Size = new System.Drawing.Size(108, 23);
            returnToAccountsButton.TabIndex = 0;
            returnToAccountsButton.Text = "Back to Accounts";
            returnToAccountsButton.UseVisualStyleBackColor = true;
            returnToAccountsButton.Click += new System.EventHandler(this.ReturnToAccounts_Click);
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.OverviewTabPage);
            this.tabView.Controls.Add(this.AccountsTabPage);
            this.tabView.Controls.Add(this.PaymentsTabPage);
            this.tabView.Controls.Add(this.InvestmentsTabItem);
            this.tabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabView.Location = new System.Drawing.Point(0, 24);
            this.tabView.Name = "tabView";
            this.tabView.SelectedIndex = 0;
            this.tabView.Size = new System.Drawing.Size(784, 388);
            this.tabView.TabIndex = 1;
            // 
            // OverviewTabPage
            // 
            this.OverviewTabPage.Location = new System.Drawing.Point(4, 22);
            this.OverviewTabPage.Name = "OverviewTabPage";
            this.OverviewTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.OverviewTabPage.Size = new System.Drawing.Size(776, 362);
            this.OverviewTabPage.TabIndex = 0;
            this.OverviewTabPage.Text = "Overview";
            this.OverviewTabPage.UseVisualStyleBackColor = true;
            // 
            // AccountsTabPage
            // 
            this.AccountsTabPage.Controls.Add(this.accountViewContainer);
            this.AccountsTabPage.Controls.Add(this.accountTree);
            this.AccountsTabPage.Controls.Add(this.AccountsList);
            this.AccountsTabPage.Location = new System.Drawing.Point(4, 22);
            this.AccountsTabPage.Name = "AccountsTabPage";
            this.AccountsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AccountsTabPage.Size = new System.Drawing.Size(776, 362);
            this.AccountsTabPage.TabIndex = 1;
            this.AccountsTabPage.Text = "Accounts / Expenses";
            this.AccountsTabPage.UseVisualStyleBackColor = true;
            // 
            // accountViewContainer
            // 
            this.accountViewContainer.Controls.Add(this.accountRegister);
            this.accountViewContainer.Controls.Add(this.accountViewActions);
            this.accountViewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountViewContainer.Location = new System.Drawing.Point(3, 3);
            this.accountViewContainer.Name = "accountViewContainer";
            this.accountViewContainer.Size = new System.Drawing.Size(770, 356);
            this.accountViewContainer.TabIndex = 3;
            this.accountViewContainer.Visible = false;
            // 
            // accountRegister
            // 
            this.accountRegister.BackColor = System.Drawing.SystemColors.Window;
            this.accountRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountRegister.Location = new System.Drawing.Point(0, 23);
            this.accountRegister.Name = "accountRegister";
            this.accountRegister.Size = new System.Drawing.Size(770, 333);
            this.accountRegister.TabIndex = 3;
            this.accountRegister.TransactionUpdated += new System.EventHandler<SharpBooks.UI.TransactionUpdatedEventArgs>(this.AccountRegister_TransactionUpdated);
            this.accountRegister.TransactionCreated += new System.EventHandler<SharpBooks.UI.TransactionCreatedEventArgs>(this.AccountRegister_TransactionCreated);
            // 
            // accountViewActions
            // 
            this.accountViewActions.Controls.Add(this.newTransactionButton);
            this.accountViewActions.Controls.Add(returnToAccountsButton);
            this.accountViewActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.accountViewActions.Location = new System.Drawing.Point(0, 0);
            this.accountViewActions.Name = "accountViewActions";
            this.accountViewActions.Size = new System.Drawing.Size(770, 23);
            this.accountViewActions.TabIndex = 4;
            // 
            // newTransactionButton
            // 
            this.newTransactionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newTransactionButton.Location = new System.Drawing.Point(662, 0);
            this.newTransactionButton.Name = "newTransactionButton";
            this.newTransactionButton.Size = new System.Drawing.Size(108, 23);
            this.newTransactionButton.TabIndex = 1;
            this.newTransactionButton.Text = "&New Transaction";
            this.newTransactionButton.UseVisualStyleBackColor = true;
            this.newTransactionButton.Click += new System.EventHandler(this.newTransactionButton_Click);
            // 
            // accountTree
            // 
            this.accountTree.Book = null;
            this.accountTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountTree.ImageList = this.accountImages;
            this.accountTree.Location = new System.Drawing.Point(3, 3);
            this.accountTree.Name = "accountTree";
            this.accountTree.Size = new System.Drawing.Size(770, 356);
            this.accountTree.TabIndex = 1;
            this.accountTree.AccountSelected += new System.EventHandler<SharpBooks.Plugins.AccountSelectedEventArgs>(this.AccountTree_AccountSelected);
            this.accountTree.NewAccountRequested += new System.EventHandler<SharpBooks.Plugins.NewAccountRequestedEventArgs>(this.AccountTree_NewAccountRequested);
            // 
            // accountImages
            // 
            this.accountImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("accountImages.ImageStream")));
            this.accountImages.TransparentColor = System.Drawing.Color.Transparent;
            this.accountImages.Images.SetKeyName(0, "Coinstack.png");
            // 
            // AccountsList
            // 
            this.AccountsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccountsList.Location = new System.Drawing.Point(3, 3);
            this.AccountsList.Name = "AccountsList";
            this.AccountsList.Size = new System.Drawing.Size(770, 356);
            this.AccountsList.TabIndex = 0;
            // 
            // PaymentsTabPage
            // 
            this.PaymentsTabPage.Location = new System.Drawing.Point(4, 22);
            this.PaymentsTabPage.Name = "PaymentsTabPage";
            this.PaymentsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PaymentsTabPage.Size = new System.Drawing.Size(776, 362);
            this.PaymentsTabPage.TabIndex = 2;
            this.PaymentsTabPage.Text = "Payments";
            this.PaymentsTabPage.UseVisualStyleBackColor = true;
            // 
            // InvestmentsTabItem
            // 
            this.InvestmentsTabItem.Location = new System.Drawing.Point(4, 22);
            this.InvestmentsTabItem.Name = "InvestmentsTabItem";
            this.InvestmentsTabItem.Padding = new System.Windows.Forms.Padding(3);
            this.InvestmentsTabItem.Size = new System.Drawing.Size(776, 362);
            this.InvestmentsTabItem.TabIndex = 3;
            this.InvestmentsTabItem.Text = "Investments";
            this.InvestmentsTabItem.UseVisualStyleBackColor = true;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 412);
            this.Controls.Add(this.tabView);
            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;
            this.Name = "MainView";
            this.Text = "MainView";
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            this.tabView.ResumeLayout(false);
            this.AccountsTabPage.ResumeLayout(false);
            this.accountViewContainer.ResumeLayout(false);
            this.accountViewActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabView;
        private System.Windows.Forms.TabPage OverviewTabPage;
        private System.Windows.Forms.TabPage AccountsTabPage;
        private System.Windows.Forms.TreeView AccountsList;
        private System.Windows.Forms.TabPage PaymentsTabPage;
        private System.Windows.Forms.TabPage InvestmentsTabItem;
        private SharpBooks.UI.AccountTree accountTree;
        private System.Windows.Forms.ImageList accountImages;
        private System.Windows.Forms.Panel accountViewContainer;
        private UI.AccountRegister accountRegister;
        private System.Windows.Forms.Panel accountViewActions;
        private System.Windows.Forms.Button newTransactionButton;

    }
}
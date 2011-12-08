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
            System.Windows.Forms.MenuStrip mainMenu;
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            this.TabView = new System.Windows.Forms.TabControl();
            this.OverviewTabPage = new System.Windows.Forms.TabPage();
            this.AccountsTabPage = new System.Windows.Forms.TabPage();
            this.AccountsList = new System.Windows.Forms.TreeView();
            this.PaymentsTabPage = new System.Windows.Forms.TabPage();
            this.InvestmentsTabItem = new System.Windows.Forms.TabPage();
            mainMenu = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            mainMenu.SuspendLayout();
            this.TabView.SuspendLayout();
            this.AccountsTabPage.SuspendLayout();
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
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // TabView
            // 
            this.TabView.Controls.Add(this.OverviewTabPage);
            this.TabView.Controls.Add(this.AccountsTabPage);
            this.TabView.Controls.Add(this.PaymentsTabPage);
            this.TabView.Controls.Add(this.InvestmentsTabItem);
            this.TabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabView.Location = new System.Drawing.Point(0, 24);
            this.TabView.Name = "TabView";
            this.TabView.SelectedIndex = 0;
            this.TabView.Size = new System.Drawing.Size(784, 388);
            this.TabView.TabIndex = 1;
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
            this.AccountsTabPage.Controls.Add(this.AccountsList);
            this.AccountsTabPage.Location = new System.Drawing.Point(4, 22);
            this.AccountsTabPage.Name = "AccountsTabPage";
            this.AccountsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AccountsTabPage.Size = new System.Drawing.Size(776, 362);
            this.AccountsTabPage.TabIndex = 1;
            this.AccountsTabPage.Text = "Accounts / Expenses";
            this.AccountsTabPage.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.TabView);
            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;
            this.Name = "MainView";
            this.Text = "MainView";
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            this.TabView.ResumeLayout(false);
            this.AccountsTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabView;
        private System.Windows.Forms.TabPage OverviewTabPage;
        private System.Windows.Forms.TabPage AccountsTabPage;
        private System.Windows.Forms.TreeView AccountsList;
        private System.Windows.Forms.TabPage PaymentsTabPage;
        private System.Windows.Forms.TabPage InvestmentsTabItem;

    }
}
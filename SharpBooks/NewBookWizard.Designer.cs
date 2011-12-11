namespace SharpBooks
{
    partial class NewBookWizard
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
            System.Windows.Forms.Button okButton;
            System.Windows.Forms.Button cancelButton;
            this.accountsTree = new System.Windows.Forms.TreeView();
            this.currencyList = new System.Windows.Forms.ListView();
            this.currencyCodeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.currencyNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            okButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(296, 475);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(60, 25);
            okButton.TabIndex = 1;
            okButton.Text = "&OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(362, 475);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(60, 25);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "&Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // accountsTree
            // 
            this.accountsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountsTree.CheckBoxes = true;
            this.accountsTree.Location = new System.Drawing.Point(12, 234);
            this.accountsTree.Name = "accountsTree";
            this.accountsTree.Size = new System.Drawing.Size(410, 235);
            this.accountsTree.TabIndex = 3;
            this.accountsTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.AccountsTree_AfterCheck);
            // 
            // currencyList
            // 
            this.currencyList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currencyList.CheckBoxes = true;
            this.currencyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.currencyCodeHeader,
            this.currencyNameHeader});
            this.currencyList.FullRowSelect = true;
            this.currencyList.Location = new System.Drawing.Point(12, 67);
            this.currencyList.Name = "currencyList";
            this.currencyList.Size = new System.Drawing.Size(410, 111);
            this.currencyList.TabIndex = 4;
            this.currencyList.UseCompatibleStateImageBehavior = false;
            this.currencyList.View = System.Windows.Forms.View.Details;
            // 
            // currencyCodeHeader
            // 
            this.currencyCodeHeader.Text = "Code";
            // 
            // currencyNameHeader
            // 
            this.currencyNameHeader.Text = "Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Step 1:  Choose your currencies.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Step 2: Choose your default accounts.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "New Book";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(408, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "(You will have a chance to add more accounts immediately after the book is create" +
    "d.)";
            // 
            // NewBookWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 512);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.currencyList);
            this.Controls.Add(this.accountsTree);
            this.Controls.Add(cancelButton);
            this.Controls.Add(okButton);
            this.Name = "NewBookWizard";
            this.Text = "New Book";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView accountsTree;
        private System.Windows.Forms.ListView currencyList;
        private System.Windows.Forms.ColumnHeader currencyCodeHeader;
        private System.Windows.Forms.ColumnHeader currencyNameHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
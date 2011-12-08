namespace SharpBooks.StandardPlugins
{
    partial class FavoriteAccountsConfiguration
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
            System.Windows.Forms.Button okButton;
            System.Windows.Forms.Button cancelButton;
            System.Windows.Forms.DataGridView accountGrid;
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.favoriteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accountViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            okButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            accountGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(accountGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountViewBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Location = new System.Drawing.Point(146, 225);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(60, 25);
            okButton.TabIndex = 1;
            okButton.Text = "&OK";
            okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(212, 225);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(60, 25);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "&Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // accountGrid
            // 
            accountGrid.AllowUserToAddRows = false;
            accountGrid.AllowUserToDeleteRows = false;
            accountGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            accountGrid.AutoGenerateColumns = false;
            accountGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            accountGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            accountGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.favoriteDataGridViewCheckBoxColumn});
            accountGrid.DataSource = this.accountViewBindingSource;
            accountGrid.Location = new System.Drawing.Point(12, 12);
            accountGrid.Name = "accountGrid";
            accountGrid.RowHeadersVisible = false;
            accountGrid.Size = new System.Drawing.Size(260, 207);
            accountGrid.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Account Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 95;
            // 
            // favoriteDataGridViewCheckBoxColumn
            // 
            this.favoriteDataGridViewCheckBoxColumn.DataPropertyName = "Favorite";
            this.favoriteDataGridViewCheckBoxColumn.HeaderText = "Favorite";
            this.favoriteDataGridViewCheckBoxColumn.Name = "favoriteDataGridViewCheckBoxColumn";
            this.favoriteDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.favoriteDataGridViewCheckBoxColumn.Width = 51;
            // 
            // accountViewBindingSource
            // 
            this.accountViewBindingSource.AllowNew = false;
            this.accountViewBindingSource.DataSource = typeof(SharpBooks.StandardPlugins.FavoriteAccountsConfiguration.AccountView);
            // 
            // FavoriteAccountsConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(cancelButton);
            this.Controls.Add(okButton);
            this.Controls.Add(accountGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FavoriteAccountsConfiguration";
            this.Text = "Configure Favorite Accounts";
            ((System.ComponentModel.ISupportInitialize)(accountGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountViewBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource accountViewBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn favoriteDataGridViewCheckBoxColumn;
    }
}
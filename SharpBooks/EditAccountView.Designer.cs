namespace SharpBooks
{
    partial class EditAccountView
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
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label typeLabel;
            System.Windows.Forms.Label currencyLabel;
            System.Windows.Forms.Label fractionLabel;
            System.Windows.Forms.Button cancelButton;
            this.titleLabel = new System.Windows.Forms.Label();
            this.balanceAccountRadio = new System.Windows.Forms.RadioButton();
            this.groupingAccountRadio = new System.Windows.Forms.RadioButton();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.securityComboBox = new System.Windows.Forms.ComboBox();
            this.fractionTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            nameLabel = new System.Windows.Forms.Label();
            typeLabel = new System.Windows.Forms.Label();
            currencyLabel = new System.Windows.Forms.Label();
            fractionLabel = new System.Windows.Forms.Label();
            cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(12, 57);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(38, 13);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "&Name:";
            // 
            // typeLabel
            // 
            typeLabel.AutoSize = true;
            typeLabel.Location = new System.Drawing.Point(12, 82);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new System.Drawing.Size(77, 13);
            typeLabel.TabIndex = 3;
            typeLabel.Text = "Account Type:";
            // 
            // currencyLabel
            // 
            currencyLabel.AutoSize = true;
            currencyLabel.Location = new System.Drawing.Point(12, 129);
            currencyLabel.Name = "currencyLabel";
            currencyLabel.Size = new System.Drawing.Size(101, 13);
            currencyLabel.TabIndex = 6;
            currencyLabel.Text = "Currency / &Security:";
            // 
            // fractionLabel
            // 
            fractionLabel.AutoSize = true;
            fractionLabel.Location = new System.Drawing.Point(12, 156);
            fractionLabel.Name = "fractionLabel";
            fractionLabel.Size = new System.Drawing.Size(90, 13);
            fractionLabel.TabIndex = 8;
            fractionLabel.Text = "Smallest &Fraction:";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(248, 198);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 11;
            cancelButton.Text = "&Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(11, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(114, 20);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "New Account";
            // 
            // balanceAccountRadio
            // 
            this.balanceAccountRadio.AutoSize = true;
            this.balanceAccountRadio.Location = new System.Drawing.Point(119, 80);
            this.balanceAccountRadio.Name = "balanceAccountRadio";
            this.balanceAccountRadio.Size = new System.Drawing.Size(107, 17);
            this.balanceAccountRadio.TabIndex = 4;
            this.balanceAccountRadio.TabStop = true;
            this.balanceAccountRadio.Text = "&Balance Account";
            this.balanceAccountRadio.UseVisualStyleBackColor = true;
            // 
            // groupingAccountRadio
            // 
            this.groupingAccountRadio.AutoSize = true;
            this.groupingAccountRadio.Location = new System.Drawing.Point(119, 103);
            this.groupingAccountRadio.Name = "groupingAccountRadio";
            this.groupingAccountRadio.Size = new System.Drawing.Size(111, 17);
            this.groupingAccountRadio.TabIndex = 5;
            this.groupingAccountRadio.TabStop = true;
            this.groupingAccountRadio.Text = "&Grouping Account";
            this.groupingAccountRadio.UseVisualStyleBackColor = true;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(119, 54);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(204, 20);
            this.nameTextBox.TabIndex = 2;
            // 
            // securityComboBox
            // 
            this.securityComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.securityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.securityComboBox.FormattingEnabled = true;
            this.securityComboBox.Location = new System.Drawing.Point(119, 126);
            this.securityComboBox.Name = "securityComboBox";
            this.securityComboBox.Size = new System.Drawing.Size(204, 21);
            this.securityComboBox.TabIndex = 7;
            this.securityComboBox.SelectedValueChanged += new System.EventHandler(this.securityComboBox_SelectedValueChanged);
            // 
            // fractionTextBox
            // 
            this.fractionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fractionTextBox.Location = new System.Drawing.Point(119, 153);
            this.fractionTextBox.Name = "fractionTextBox";
            this.fractionTextBox.Size = new System.Drawing.Size(204, 20);
            this.fractionTextBox.TabIndex = 9;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(167, 198);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 10;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // EditAccountView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 233);
            this.Controls.Add(cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fractionTextBox);
            this.Controls.Add(fractionLabel);
            this.Controls.Add(currencyLabel);
            this.Controls.Add(this.securityComboBox);
            this.Controls.Add(typeLabel);
            this.Controls.Add(nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.groupingAccountRadio);
            this.Controls.Add(this.balanceAccountRadio);
            this.Controls.Add(this.titleLabel);
            this.Name = "EditAccountView";
            this.Text = "New Account";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton balanceAccountRadio;
        private System.Windows.Forms.RadioButton groupingAccountRadio;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ComboBox securityComboBox;
        private System.Windows.Forms.TextBox fractionTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label titleLabel;
    }
}
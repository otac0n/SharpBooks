namespace SharpBooks.UI
{
    partial class TransactionEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label transactionDateLabel;
            System.Windows.Forms.Label clearedLabel;
            this.saveButton = new System.Windows.Forms.Button();
            this.discardButton = new System.Windows.Forms.Button();
            this.transactionDatePicker = new System.Windows.Forms.DateTimePicker();
            this.depositTextBox = new System.Windows.Forms.TextBox();
            this.withdrawalTextBox = new System.Windows.Forms.TextBox();
            this.clearDatePicker = new System.Windows.Forms.DateTimePicker();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.accountComboBox = new System.Windows.Forms.ComboBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            transactionDateLabel = new System.Windows.Forms.Label();
            clearedLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // transactionDateLabel
            // 
            transactionDateLabel.AutoSize = true;
            transactionDateLabel.Location = new System.Drawing.Point(3, 7);
            transactionDateLabel.Name = "transactionDateLabel";
            transactionDateLabel.Size = new System.Drawing.Size(90, 13);
            transactionDateLabel.TabIndex = 0;
            transactionDateLabel.Text = "Transaction date:";
            // 
            // clearedLabel
            // 
            clearedLabel.AutoSize = true;
            clearedLabel.Location = new System.Drawing.Point(3, 33);
            clearedLabel.Name = "clearedLabel";
            clearedLabel.Size = new System.Drawing.Size(46, 13);
            clearedLabel.TabIndex = 6;
            clearedLabel.Text = "Cleared:";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(572, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 20);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // discardButton
            // 
            this.discardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.discardButton.Location = new System.Drawing.Point(572, 29);
            this.discardButton.Name = "discardButton";
            this.discardButton.Size = new System.Drawing.Size(75, 20);
            this.discardButton.TabIndex = 8;
            this.discardButton.Text = "&Discard";
            this.discardButton.UseVisualStyleBackColor = true;
            // 
            // transactionDatePicker
            // 
            this.transactionDatePicker.CustomFormat = "";
            this.transactionDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.transactionDatePicker.Location = new System.Drawing.Point(99, 3);
            this.transactionDatePicker.Name = "transactionDatePicker";
            this.transactionDatePicker.Size = new System.Drawing.Size(117, 20);
            this.transactionDatePicker.TabIndex = 1;
            this.transactionDatePicker.ValueChanged += new System.EventHandler(this.TransactionDatePicker_ValueChanged);
            // 
            // depositTextBox
            // 
            this.depositTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.depositTextBox.Location = new System.Drawing.Point(416, 3);
            this.depositTextBox.Name = "depositTextBox";
            this.depositTextBox.Size = new System.Drawing.Size(72, 20);
            this.depositTextBox.TabIndex = 3;
            this.depositTextBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.depositTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.AmountBox_Validating);
            this.depositTextBox.Validated += new System.EventHandler(this.AmountBox_Validated);
            // 
            // withdrawalTextBox
            // 
            this.withdrawalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.withdrawalTextBox.Location = new System.Drawing.Point(494, 3);
            this.withdrawalTextBox.Name = "withdrawalTextBox";
            this.withdrawalTextBox.Size = new System.Drawing.Size(72, 20);
            this.withdrawalTextBox.TabIndex = 4;
            this.withdrawalTextBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.withdrawalTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.AmountBox_Validating);
            this.withdrawalTextBox.Validated += new System.EventHandler(this.AmountBox_Validated);
            // 
            // clearDatePicker
            // 
            this.clearDatePicker.CustomFormat = "";
            this.clearDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.clearDatePicker.Location = new System.Drawing.Point(99, 29);
            this.clearDatePicker.Name = "clearDatePicker";
            this.clearDatePicker.ShowCheckBox = true;
            this.clearDatePicker.Size = new System.Drawing.Size(117, 20);
            this.clearDatePicker.TabIndex = 7;
            this.clearDatePicker.ValueChanged += new System.EventHandler(this.ClearDatePicker_ValueChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // accountComboBox
            // 
            this.accountComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountComboBox.FormattingEnabled = true;
            this.accountComboBox.ItemHeight = 13;
            this.accountComboBox.Location = new System.Drawing.Point(222, 3);
            this.accountComboBox.Name = "accountComboBox";
            this.accountComboBox.Size = new System.Drawing.Size(188, 21);
            this.accountComboBox.TabIndex = 2;
            this.accountComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.AccountComboBox_Validating);
            this.accountComboBox.Validated += new System.EventHandler(this.AccountComboBox_Validated);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(222, 29);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(188, 20);
            this.descriptionTextBox.TabIndex = 9;
            // 
            // TransactionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.accountComboBox);
            this.Controls.Add(clearedLabel);
            this.Controls.Add(this.clearDatePicker);
            this.Controls.Add(this.withdrawalTextBox);
            this.Controls.Add(this.depositTextBox);
            this.Controls.Add(transactionDateLabel);
            this.Controls.Add(this.transactionDatePicker);
            this.Controls.Add(this.discardButton);
            this.Controls.Add(this.saveButton);
            this.Name = "TransactionEditor";
            this.Size = new System.Drawing.Size(650, 52);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button discardButton;
        private System.Windows.Forms.DateTimePicker transactionDatePicker;
        private System.Windows.Forms.TextBox depositTextBox;
        private System.Windows.Forms.TextBox withdrawalTextBox;
        private System.Windows.Forms.DateTimePicker clearDatePicker;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox accountComboBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
    }
}

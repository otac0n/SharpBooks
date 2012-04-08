namespace SharpBooks.UI
{
    partial class AccountRegister
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
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.splitsView = new SharpBooks.UI.SplitsView();
            this.headers = new SharpBooks.UI.HeaderControl();
            this.transactionEditor = new SharpBooks.UI.TransactionEditor();
            this.SuspendLayout();
            // 
            // vScroll
            // 
            this.vScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScroll.LargeChange = 1;
            this.vScroll.Location = new System.Drawing.Point(583, 0);
            this.vScroll.Maximum = 0;
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size(17, 248);
            this.vScroll.TabIndex = 1;
            this.vScroll.ValueChanged += new System.EventHandler(this.VScroll_ValueChanged);
            // 
            // splitsView
            // 
            this.splitsView.AlternatingBackColor = System.Drawing.Color.WhiteSmoke;
            this.splitsView.BackColor = System.Drawing.Color.Transparent;
            this.splitsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitsView.Location = new System.Drawing.Point(0, 25);
            this.splitsView.Name = "splitsView";
            this.splitsView.Offset = new System.Drawing.Point(0, 0);
            this.splitsView.Padding = new System.Windows.Forms.Padding(3);
            this.splitsView.SelectedIndex = -1;
            this.splitsView.Size = new System.Drawing.Size(583, 223);
            this.splitsView.TabIndex = 2;
            this.splitsView.ScrollableSizeChanged += new System.EventHandler<System.EventArgs>(this.Splits_ScrollableSizeChanged);
            this.splitsView.DesiresOffset += new System.EventHandler<SharpBooks.UI.DesiresOffsetEventArgs>(this.Splits_DesiresOffset);
            this.splitsView.SelectedIndexChanged += new System.EventHandler<System.EventArgs>(this.Splits_SelectedIndexChanged);
            this.splitsView.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Splits_MouseWheel);
            // 
            // headers
            // 
            this.headers.BackColor = System.Drawing.SystemColors.Control;
            this.headers.CellPadding = 3;
            this.headers.Dock = System.Windows.Forms.DockStyle.Top;
            this.headers.Location = new System.Drawing.Point(0, 0);
            this.headers.Name = "headers";
            this.headers.Size = new System.Drawing.Size(583, 25);
            this.headers.TabIndex = 0;
            this.headers.ColumnWidthChanged += new System.EventHandler<System.Windows.Forms.ColumnWidthChangedEventArgs>(this.Headers_ColumnWidthChanged);
            // 
            // transactionEditor
            // 
            this.transactionEditor.BackColor = System.Drawing.SystemColors.Control;
            this.transactionEditor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.transactionEditor.Location = new System.Drawing.Point(0, 248);
            this.transactionEditor.Name = "transactionEditor";
            this.transactionEditor.Size = new System.Drawing.Size(600, 52);
            this.transactionEditor.TabIndex = 3;
            this.transactionEditor.TransactionUpdated += new System.EventHandler<SharpBooks.UI.TransactionUpdatedEventArgs>(this.transactionEditor_TransactionUpdated);
            // 
            // AccountRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.splitsView);
            this.Controls.Add(this.headers);
            this.Controls.Add(this.vScroll);
            this.Controls.Add(this.transactionEditor);
            this.Name = "AccountRegister";
            this.Size = new System.Drawing.Size(600, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private HeaderControl headers;
        private System.Windows.Forms.VScrollBar vScroll;
        private SplitsView splitsView;
        private TransactionEditor transactionEditor;


    }
}

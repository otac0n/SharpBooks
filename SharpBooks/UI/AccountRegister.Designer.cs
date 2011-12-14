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
            this.headers = new SharpBooks.UI.HeaderControl();
            this.SuspendLayout();
            // 
            // vScroll
            // 
            this.vScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScroll.Location = new System.Drawing.Point(133, 0);
            this.vScroll.Maximum = 100000;
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size(17, 150);
            this.vScroll.TabIndex = 1;
            this.vScroll.ValueChanged += new System.EventHandler(this.VScroll_ValueChanged);
            // 
            // headers
            // 
            this.headers.BackColor = System.Drawing.SystemColors.Control;
            this.headers.CellPadding = 3;
            this.headers.Dock = System.Windows.Forms.DockStyle.Top;
            this.headers.Location = new System.Drawing.Point(0, 0);
            this.headers.Name = "headers";
            this.headers.Size = new System.Drawing.Size(133, 25);
            this.headers.TabIndex = 0;
            this.headers.ColumnWidthChanged += new System.EventHandler<System.Windows.Forms.ColumnWidthChangedEventArgs>(this.Headers_ColumnWidthChanged);
            // 
            // AccountRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.headers);
            this.Controls.Add(this.vScroll);
            this.DoubleBuffered = true;
            this.Name = "AccountRegister";
            this.ResumeLayout(false);

        }

        #endregion

        private HeaderControl headers;
        private System.Windows.Forms.VScrollBar vScroll;


    }
}

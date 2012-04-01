namespace SharpBooks.UI
{
    partial class AccountTree
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
            System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem newAccountToolStripMenuItem;
            this.tree = new System.Windows.Forms.TreeView();
            this.nodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += new System.EventHandler(this.open_Click);
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tree.FullRowSelect = true;
            this.tree.HideSelection = false;
            this.tree.ItemHeight = 24;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.ShowLines = false;
            this.tree.Size = new System.Drawing.Size(150, 150);
            this.tree.TabIndex = 0;
            this.tree.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tree_DrawNode);
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseClick);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
            // 
            // nodeContextMenu
            // 
            this.nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            openToolStripMenuItem,
            newAccountToolStripMenuItem});
            this.nodeContextMenu.Name = "nodeContextMenu";
            this.nodeContextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // newAccountToolStripMenuItem
            // 
            newAccountToolStripMenuItem.Name = "newAccountToolStripMenuItem";
            newAccountToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            newAccountToolStripMenuItem.Text = "New Account";
            newAccountToolStripMenuItem.Click += new System.EventHandler(this.newAccount_Click);
            // 
            // AccountTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tree);
            this.Name = "AccountTree";
            this.nodeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ContextMenuStrip nodeContextMenu;
    }
}

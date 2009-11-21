namespace SharpCash.Debug
{
    partial class ChartView
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
            this.Chart = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // Chart
            // 
            this.Chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart.IsAntiAlias = true;
            this.Chart.Location = new System.Drawing.Point(0, 0);
            this.Chart.Name = "Chart";
            this.Chart.ScrollGrace = 0;
            this.Chart.ScrollMaxX = 0;
            this.Chart.ScrollMaxY = 0;
            this.Chart.ScrollMaxY2 = 0;
            this.Chart.ScrollMinX = 0;
            this.Chart.ScrollMinY = 0;
            this.Chart.ScrollMinY2 = 0;
            this.Chart.Size = new System.Drawing.Size(838, 493);
            this.Chart.TabIndex = 0;
            // 
            // ChartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 493);
            this.Controls.Add(this.Chart);
            this.Name = "ChartView";
            this.Text = "ChartView";
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl Chart;
    }
}
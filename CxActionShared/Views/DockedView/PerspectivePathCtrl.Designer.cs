namespace CxViewerAction2022.Views.DockedView
{
    partial class PerspectivePathCtrl
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
            this.pnlPath = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // pnlPath
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(200, 400);
            this.pnlPath.ColumnCount = 1;
            this.pnlPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPath.Location = new System.Drawing.Point(0, 0);
            this.pnlPath.Name = "pnlPath";
            this.pnlPath.RowCount = 1;
            this.pnlPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 303F));
            this.pnlPath.Size = new System.Drawing.Size(390, 303);
            this.pnlPath.TabIndex = 0;
            // 
            // PerspectivePathCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlPath);
            this.Name = "PerspectivePathCtrl";
            this.Size = new System.Drawing.Size(390, 303);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlPath;

    }
}

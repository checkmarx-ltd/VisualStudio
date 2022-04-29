namespace CxViewerAction.Views.DockedView
{
    partial class ScanProcessCtrl
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
            this.progressTotal = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentIterationProgress = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressTotal
            // 
            this.progressTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTotal.Location = new System.Drawing.Point(3, 20);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(492, 23);
            this.progressTotal.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Scan progress:";
            // 
            // lblCurrentIterationProgress
            // 
            this.lblCurrentIterationProgress.AutoSize = true;
            this.lblCurrentIterationProgress.Location = new System.Drawing.Point(76, 46);
            this.lblCurrentIterationProgress.Name = "lblCurrentIterationProgress";
            this.lblCurrentIterationProgress.Size = new System.Drawing.Size(84, 13);
            this.lblCurrentIterationProgress.TabIndex = 4;
            this.lblCurrentIterationProgress.Text = "Waiting server...";
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(3, 4);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(84, 13);
            this.lblProjectName.TabIndex = 6;
            this.lblProjectName.Text = "Waiting server...";
            // 
            // ScanProcessCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblCurrentIterationProgress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressTotal);
            this.Name = "ScanProcessCtrl";
            this.Size = new System.Drawing.Size(498, 74);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentIterationProgress;
        private System.Windows.Forms.Label lblProjectName;
    }
}

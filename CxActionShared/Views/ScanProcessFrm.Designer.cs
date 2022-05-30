namespace CxViewerAction2022.Views
{
    partial class ScanProcessFrm
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
            this.label1 = new System.Windows.Forms.Label();
            this.progressMargue1 = new System.Windows.Forms.ProgressBar();
            this.lblCurrentIterationName = new System.Windows.Forms.Label();
            this.lblCurrentIterationProgress = new System.Windows.Forms.Label();
            this.chBackground = new System.Windows.Forms.CheckBox();
            this.btnRunBackground = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDetails = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.progressTotal = new System.Windows.Forms.ProgressBar();
            this.lbl = new System.Windows.Forms.Label();
            this.lblTotalScanProgress = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.progressMargue2 = new System.Windows.Forms.ProgressBar();
            this.lblProgressTotalTop = new System.Windows.Forms.Label();
            this.pnlDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Operation in progress...";
            // 
            // progressMargue1
            // 
            this.progressMargue1.Location = new System.Drawing.Point(13, 29);
            this.progressMargue1.Name = "progressMargue1";
            this.progressMargue1.Size = new System.Drawing.Size(469, 23);
            this.progressMargue1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressMargue1.TabIndex = 1;
            // 
            // lblCurrentIterationName
            // 
            this.lblCurrentIterationName.AutoSize = true;
            this.lblCurrentIterationName.Location = new System.Drawing.Point(10, 129);
            this.lblCurrentIterationName.Name = "lblCurrentIterationName";
            this.lblCurrentIterationName.Size = new System.Drawing.Size(84, 13);
            this.lblCurrentIterationName.TabIndex = 2;
            this.lblCurrentIterationName.Text = "Waiting server...";
            // 
            // lblCurrentIterationProgress
            // 
            this.lblCurrentIterationProgress.AutoSize = true;
            this.lblCurrentIterationProgress.Location = new System.Drawing.Point(10, 142);
            this.lblCurrentIterationProgress.Name = "lblCurrentIterationProgress";
            this.lblCurrentIterationProgress.Size = new System.Drawing.Size(84, 13);
            this.lblCurrentIterationProgress.TabIndex = 3;
            this.lblCurrentIterationProgress.Text = "Waiting server...";
            // 
            // chBackground
            // 
            this.chBackground.AutoSize = true;
            this.chBackground.Location = new System.Drawing.Point(12, 95);
            this.chBackground.Name = "chBackground";
            this.chBackground.Size = new System.Drawing.Size(148, 17);
            this.chBackground.TabIndex = 4;
            this.chBackground.Text = "Always run in background";
            this.chBackground.UseVisualStyleBackColor = true;
            // 
            // btnRunBackground
            // 
            this.btnRunBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunBackground.Location = new System.Drawing.Point(181, 137);
            this.btnRunBackground.Name = "btnRunBackground";
            this.btnRunBackground.Size = new System.Drawing.Size(140, 23);
            this.btnRunBackground.TabIndex = 5;
            this.btnRunBackground.Text = "Run in Background";
            this.btnRunBackground.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 137);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDetails
            // 
            this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetails.Location = new System.Drawing.Point(408, 137);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(75, 23);
            this.btnDetails.TabIndex = 7;
            this.btnDetails.Text = "Details  >>";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // pnlDetails
            // 
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.progressTotal);
            this.pnlDetails.Controls.Add(this.lbl);
            this.pnlDetails.Controls.Add(this.lblTotalScanProgress);
            this.pnlDetails.Controls.Add(this.lblProjectName);
            this.pnlDetails.Controls.Add(this.progressMargue2);
            this.pnlDetails.Controls.Add(this.lblCurrentIterationProgress);
            this.pnlDetails.Controls.Add(this.lblCurrentIterationName);
            this.pnlDetails.Location = new System.Drawing.Point(13, 118);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(470, 0);
            this.pnlDetails.TabIndex = 8;
            this.pnlDetails.Visible = false;
            // 
            // progressTotal
            // 
            this.progressTotal.Location = new System.Drawing.Point(13, 103);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(439, 23);
            this.progressTotal.TabIndex = 4;
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(10, 87);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(75, 13);
            this.lbl.TabIndex = 3;
            this.lbl.Text = "Scan progress";
            // 
            // lblTotalScanProgress
            // 
            this.lblTotalScanProgress.AutoSize = true;
            this.lblTotalScanProgress.Location = new System.Drawing.Point(10, 54);
            this.lblTotalScanProgress.Name = "lblTotalScanProgress";
            this.lblTotalScanProgress.Size = new System.Drawing.Size(22, 13);
            this.lblTotalScanProgress.TabIndex = 2;
            this.lblTotalScanProgress.Text = ". . .";
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(10, 12);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(22, 13);
            this.lblProjectName.TabIndex = 1;
            this.lblProjectName.Text = ". . .";
            // 
            // progressMargue2
            // 
            this.progressMargue2.Location = new System.Drawing.Point(13, 28);
            this.progressMargue2.Name = "progressMargue2";
            this.progressMargue2.Size = new System.Drawing.Size(439, 23);
            this.progressMargue2.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressMargue2.TabIndex = 0;
            // 
            // lblProgressTotalTop
            // 
            this.lblProgressTotalTop.AutoSize = true;
            this.lblProgressTotalTop.Location = new System.Drawing.Point(13, 59);
            this.lblProgressTotalTop.Name = "lblProgressTotalTop";
            this.lblProgressTotalTop.Size = new System.Drawing.Size(84, 13);
            this.lblProgressTotalTop.TabIndex = 9;
            this.lblProgressTotalTop.Text = "Waiting server...";
            // 
            // ScanProcessFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 172);
            this.ControlBox = false;
            this.Controls.Add(this.lblProgressTotalTop);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRunBackground);
            this.Controls.Add(this.chBackground);
            this.Controls.Add(this.progressMargue1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanProcessFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scanning project";
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressMargue1;
        private System.Windows.Forms.Label lblCurrentIterationName;
        private System.Windows.Forms.Label lblCurrentIterationProgress;
        private System.Windows.Forms.CheckBox chBackground;
        private System.Windows.Forms.Button btnRunBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.ProgressBar progressMargue2;
        private System.Windows.Forms.ProgressBar progressTotal;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Label lblTotalScanProgress;
        private System.Windows.Forms.Label lblProgressTotalTop;
    }
}
namespace CxViewerAction2022.Views.DockedView
{
	partial class ConnectionCtrl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTimeoutInterval = new System.Windows.Forms.TextBox();
            this.txtReconnectCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtUpdateStatusInterval = new System.Windows.Forms.TextBox();
            this.checkInBackground = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.disableConnectionOptimizationsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtTimeoutInterval);
            this.groupBox1.Controls.Add(this.txtReconnectCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 87);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reconnection settings";
            // 
            // txtTimeoutInterval
            // 
            this.txtTimeoutInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTimeoutInterval.Location = new System.Drawing.Point(236, 22);
            this.txtTimeoutInterval.MaxLength = 3;
            this.txtTimeoutInterval.Name = "txtTimeoutInterval";
            this.txtTimeoutInterval.Size = new System.Drawing.Size(33, 20);
            this.txtTimeoutInterval.TabIndex = 2;
            this.txtTimeoutInterval.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimeoutInterval_KeyDown);
            // 
            // txtReconnectCount
            // 
            this.txtReconnectCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReconnectCount.Location = new System.Drawing.Point(236, 53);
            this.txtReconnectCount.MaxLength = 3;
            this.txtReconnectCount.Name = "txtReconnectCount";
            this.txtReconnectCount.Size = new System.Drawing.Size(33, 20);
            this.txtReconnectCount.TabIndex = 3;
            this.txtReconnectCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimeoutInterval_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Reconnect count:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reconnect timeout  (seconds):";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtUpdateStatusInterval);
            this.groupBox2.Controls.Add(this.checkInBackground);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(0, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(457, 76);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scan settings";
            // 
            // txtUpdateStatusInterval
            // 
            this.txtUpdateStatusInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdateStatusInterval.Location = new System.Drawing.Point(236, 23);
            this.txtUpdateStatusInterval.MaxLength = 3;
            this.txtUpdateStatusInterval.Name = "txtUpdateStatusInterval";
            this.txtUpdateStatusInterval.Size = new System.Drawing.Size(33, 20);
            this.txtUpdateStatusInterval.TabIndex = 4;
            this.txtUpdateStatusInterval.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimeoutInterval_KeyDown);
            // 
            // checkInBackground
            // 
            this.checkInBackground.AutoSize = true;
            this.checkInBackground.Location = new System.Drawing.Point(236, 55);
            this.checkInBackground.Name = "checkInBackground";
            this.checkInBackground.Size = new System.Drawing.Size(148, 17);
            this.checkInBackground.TabIndex = 7;
            this.checkInBackground.Text = "Always run in background";
            this.checkInBackground.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Scan progress:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Update scan status interval (seconds):";
            // 
            // btnRestore
            // 
            this.btnRestore.CausesValidation = false;
            this.btnRestore.Location = new System.Drawing.Point(0, 211);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(108, 23);
            this.btnRestore.TabIndex = 14;
            this.btnRestore.Text = "Restore Defaults";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(117, 211);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Apply";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // disableConnectionOptimizationsCheckBox
            // 
            this.disableConnectionOptimizationsCheckBox.AutoSize = true;
            this.disableConnectionOptimizationsCheckBox.Location = new System.Drawing.Point(3, 178);
            this.disableConnectionOptimizationsCheckBox.Name = "disableConnectionOptimizationsCheckBox";
            this.disableConnectionOptimizationsCheckBox.Size = new System.Drawing.Size(180, 17);
            this.disableConnectionOptimizationsCheckBox.TabIndex = 0;
            this.disableConnectionOptimizationsCheckBox.Text = "Disable connection optimizations";
            this.disableConnectionOptimizationsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConnectionCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.disableConnectionOptimizationsCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnSave);
            this.Name = "ConnectionCtrl";
            this.Size = new System.Drawing.Size(460, 263);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtReconnectCount;
		private System.Windows.Forms.TextBox txtTimeoutInterval;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtUpdateStatusInterval;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkInBackground;
		private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox disableConnectionOptimizationsCheckBox;
	}
}

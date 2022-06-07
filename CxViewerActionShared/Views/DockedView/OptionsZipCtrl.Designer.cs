namespace CxViewerAction2022.Views.DockedView
{
    partial class OptionsZipCtrl
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtExcludeFileExt = new System.Windows.Forms.TextBox();
            this.txtExcludeFolder = new System.Windows.Forms.TextBox();
            this.txtZipMazSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtExcludeFileExt);
            this.groupBox2.Controls.Add(this.txtExcludeFolder);
            this.groupBox2.Controls.Add(this.txtZipMazSize);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(0, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 251);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pack sources";
            // 
            // txtExcludeFileExt
            // 
            this.txtExcludeFileExt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExcludeFileExt.Location = new System.Drawing.Point(22, 41);
            this.txtExcludeFileExt.Multiline = true;
            this.txtExcludeFileExt.Name = "txtExcludeFileExt";
            this.txtExcludeFileExt.Size = new System.Drawing.Size(352, 73);
            this.txtExcludeFileExt.TabIndex = 1;
            // 
            // txtExcludeFolder
            // 
            this.txtExcludeFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExcludeFolder.Location = new System.Drawing.Point(22, 133);
            this.txtExcludeFolder.Multiline = true;
            this.txtExcludeFolder.Name = "txtExcludeFolder";
            this.txtExcludeFolder.Size = new System.Drawing.Size(352, 77);
            this.txtExcludeFolder.TabIndex = 2;
            // 
            // txtZipMazSize
            // 
            this.txtZipMazSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZipMazSize.Location = new System.Drawing.Point(159, 216);
            this.txtZipMazSize.Name = "txtZipMazSize";
            this.txtZipMazSize.Size = new System.Drawing.Size(59, 20);
            this.txtZipMazSize.TabIndex = 3;
            this.txtZipMazSize.Validating += new System.ComponentModel.CancelEventHandler(this.txtZipMazSize_Validating);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 219);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Max ZIP file size (MB)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Exclude folders: (comma sep.)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Exclude files: (comma sep.)";
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(0, 258);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Apply";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // OptionsZipCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSave);
            this.Name = "OptionsZipCtrl";
            this.Size = new System.Drawing.Size(460, 284);
            this.VisibleChanged += new System.EventHandler(this.OptionsZipCtrl_VisibleChanged);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtExcludeFolder;
        private System.Windows.Forms.TextBox txtExcludeFileExt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox txtZipMazSize;
        private System.Windows.Forms.Label label8;
    }
}

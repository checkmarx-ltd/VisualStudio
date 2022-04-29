using System;

namespace CxViewerAction.Views.DockedView
{
    partial class OptionsAuthCtrl
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
        [STAThread]
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.groupAuth = new System.Windows.Forms.GroupBox();
			this.logoutBtn = new System.Windows.Forms.Button();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.loginBtn = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.applyButton = new System.Windows.Forms.Button();
            this.lblNote = new System.Windows.Forms.Label();
            this.groupAuth.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// groupAuth
			// 
			this.groupAuth.AutoSize = true;
			this.groupAuth.Controls.Add(this.logoutBtn);
			this.groupAuth.Controls.Add(this.txtServer);
			this.groupAuth.Controls.Add(this.loginBtn);
			this.groupAuth.Controls.Add(this.label1);
			this.groupAuth.Location = new System.Drawing.Point(3, 3);
			this.groupAuth.Name = "groupAuth";
			this.groupAuth.Size = new System.Drawing.Size(474, 179);
			this.groupAuth.TabIndex = 0;
			this.groupAuth.TabStop = false;
			this.groupAuth.Text = "Authentication";
			// 
			// logoutBtn
			// 
			this.logoutBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.logoutBtn.Enabled = false;
			this.logoutBtn.Location = new System.Drawing.Point(121, 95);
			this.logoutBtn.Name = "logoutBtn";
			this.logoutBtn.Size = new System.Drawing.Size(218, 23);
			this.logoutBtn.TabIndex = 2;
			this.logoutBtn.Text = "Logout";
			this.logoutBtn.UseVisualStyleBackColor = true;
			this.logoutBtn.Click += new System.EventHandler(this.logoutBtn_Click);
			// 
			// txtServer
			// 
			this.txtServer.Location = new System.Drawing.Point(121, 17);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(218, 20);
			this.txtServer.TabIndex = 1;
			// 
			// loginBtn
			// 
			this.loginBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.loginBtn.Location = new System.Drawing.Point(121, 55);
			this.loginBtn.Name = "loginBtn";
			this.loginBtn.Size = new System.Drawing.Size(218, 23);
			this.loginBtn.TabIndex = 1;
			this.loginBtn.Text = "Login";
			this.loginBtn.UseVisualStyleBackColor = true;
			this.loginBtn.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Server:";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(7, 225);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(41, 13);
            this.lblNote.TabIndex = 0;
            this.lblNote.Text = "Note that for Visual Studio 2019, uncheck the\n'Optimize rendering for screens with different pixel densities (requires restart)'\ncheckbox located under: Tools > Options > Environment > General,\nin order to optimize visualization.";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
			// 
			// applyButton
			// 
			this.applyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.applyButton.Location = new System.Drawing.Point(13, 188);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(150, 22);
			this.applyButton.TabIndex = 3;
			this.applyButton.Text = "Apply";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// OptionsAuthCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.groupAuth);
            this.Controls.Add(this.lblNote);
            this.Name = "OptionsAuthCtrl";
			this.Size = new System.Drawing.Size(598, 250);
			this.VisibleChanged += new System.EventHandler(this.OptionsAuthCtrl_VisibleChanged);
			this.groupAuth.ResumeLayout(false);
			this.groupAuth.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupAuth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Button logoutBtn;
		private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label lblNote;
    }
}

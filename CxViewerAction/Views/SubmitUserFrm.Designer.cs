
namespace CxViewerAction.Views
{
    partial class SubmitUserFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubmitUserFrm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnsubmituser = new System.Windows.Forms.Button();
            this.txtInvUserName = new System.Windows.Forms.TextBox();
            this.txtInvPassword = new System.Windows.Forms.TextBox();
            this.lbInvlPassword = new System.Windows.Forms.Label();
            this.lblInvUserName = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnsubmituser);
            this.groupBox1.Controls.Add(this.txtInvUserName);
            this.groupBox1.Controls.Add(this.txtInvPassword);
            this.groupBox1.Controls.Add(this.lbInvlPassword);
            this.groupBox1.Controls.Add(this.lblInvUserName);
            this.groupBox1.Location = new System.Drawing.Point(96, 34);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(521, 215);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Submit login details";
            // 
            // btnsubmituser
            // 
            this.btnsubmituser.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnsubmituser.Location = new System.Drawing.Point(209, 145);
            this.btnsubmituser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnsubmituser.Name = "btnsubmituser";
            this.btnsubmituser.Size = new System.Drawing.Size(145, 41);
            this.btnsubmituser.TabIndex = 3;
            this.btnsubmituser.Text = "Submit";
            this.btnsubmituser.UseVisualStyleBackColor = true;
            this.btnsubmituser.Click += new System.EventHandler(this.btnsubmituser_Click);
            // 
            // txtInvUserName
            // 
            this.txtInvUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInvUserName.Location = new System.Drawing.Point(195, 38);
            this.txtInvUserName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInvUserName.MaxLength = 50;
            this.txtInvUserName.Name = "txtInvUserName";
            this.txtInvUserName.Size = new System.Drawing.Size(223, 26);
            this.txtInvUserName.TabIndex = 1;
            // 
            // txtInvPassword
            // 
            this.txtInvPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInvPassword.Location = new System.Drawing.Point(195, 96);
            this.txtInvPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInvPassword.MaxLength = 50;
            this.txtInvPassword.Name = "txtInvPassword";
            this.txtInvPassword.PasswordChar = '*';
            this.txtInvPassword.Size = new System.Drawing.Size(223, 26);
            this.txtInvPassword.TabIndex = 2;
            // 
            // lbInvlPassword
            // 
            this.lbInvlPassword.AutoSize = true;
            this.lbInvlPassword.Location = new System.Drawing.Point(77, 96);
            this.lbInvlPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbInvlPassword.Name = "lbInvlPassword";
            this.lbInvlPassword.Size = new System.Drawing.Size(78, 20);
            this.lbInvlPassword.TabIndex = 1;
            this.lbInvlPassword.Text = "Password";
            // 
            // lblInvUserName
            // 
            this.lblInvUserName.AutoSize = true;
            this.lblInvUserName.Location = new System.Drawing.Point(77, 44);
            this.lblInvUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInvUserName.Name = "lblInvUserName";
            this.lblInvUserName.Size = new System.Drawing.Size(89, 20);
            this.lblInvUserName.TabIndex = 0;
            this.lblInvUserName.Text = "User Name";
            // 
            // SubmitUserFrm
            // 
            this.AcceptButton = this.btnsubmituser;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 263);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubmitUserFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnsubmituser;
        private System.Windows.Forms.TextBox txtInvUserName;
        private System.Windows.Forms.TextBox txtInvPassword;
        private System.Windows.Forms.Label lbInvlPassword;
        private System.Windows.Forms.Label lblInvUserName;
    }
}
using CxViewerAction.Views.DockedView;

namespace CxViewerAction.Views
{
    partial class OidcLoginFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OidcLoginFrm));
            this.oidcLoginCtrl2 = new CxViewerAction.Views.DockedView.OidcLoginCtrl();
            this.SuspendLayout();
            // 
            // oidcLoginCtrl2
            // 
            this.oidcLoginCtrl2.AutoScroll = true;
            this.oidcLoginCtrl2.Location = new System.Drawing.Point(-1, -2);
            this.oidcLoginCtrl2.Name = "oidcLoginCtrl2";
            this.oidcLoginCtrl2.Size = new System.Drawing.Size(495, 574);
            this.oidcLoginCtrl2.TabIndex = 0;
            // 
            // OidcLoginFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 573);
            this.Controls.Add(this.oidcLoginCtrl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OidcLoginFrm";
            this.Text = "Login";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SamlLoginFrm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

		//private DockedView.OidcLoginCtrl oidcLoginCtrl1;
        private DockedView.OidcLoginCtrl oidcLoginCtrl2;

        public OidcLoginCtrl OidcLoginCtrl2 { get => oidcLoginCtrl2; set => oidcLoginCtrl2 = value; }
	}
}
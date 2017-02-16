using System;

namespace CxViewerAction.Views.DockedView
{
    partial class SamlLoginCtrl
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
            this.webBrowserIdentityProvider = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserIdentityProvider
            // 
            this.webBrowserIdentityProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserIdentityProvider.Location = new System.Drawing.Point(0, 0);
            this.webBrowserIdentityProvider.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserIdentityProvider.Name = "webBrowserIdentityProvider";
            this.webBrowserIdentityProvider.Size = new System.Drawing.Size(388, 572);
            this.webBrowserIdentityProvider.TabIndex = 0;
            // 
            // SamlLoginCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webBrowserIdentityProvider);
            this.Name = "SamlLoginCtrl";
            this.Size = new System.Drawing.Size(388, 572);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserIdentityProvider;
        public System.Windows.Forms.WebBrowser WebBrowserIdentityProvider
        {
            get
            {
                return webBrowserIdentityProvider;
            }
            set
            {
                webBrowserIdentityProvider = value;
            }
        }
    }
}

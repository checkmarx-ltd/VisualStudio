using System;
using Microsoft.Web;
using Microsoft.Web.WebView2.Core;

namespace CxViewerAction.Views.DockedView
{
    partial class OidcLoginCtrl
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
            this.loginWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.SuspendLayout();
            // 
            // loginWebView
            // 
            this.loginWebView.CreationProperties = null;
            this.loginWebView.Location = new System.Drawing.Point(0, 3);
            this.loginWebView.Name = "loginWebView";
            this.loginWebView.Size = new System.Drawing.Size(659, 607);
            this.loginWebView.TabIndex = 0;
            this.loginWebView.Text = "webView21";
            this.loginWebView.ZoomFactor = 1D;
            // 
            // OidcLoginCtrl
            // 
            this.Controls.Add(this.loginWebView);
            this.Name = "OidcLoginCtrl";
            this.Size = new System.Drawing.Size(659, 613);
            this.ResumeLayout(false);

        }


        #endregion        
        private Microsoft.Web.WebView2.WinForms.WebView2 loginWebView;
    }
}

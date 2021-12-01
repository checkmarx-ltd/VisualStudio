using CxViewerAction.Views.DockedView;
using Microsoft.Web.WebView2.Core;

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
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.SuspendLayout();            
            // 
            // webView21
            // 
            InitializeAsync();
            this.webView21.Location = new System.Drawing.Point(1, -2);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(1011, 706);
            this.webView21.TabIndex = 0;
            //this.webView21.Text = "webView21";
            this.webView21.Source = new System.Uri("about:blank");
            //this.webView21.Source = new System.Uri("http://www.google.com", System.UriKind.Absolute);
            this.webView21.ZoomFactor = 1D;
            // 
            // OidcLoginFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 699);
            this.Controls.Add(this.webView21);
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

        private async void InitializeAsync()
        {
            var env = await CoreWebView2Environment.CreateAsync(null, "C:\\temp");
            await webView21.EnsureCoreWebView2Async(env);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
    }
}
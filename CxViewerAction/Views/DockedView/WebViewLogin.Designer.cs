
namespace CxViewerAction.Views.DockedView
{
    partial class WebViewLogin
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
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.SuspendLayout();
            // 
            // webView
            // 
            this.webView.CreationProperties = null;
            this.webView.Location = new System.Drawing.Point(-3, -1);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(803, 455);
            this.webView.TabIndex = 0;
            //this.webView.Text = "webView21";
            this.webView.ZoomFactor = 1D;
            this.webView.Click += new System.EventHandler(this.webView21_Click);
            // 
            // WebViewLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webView);
            this.Name = "WebViewLogin";
            this.Text = "WebViewLogin";
            this.Load += new System.EventHandler(this.WebViewLogin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
    }
}
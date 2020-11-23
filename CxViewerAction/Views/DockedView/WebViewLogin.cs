using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CxViewerAction.Views.DockedView
{
    public partial class WebViewLogin : Form
    {
        public WebViewLogin()
        {
            InitializeComponent();            
            this.Resize += new System.EventHandler(this.Form_Resize);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            //webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            //goButton.Left = this.ClientSize.Width - goButton.Width;
            //addressBar.Width = goButton.Left - addressBar.Left;
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void WebViewLogin_Load(object sender, EventArgs e)
        {

        }
    }
}

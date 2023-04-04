using System;
using System.Windows.Forms;
using Common;
using CefSharp.WinForms;
using static CxActionShared.CommonCustomResourceRequestHandler;

namespace CxViewerAction.Views
{
    public partial class QueryDescriptionForm : Form
    {
        public string DescriptionUrl;
        public string _token;
        public QueryDescriptionForm(string descriptionUrl, string token)
        {
            DescriptionUrl = descriptionUrl;
            _token = token;
            InitializeComponent();
        }

        ChromiumWebBrowser browser;

        private void QueryDescriptionForm_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser();
            this.pContainer.Controls.Add(browser);
            Logger.Create().Info("QueryDescriptionForm() " + browser);
            browser.Dock = DockStyle.Fill;
            Logger.Create().Info("In browser form load calling address changed event.");
            browser.LoadUrl(DescriptionUrl);
            Logger.Create().Info("On browser form load calling load end event.");
            browser.RequestHandler = new NewCustomRequestHandler(_token);
        }

        private void QueryDescription_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
        }
    }
}


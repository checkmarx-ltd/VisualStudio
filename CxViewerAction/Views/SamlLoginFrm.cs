using System;
using System.Windows.Forms;
using CxViewerAction.Helpers;

namespace CxViewerAction.Views
{
    public partial class SamlLoginFrm : Form
    {
        public event EventHandler UserClosedForm;
        private const string SAML_LOGIN_RELATIVE_PATH = "/cxrestapi/auth/samlLogin";

        public SamlLoginFrm()
        {
            InitializeComponent();
        }

        public void ConnectToIdentidyProvider(string baseServerUri)
        {
            var serverUri = new Uri(baseServerUri + SAML_LOGIN_RELATIVE_PATH);
            samlLoginCtrl.ConnectToIdentidyProvider(serverUri);
        }

        public void CloseForm()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(CloseForm));
                return;
            }

            Hide();
        }

        private void SamlLoginFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            CloseForm();
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (UserClosedForm != null)
                {
                    UserClosedForm(this, new EventArgs());
                }
            }
        }
    }
}
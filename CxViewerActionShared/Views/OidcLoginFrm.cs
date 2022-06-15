using System;
using System.Windows.Forms;
using System.Reflection;
using Common;

namespace CxViewerAction2022.Views
{
    public partial class OidcLoginFrm : Form
    {
        public event EventHandler UserClosedForm;

        public OidcLoginFrm()
        {
            InitializeComponent();
		}

        public void ConnectToIdentidyProvider(string baseServerUri)
        {
            Logger.Create().Debug("Connecting with identity provider with url:" + baseServerUri);
            OidcLoginCtrl2.ConnectToIdentidyProvider(baseServerUri);
            Logger.Create().Debug("Connected by identity provider" );
        }

        public void CloseForm()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(CloseForm));
                Logger.Create().Debug("form closed");
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
                    Logger.Create().Debug("Saml login form closed");
                }
            }
        }
    }
}
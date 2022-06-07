using System;
using System.Windows.Forms;
using System.Reflection;

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
            OidcLoginCtrl2.ConnectToIdentidyProvider(baseServerUri);
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
using Common;
using CxViewerAction.Entities;
using CxViewerAction.Services;
using System;
using System.Net;
using System.Windows.Forms;

namespace CxViewerAction.Views
{
    /// <summary>
    /// SubmitUserFrm
    /// </summary>
    public partial class SubmitUserFrm : Form
    {
        public event EventHandler UserClosedForm;
        /// <summary>
        /// SubmitUserFrm
        /// </summary>
        public SubmitUserFrm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// CloseForm
        /// </summary>
        public void CloseForm()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(CloseForm));
                return;
            }

            Hide();
        }
        private static CxRESTApi cxRestApi = null;

        static LoginData loginCache = null;

        private static string server;
        /// <summary>
        /// Close dialog
        /// </summary>
        public void CloseView()
        {
            this.Close();
        }

        /// <summary>
        /// Show modal dialog
        /// </summary>
        /// <returns></returns>
        public virtual DialogResult ShowModalView()
        {
            return this.ShowDialog();
        }

        /// <summary>
        /// Gets or sets User Name
        /// </summary>
        public string UserName
        {
            get { return txtInvUserName.Text; }
            set { txtInvUserName.Text = value; }
        }

        /// <summary>
        /// Gets or sets User Name
        /// </summary>
        public string Password
        {
            get { return txtInvPassword.Text; }
            set { txtInvPassword.Text = value; }
        }
        /// <summary>
        /// Show modal dialog
        /// </summary>
        /// <param name="parent">Parent view container</param>
        /// <returns></returns>
        public virtual DialogResult ShowModalView(IView parent)
        {
            return this.ShowDialog((IWin32Window)parent);
        }

        /// <summary>
        /// Show non modal dialog
        /// </summary>
        public void ShowView()
        {
            this.Show();
        }

        /// <summary>
        /// Show non modal dialog
        /// </summary>
        /// <param name="parent">Parent view container</param>
        public void ShowView(IView parent)
        {
            this.Show((IWin32Window)parent);
        }
        private void btnsubmituser_Click(object sender, EventArgs e)
        {
            LoginData login;

            login = LoadSaved();
            string username = txtInvUserName.Text;
            string password = txtInvPassword.Text;


            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                string accessToken = null;
                try
                {
                    cxRestApi = new CxRESTApi(login);
                    accessToken = cxRestApi.LoginUserNamePassword(username, password);
                    cxRestApi.GetPermissions(accessToken);
                }
                catch (WebException ex)
                {
                    Logger.Create().Error(ex.Message, ex);
                    this.DialogResult = DialogResult.Cancel;
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.Message, ex);
                    this.DialogResult = DialogResult.Cancel;
                }
                finally
                {
                }

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                }

            }

            else
            {
                MessageBox.Show("Please enter valid credentials.");
                this.DialogResult = DialogResult.Retry;
                // txtInvUserName.Clear();               
                // txtInvPassword.Clear();
                // txtInvUserName.Focus();
                // this. = false;
                // this.CloseForm();
                //// SubmitUserFrm submitUserFrm = new SubmitUserFrm();
                // this.ShowModalView();
                // //this.DialogResult = DialogResult.Retry;
                //// e.Cancel = (window == DialogResult.No);
            }

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
        /// <summary>
        /// Load stored user data
        /// </summary>
        /// <returns></returns>
        internal static LoginData LoadSaved()
        {
            if (loginCache != null)
            {
                return loginCache;
            }
            LoginData login = Helpers.LoginHelper.Load(0);
            server = login.Server;
            loginCache = login;
            return login;
        }
    }
}

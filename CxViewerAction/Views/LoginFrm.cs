using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.Entities;

namespace CxViewerAction.Views
{
    /// <summary>
    /// Represent login form object
    /// </summary>
    public partial class LoginFrm : Form, ILoginView
    {
        #region [Private]

        private EntityId _entityId;
        private bool _ssl = false;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Get or set entity identifier
        /// </summary>
        public EntityId EntityId { get { return _entityId; } set { _entityId = value; } }

        /// <summary>
        /// Get or set domain
        /// </summary>
        public string ServerDomain
        {
            get
            {
                string server = txtServer.Text.Trim();

                if (server.StartsWith("http://"))
                {
                    _ssl = false;
                }

                if (server.StartsWith("https://"))
                {
                    _ssl = true;
                }

                return server;
            }
            set { txtServer.Text = value; }
        }

        public bool Ssl
        {
            get { return _ssl; }
            set { _ssl = value; }
        }

        /// <summary>
        /// Get or set userName
        /// </summary>
        public string UserName
        {
            get { return txtUserName.Text.Trim(); }
            set { txtUserName.Text = value; }
        }

        /// <summary>
        /// Get or set password
        /// </summary>
        public string Password
        {
            get { return txtPassword.Text.Trim(); }
            set { txtPassword.Text = value; }
        }

        public bool SSO
        {
            get { return rbUseCurrentUser.Checked; }
            set
            {
                if (value)
                {
                    rbUseCurrentUser.Checked = true;
                }
                else
                {
                    rbUseCredentials.Checked = true;
                }
            }
        }

        public bool Saml
        {
            get { return RadioButtonSaml.Checked; }
            set
            {
                if (value)
                {
                    RadioButtonSaml.Checked = true;
                }
                else
                {
                    rbUseCredentials.Checked = true;
                }
            }
        }

        #endregion

        #region [ Event Handlers ]

        public LoginFrm()
        {
            InitializeComponent();
        }

        public void CloseView()
        {
            this.Close();
        }

        public virtual DialogResult ShowModalView()
        {
            return this.ShowDialog();
        }

        public virtual DialogResult ShowModalView(IView parent)
        {
			this.BringToFront();
            return this.ShowDialog((IWin32Window)parent);
        }

        public void ShowView()
        {
            this.Show();
        }

        public void ShowView(IView parent)
        {
            this.Show((IWin32Window)parent);
        }

        #endregion

        /// <summary>
        /// Server name validation handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtServer_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtServer.Text)) //field empty
            {
                e.Cancel = true;
                validatorLoginForm.SetError((Control)sender, "Please enter server name");
            }
            else
                validatorLoginForm.SetError((Control)sender, string.Empty); // no errors
        }

        /// <summary>
        /// User name validation handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text)) //field empty
            {
                e.Cancel = true;
                validatorLoginForm.SetError((Control)sender, "Please enter user name"); // no errors
            }
            else
                validatorLoginForm.SetError((Control)sender, string.Empty);
        }

        /// <summary>
        /// Password validation handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                validatorLoginForm.SetError((Control)sender, "Please enter password");
            }
            else
                validatorLoginForm.SetError((Control)sender, string.Empty);
        }

        void CheckValidation()
        {
            txtServer_Validating(txtServer, new CancelEventArgs());
            txtUserName_Validating(txtUserName, new CancelEventArgs());
            txtPassword_Validating(txtPassword, new CancelEventArgs());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbUseCredentials.Checked)
            {
                CheckValidation();
                if (!string.IsNullOrEmpty(validatorLoginForm.GetError(txtServer)) ||
                !string.IsNullOrEmpty(validatorLoginForm.GetError(txtUserName)) ||
                !string.IsNullOrEmpty(validatorLoginForm.GetError(txtPassword))
                )
                {
                    this.DialogResult = DialogResult.None;
                }
            }
            else
            {
                txtServer_Validating(txtServer, new CancelEventArgs());
                if (!string.IsNullOrEmpty(validatorLoginForm.GetError(txtServer)))
                {
                    this.DialogResult = DialogResult.None;
                }
            }
        }

        private void rbUseCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUseCredentials.Checked)
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
            else
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                validatorLoginForm.SetError(txtUserName, "");
                validatorLoginForm.SetError(txtPassword, "");
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.Helpers;
using CxViewerAction.Entities;
using CxViewerAction.Entities.Enum;

namespace CxViewerAction.Views.DockedView
{
    public partial class OptionsCtrl : UserControl
    {
        private string _server = "";
        private string _userName = "";
        private string _password = "";

        public OptionsCtrl()
        {
            InitializeComponent();
            BindDataToView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServer.Text) || string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
                return;

            Login currentLogin = BindDataFromView();
            LoginHelper.Save(currentLogin);
        }

        private void BindDataToView()
        {
            Login currentLogin = LoginHelper.Load(0);

            txtServer.Text = currentLogin.ServerDomain;
            txtUserName.Text = currentLogin.UserName;
            txtPassword.Text = currentLogin.Password;
            checkInBackground.Checked = currentLogin.IsRunScanInBackground == CxViewerAction.Entities.Enum.SimpleDecision.Yes;
        }

        private Login BindDataFromView()
        {
            Login currentLogin = LoginHelper.Load(0);

            currentLogin.Server = txtServer.Text;
            currentLogin.UserName = txtUserName.Text;
            currentLogin.Password = txtPassword.Text;
            currentLogin.IsRunScanInBackground = checkInBackground.Checked ? SimpleDecision.Yes : SimpleDecision.None;

            return currentLogin;
        }

        private void txtServer_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtServer.Text))
            {
                errorProvider.SetError(txtServer, "Enter server domain");
                e.Cancel = true;
            }
            else
                errorProvider.SetError(txtServer, string.Empty);
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                errorProvider.SetError(txtUserName, "Enter user name");
                e.Cancel = true;
            }
            else
                errorProvider.SetError(txtUserName, string.Empty);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Enter password");
                e.Cancel = true;
            }
            else
                errorProvider.SetError(txtPassword, string.Empty);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            txtServer.Text = _server;
            txtUserName.Text = _userName;
            txtPassword.Text = _password;
            checkInBackground.Checked = false;
        }
    }
}

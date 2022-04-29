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
using CxViewerAction.Entities.WebServiceEntity;
using Common;
using EnvDTE;

namespace CxViewerAction.Views.DockedView
{
	/// <summary>
	/// Control represent addin custom settings form, available through "Tools" - "Options" menu item
	/// </summary>
	public partial class OptionsAuthCtrl : UserControl
	{
		#region [Private Members]
		private string _server = "";
		#endregion

		private LoginData currentLogin = null;
		private LoginResult loginResult = null;

		#region [Public Properties]

		#endregion

		#region [Constructors]

		public OptionsAuthCtrl()
		{
			InitializeComponent();
			if (Connect.IsLoaded)
			{
				BindDataToView();
			}
		}
		#endregion

		#region [Public Methods]




		/// <summary>
		/// Login click button handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, EventArgs e)
		{
			save();
			currentLogin = BindDataFromView();
			if (currentLogin.BindedProjects != null)
			{
				currentLogin.BindedProjects.Clear();
			}

			CommonData.IsProjectBound = false;
			LoginHelper.IsLogged = false;
			LoginHelper.Save(currentLogin);
			try
			{
				bool cancelPressed;
				loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, true);
				if (loginResult.IsSuccesfull)
				{
					MessageBox.Show("Login Successful", "Information", MessageBoxButtons.OK);
					loginBtn.Enabled = false;
					logoutBtn.Enabled = true;
				}

				else if (!string.IsNullOrWhiteSpace(currentLogin.AuthenticationType) && (currentLogin.AuthenticationType == (Common.Constants.AuthenticationaType_DefaultValue) || (currentLogin.AuthenticationType ==  Common.Constants.AuthenticationaType_IE)) && loginResult.LoginResultMessage != null && !loginResult.LoginResultMessage.Equals("Exit"))
				{
					MessageBox.Show("Login Failed", "Information", MessageBoxButtons.OK);
					loginBtn.Enabled = true;
					logoutBtn.Enabled = false;
				}
				else if (!string.IsNullOrWhiteSpace(currentLogin.AuthenticationType) && (currentLogin.AuthenticationType != (Common.Constants.AuthenticationaType_DefaultValue) || (currentLogin.AuthenticationType != Common.Constants.AuthenticationaType_IE)) && !loginResult.IsSuccesfull)
				{
					MessageBox.Show("Login Failed", "Information", MessageBoxButtons.OK);
					loginBtn.Enabled = true;
					logoutBtn.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				Logger.Create().Error(ex);
			}
		}

		private void save()
		{
			if (!Connect.IsLoaded)
			{
				MessageBox.Show("Checkmarx Visual Studio Plugin is disabled. \r\nPlease enable the plugin in Tools->Add-In Manager", "Information", MessageBoxButtons.OK);
				return;
			}
			try
			{
				txtServer_Validating(txtServer, new CancelEventArgs());
				if (!string.IsNullOrEmpty(errorProvider.GetError(txtServer)) || string.IsNullOrEmpty(txtServer.Text))
				{
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.Create().Error(ex);
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
			}
		}

		void CheckValidation()
		{
			txtServer_Validating(txtServer, new CancelEventArgs());
		}

		#endregion

		#region [Private Methods]

		/// <summary>
		/// Bind object data to form controls
		/// </summary>
		private void BindDataToView()
		{
			OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
			currentLogin = LoginHelper.Load(0);
			txtServer.Text = currentLogin.ServerDomain;
			if (oidcLoginData.AccessToken == null)
			{
				loginBtn.Enabled = true;
				logoutBtn.Enabled = false;
			}
			else
			{
				loginBtn.Enabled = false;
				logoutBtn.Enabled = true;
			}
		}

		/// <summary>
		/// Bind form contol values to object data
		/// </summary>
		/// <returns></returns>
		private LoginData BindDataFromView()
		{
			currentLogin = LoginHelper.Load(0);

			currentLogin.ServerDomain = txtServer.Text;

			return currentLogin;
		}

		/// <summary>
		/// Validate server name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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




		/// <summary>
		/// "Restore default" button handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRestore_Click(object sender, EventArgs e)
		{
			txtServer.Text = _server;
		}
		#endregion

		private void OptionsAuthCtrl_VisibleChanged(object sender, EventArgs e)
		{
			if (Connect.IsLoaded)
			{
				BindDataToView();
				errorProvider.Clear();
			}
		}


		public void OnOK()
		{
			save();
			if (Connect.IsLoaded && (string.IsNullOrEmpty(txtServer.Text)))
			{
				MessageBox.Show("Cannot save. Data is not valid.", "Error", MessageBoxButtons.OK);
			}
		}

		// Logout button
		private void logoutBtn_Click(object sender, EventArgs e)
		{
			if (loginResult != null)
			{
				loginResult.IsSuccesfull = false;
			}
			LoginHelper.DoLogout();
			MessageBox.Show("Logout Successful", "Information", MessageBoxButtons.OK);
			loginBtn.Enabled = true;
			logoutBtn.Enabled = false;

		}


		// Apply button
		private void button1_Click(object sender, EventArgs e)
		{
			OnOK();
		}

	}
}

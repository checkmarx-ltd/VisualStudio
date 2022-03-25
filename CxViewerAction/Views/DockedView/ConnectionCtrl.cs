using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;
using CxViewerAction.Entities.Enum;
using Common;

namespace CxViewerAction.Views.DockedView
{
    public partial class ConnectionCtrl : UserControl
	{
		public ConnectionCtrl()
		{
			InitializeComponent();
            if (Connect.IsLoaded)
            {
                BindDataToView();
            }
		}

		private void BindDataToView()
		{
			LoginData currentLogin = LoginHelper.Load(0);
			if (currentLogin == null)
			{
				btnRestore_Click(null, null);
			}
			else
			{
				txtTimeoutInterval.Text = currentLogin.ReconnectInterval.ToString();
				txtReconnectCount.Text = currentLogin.ReconnectCount.ToString();
				checkInBackground.Checked = (currentLogin.IsRunScanInBackground == SimpleDecision.Yes) ? true : false;
				txtUpdateStatusInterval.Text = currentLogin.UpdateStatusInterval.ToString();
                disableConnectionOptimizationsCheckBox.Checked = currentLogin.DisableConnectionOptimizations;
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
            save();
		}

        private void save()
        {
            if (!Connect.IsLoaded)
            {
                MessageBox.Show("Checkmarx Visual Studio Plugin is disable. \r\nPlease enable the plugin in Tools->Add-In Manager", "Information", MessageBoxButtons.OK);
                return;
            }
            try
            {
                if (ValidateValues())
                {
                    LoginData currentLogin = BindDataFromView();
                    LoginHelper.Save(currentLogin);
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

		private LoginData BindDataFromView()
		{
			LoginData currentLogin = LoginHelper.Load(0);
			currentLogin.ReconnectInterval = int.Parse(txtTimeoutInterval.Text);
			currentLogin.ReconnectCount = int.Parse(txtReconnectCount.Text);
			currentLogin.IsRunScanInBackground = checkInBackground.Checked ? SimpleDecision.Yes : SimpleDecision.None;
			currentLogin.UpdateStatusInterval = int.Parse(txtUpdateStatusInterval.Text);
            currentLogin.DisableConnectionOptimizations = disableConnectionOptimizationsCheckBox.Checked;
			return currentLogin;
		}

		private bool ValidateValues()
		{
			string errors = string.Empty;
			int val = 0;

			if (!int.TryParse(txtTimeoutInterval.Text, out val))
				errors += FormatError("Reconnect timeout: Please specify value.");

			if (!int.TryParse(txtReconnectCount.Text, out val))
				errors += FormatError("Reconnect count: Please specify value.");

			if (!int.TryParse(txtUpdateStatusInterval.Text, out val))
				errors += (FormatError("Scan interval: Please specify value."));

			if (!string.IsNullOrEmpty(errors))
			{
				MessageBox.Show(errors, "Error", MessageBoxButtons.OK);
			}

			return errors.Length == 0;
		}

		private string FormatError(string text)
		{
			return string.Format("{0}{1}", text, Environment.NewLine);
		}

		private void btnRestore_Click(object sender, EventArgs e)
		{
			txtUpdateStatusInterval.Text = "20";
			txtTimeoutInterval.Text = "15";
			txtReconnectCount.Text = "3";
			checkInBackground.Checked = false;
            disableConnectionOptimizationsCheckBox.Checked = false;
		}

		private void txtTimeoutInterval_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Back:
				case Keys.Left:
				case Keys.Return:
				case Keys.Right:
				case Keys.Space:
				case Keys.Tab:
				case Keys.U:
					break;
				default:
					e.SuppressKeyPress = !(e.KeyValue >= 48 && e.KeyValue <= 57);
					break;
			}
		}

        public void OnOK()
        {
            save();
        }
	}
}

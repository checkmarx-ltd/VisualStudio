using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Helpers;
using CxViewerAction2022.Entities.Enum;
using EnvDTE;

namespace CxViewerAction2022.Views.DockedView
{
    /// <summary>
    /// Control represent addin custom settings form, available through "Tools" - "Options" menu item
    /// </summary>
    public partial class OptionsZipCtrl : UserControl
    {
        #region [Public Properties]

        /// <summary>
        /// Gets or sets user decision value to open perspective immediately after scan complete
        /// </summary>
        public SimpleDecision IsOpenPerspectiveNow
        {
            get
            {
				//if (radioPrompt.Checked)
				//    return SimpleDecision.None;
				//else if (radioAlways.Checked)
				//    return SimpleDecision.Yes;
				//else
                    return SimpleDecision.No;
            }

            set
            {
				//if (value == SimpleDecision.None)
				//    radioPrompt.Checked = true;
				//else if (value == SimpleDecision.Yes)
				//    radioAlways.Checked = true;
				//else
				//    radioNever.Checked = true;
            }
        }

        #endregion

        #region [Constructors]
        public OptionsZipCtrl()
        {
            InitializeComponent();
            if (Connect.IsLoaded)
            {
                BindDataToView();
            }
        }
        #endregion

        #region [Private Methods]

        /// <summary>
        /// Save button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void save()
        {
            try
            {
                if (!Connect.IsLoaded)
                {
                    MessageBox.Show("Checkmarx Visual Studio Plugin is disable. \r\nPlease enable the plugin in Tools->Add-In Manager", "Information", MessageBoxButtons.OK);
                    return;
                }
                string errorMessage;
                if (txtExcludeFileExtValidating(out errorMessage) && txtExcludeFolderValidating(out errorMessage)
                    && txtZipMazSizeValidating(out errorMessage))
                {
                    LoginData currentLogin = BindDataFromView();
                    LoginHelper.Save(currentLogin);
                }
                else
                {
                    TopMostMessageBox.Show(errorMessage, "Error");
                }
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Bind object data to form controls
        /// </summary>
        private void BindDataToView()
        {
            LoginData currentLogin = LoginHelper.Load(0);

            StringBuilder ext = new StringBuilder();
            foreach (string s in currentLogin.ExcludeFileExt)
                ext.Append(s + ",");

            StringBuilder folder = new StringBuilder();
            foreach (string s in currentLogin.ExcludeFolder)
                folder.Append(s + ",");

            txtExcludeFileExt.Text = ext.Remove(ext.Length - 1, 1).ToString();
            txtExcludeFolder.Text = folder.Remove(folder.Length - 1, 1).ToString();
            txtZipMazSize.Text = currentLogin.MaxZipFileSize.ToString();
			IsOpenPerspectiveNow = currentLogin.IsOpenPerspective;
        }

        /// <summary>
        /// Bind form control values to object data
        /// </summary>
        /// <returns></returns>
        private LoginData BindDataFromView()
        {
            LoginData currentLogin = LoginHelper.Load(0);
            currentLogin.ExcludeFileExt = txtExcludeFileExt.Text.Split(',');
            currentLogin.ExcludeFolder = txtExcludeFolder.Text.Split(',');
			currentLogin.MaxZipFileSize = int.Parse(txtZipMazSize.Text);
            currentLogin.IsOpenPerspective = IsOpenPerspectiveNow;
            return currentLogin;
        }

        /// <summary>
        /// Validate set of excluded extensions: not empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExcludeFileExt_Validating(object sender, CancelEventArgs e)
        {
            string errorMessage;
            if (!txtExcludeFileExtValidating(out errorMessage))
            {
                errorProvider.SetError(txtExcludeFileExt, errorMessage);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(txtExcludeFileExt, string.Empty);
        }

        private bool txtExcludeFileExtValidating(out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(txtExcludeFileExt.Text))
            {
                errorMessage = "Enter extensions to exclude";
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Validate set of excluded folders: not empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExcludeFolder_Validating(object sender, CancelEventArgs e)
        {
            string errorMessage;
            if (!txtExcludeFolderValidating(out errorMessage))
            {
                errorProvider.SetError(txtExcludeFolder, errorMessage);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(txtExcludeFolder, string.Empty);
        }

        private bool txtExcludeFolderValidating(out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(txtExcludeFolder.Text))
            {
                errorMessage = "Enter folders to exclude";
                return false;
            }
            else
                return true;

        }

		private void txtZipMazSize_Validating(object sender, CancelEventArgs e)
        {
            string errorMessage;
            if (!txtZipMazSizeValidating(out errorMessage))
            {
                errorProvider.SetError(txtZipMazSize, errorMessage);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(txtZipMazSize, string.Empty);
        }

        private bool txtZipMazSizeValidating(out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(txtZipMazSize.Text))
            {
                errorMessage = "Enter max ZIP file size";
                return false;
            }

            int size;
            if (!int.TryParse(txtZipMazSize.Text, out size) || size < 1)
            {
                errorMessage = "ZIP file size is numeric value more then 1";
                return false;
            }

            return true;
        }
        #endregion

        private void OptionsZipCtrl_VisibleChanged(object sender, EventArgs e)
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
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.Entities;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Helpers;
using System.Text.RegularExpressions;
using System.IO;

namespace CxViewerAction.Views
{
    public partial class UploadFrm : Form, IUploadView
    {
        #region [Constants]

        /// <summary>
        /// Project name allowed format RegEx
        /// </summary>
        private const string _checkProjectNamePattern = @"^[a-zA-Z_][.a-zA-Z\d_ ]*$";

        #endregion

        #region [Private members]

        private EntityId _entityId;
        private Dictionary<int, string> _presets;
        private Dictionary<string, string> _teams;

        #endregion

        #region [Properties]

        /// <summary>
        /// Gets or sets custom identity
        /// </summary>
        public EntityId EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Gets or sets project name
        /// </summary>
        public string ProjectName
        {
            get { return txtProjectName.Text; }
            set { txtProjectName.Text = value; }
        }

        /// <summary>
        /// Gets or sets project description
        /// </summary>
        public string Description
        {
            get { return txtDescription.Text; }
            set { txtDescription.Text = value; }
        }

        /// <summary>
        /// Gets or sets selected preset
        /// </summary>
        public int Preset
        {
            get { return ((KeyValuePair<int, string>)cmbPreset.SelectedItem).Key; }
            set {
                
                if (cmbPreset.Items.Count > 0)
                    cmbPreset.SelectedValue = value; 
            }
        }

        /// <summary>
        /// Gets or sets selected team
        /// </summary>
        public string Team
        {
            get { return ((KeyValuePair<string, string>)cmbTeams.SelectedItem).Key; }
            set
            {

                if (cmbTeams.Items.Count > 0)
                    cmbTeams.SelectedValue = value;
            }
        }

        public bool IsPublic
        {
            get { return cbPublic.Checked; }
        }

        /// <summary>
        /// Gets or sets preset list
        /// </summary>
        public Dictionary<int, string> Presets
        {
            get { return _presets; }
            set 
            { 
                _presets = value;

                if (cmbPreset.Items != null)
                    cmbPreset.Items.Clear();

                foreach (KeyValuePair<int, string> preset in _presets)
                    cmbPreset.Items.Add(preset);

                if (cmbPreset.Items.Count > 0)
                    cmbPreset.SelectedIndex = 0;
                cmbPreset.DisplayMember = "value";
                cmbPreset.ValueMember = "key";

            }
        }

        /// <summary>
        /// Gets or sets team list
        /// </summary>
        public Dictionary<string, string> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;

                if (cmbTeams.Items != null)
                    cmbTeams.Items.Clear();

                foreach (KeyValuePair<string, string> team in _teams)
                    cmbTeams.Items.Add(team);

                if (cmbTeams.Items.Count > 0)
                    cmbTeams.SelectedIndex = 0;
                cmbTeams.DisplayMember = "value";
                cmbTeams.ValueMember = "key";

            }
        }

        #endregion

        public UploadFrm(string sessionId)
        {
            InitializeComponent();
        }

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

        /// <summary>
        /// Validation handler to control project name entered value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProjectName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtProjectName.Text)) //Check for non empty project name field
            {
                e.Cancel = true;
                errorProvider.SetError((Control)sender, "Please enter project name");
            }
            else if (txtProjectName.Text.Length > 200)
            {
                e.Cancel = true;
                errorProvider.SetError((Control)sender, "Project name is limited to 200 characters, please edit and try again");
            }
            //else if (txtProjectName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
            //{
            //    e.Cancel = true;
            //    // Illeagle chars should be presented to User
            //    // David will add it.
            //    errorProvider.SetError((Control)sender, "Project name contains illeagle characters: (\\ / : * ? \" < > |)");                    
            //}
        }

        /// <summary>
        /// Validation handler to control thar preset item was selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPreset_Validating(object sender, CancelEventArgs e)
        {
            if (cmbPreset.SelectedItem == null)
            {
                e.Cancel = true;
                errorProvider.SetError((Control)sender, "Please select preset");
            }
            else
                errorProvider.SetError((Control)sender, string.Empty);
        }

        /// <summary>
        /// Validation handler to control thar team item was selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTeams_Validating(object sender, CancelEventArgs e)
        {
            if (cmbTeams.SelectedItem == null)
            {
                e.Cancel = true;
                errorProvider.SetError((Control)sender, "Please select team");
            }
            else
                errorProvider.SetError((Control)sender, string.Empty);
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                e.Cancel = true;
                errorProvider.SetError((Control)sender, "Please enter project description");
            }
            else
                errorProvider.SetError((Control)sender, string.Empty);
        }

        void CheckValidation()
        {
            txtProjectName_Validating(txtProjectName, new CancelEventArgs());
            txtDescription_Validating(txtDescription, new CancelEventArgs());
            cmbPreset_Validating(cmbPreset, new CancelEventArgs());
            cmbTeams_Validating(cmbTeams, new CancelEventArgs());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CheckValidation();
            if (!string.IsNullOrEmpty(errorProvider.GetError(txtProjectName)) ||
            !string.IsNullOrEmpty(errorProvider.GetError(txtDescription)) ||
            !string.IsNullOrEmpty(errorProvider.GetError(cmbPreset))
            )
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private void UploadFrm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void txtProjectName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void cmbPreset_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void cmbTeams_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.CxVSWebService;
using Common;

namespace CxViewerAction.Views
{
    public partial class frmBindingPrjList : Form, IBindProjectView
    {
        public frmBindingPrjList()
        {
            Logger.Create().Debug("frmBindingPrjList in");
            InitializeComponent();
        }

        private void BindingPrjList_Load(object sender, EventArgs e)
        {
            dgvProjects.MultiSelect = false;
        }

        ProjectDisplayData[] _projectList;

        public ProjectDisplayData[] ProjectList
        {
            get { return _projectList; }
            set
            {
                _projectList = value;
                if (_projectList != null && _projectList.Length > 0)
                {                
                    foreach (ProjectDisplayData item in value)
                    {
                        dgvProjects.Rows.Add(item.projectID, item.ProjectName, item.Owner, item.Group);            
                    }                                       
                    dgvProjects.Rows[0].Selected = true;
                    dgvProjects.Refresh();           
                }
            }
        }

        public ProjectDisplayData SelectedProject
        {
            get
            {
                if (dgvProjects.SelectedRows.Count > 0 && _projectList != null)
                {
                    foreach (ProjectDisplayData item in _projectList)
                    {
                        if (item.projectID == Convert.ToInt64(dgvProjects.SelectedRows[0].Cells[0].Value))
                        {
                            return item;
                        }
                    }                    
                }
                return null;
            }  
        }

        public bool isPublic
        {
            get
            {
                return cbPublic.Checked;
            }
        }


        #region IView Members

        public DialogResult ShowModalView()
        {
            return this.ShowDialog();
        }

        public DialogResult ShowModalView(IView parent)
        {
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

        public void CloseView()
        {
            this.Close();
        }

        #endregion
    }
}
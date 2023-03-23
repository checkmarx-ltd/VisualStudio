using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.CxVSWebService;
using Common;
using CefSharp.DevTools.CSS;
using System.Linq;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;

namespace CxViewerAction.Views
{
    public partial class frmBindingPrjList : Form, IBindProjectView
    {
        private int defaultProjectDisplayCount = 100;
        public frmBindingPrjList()
        {
            Logger.Create().Debug("Loading bind Project list form.");
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
                DisplayProjectLists(value);
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

        private void DisplayProjectLists(ProjectDisplayData[] projectList)
        {
            dgvProjects.Rows.Clear();
            if (projectList != null && projectList.Length > 0)
            {
                LoginData _loginResult = LoginHelper.Load(0);
                if(_loginResult.BindProjectCount == -1)
                {
                    checkBox1.Show();
                    if (checkBox1.Checked == false && string.IsNullOrEmpty(textBox1.Text))
                    {
                        addInBindProjectsList(projectList, defaultProjectDisplayCount);
                    }
                    else if (checkBox1.Checked == true && string.IsNullOrEmpty(textBox1.Text))
                    {
                        addInBindProjectsList(projectList);
                    }
                    else if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        if (checkBox1.Checked == true)
                        {
                            addInBindProjectsList(searchProjectsByName(_projectList));
                        }
                        else
                        {
                            addInBindProjectsList(searchProjectsByName(_projectList), defaultProjectDisplayCount);
                        }
                    }
                }
                else
                {
                    checkBox1.Hide();
                    if(string.IsNullOrEmpty(textBox1.Text))
                    {
                        addInBindProjectsList(_projectList, _loginResult.BindProjectCount);
                    }
                    else if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        addInBindProjectsList(searchProjectsByName(_projectList), _loginResult.BindProjectCount);
                    }
                }
                dgvProjects.Rows[0].Selected = true;
                dgvProjects.Refresh();
            }
        }

        private ProjectDisplayData[] searchProjectsByName(ProjectDisplayData[] _projectList)
        {
            return Array.FindAll(_projectList, element => element.ProjectName.Contains(textBox1.Text)).ToArray();
        }

        private void addInBindProjectsList(ProjectDisplayData[] _projectList,int projectDisplayCount=-1)
        {
            if(projectDisplayCount == -1)
            {
                foreach (ProjectDisplayData item in _projectList)
                {
                    dgvProjects.Rows.Add(item.projectID, item.ProjectName, item.Owner, item.Group);
                }
            }
            else
            {
                for (int j = 0; j < _projectList.Count() && j < projectDisplayCount; j++)
                {
                    dgvProjects.Rows.Add(_projectList[j].projectID, _projectList[j].ProjectName, _projectList[j].Owner,
                                        _projectList[j].Group);
                }
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DisplayProjectLists(_projectList);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DisplayProjectLists(_projectList);
        }
        #endregion
    }
}
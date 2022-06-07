using System;
using System.Collections.Generic;
using System.Text;
using CxViewerAction2022.CxVSWebService;
using System.Windows.Forms;

namespace CxViewerAction2022.Entities
{
    public class BindProjectEntity : IEntity
    {
        #region Variables

        static bool isUploading;
        CxWSResponseProjectsDisplayData cxProjectsDisplayData;
        DialogResult commandResult;

        #endregion

        #region Properties

        public bool IsUploading
        {
            get { return isUploading; }
            set { isUploading = value; }
        }

        public CxWSResponseProjectsDisplayData CxProjectsDisplayData
        {
            get { return cxProjectsDisplayData; }
            set { cxProjectsDisplayData = value; }
        }

        ProjectDisplayData selectedProject;
        public ProjectDisplayData SelectedProject
        {
            get { return selectedProject; }
            set { selectedProject = value; }
        }

        public DialogResult CommandResult
        {
            get { return commandResult; }
            set { commandResult = value; }
        }

        public bool isPublic
        {
            get;
            set;
        }

        #endregion
    }
}

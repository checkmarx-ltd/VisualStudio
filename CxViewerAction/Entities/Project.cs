using System;
using System.Collections.Generic;

using System.Text;

namespace CxViewerAction.Entities
{
    /// <summary>
    /// Class represent project entity
    /// </summary>
    public class Project
    {
        #region [Properties]
        /// <summary>
        /// Solution project name
        /// </summary>
        private string _projectName;

        /// <summary>
        /// Gets or sets solution project name
        /// </summary>
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        /// <summary>
        /// Solution path or single project path
        /// </summary>
        private string _rootPath;

        /// <summary>
        /// Gets or sets root solution path or single project path
        /// </summary>
        public string RootPath
        {
            get { return _rootPath; }
            set { _rootPath = value; }
        }

        /// <summary>
        /// Solution projects full file paths
        /// </summary>
        private List<Project> _projectPaths;

        /// <summary>
        /// Gets or sets solution project full file path
        /// </summary>
        public List<Project> ProjectPaths
        {
            get { return _projectPaths; }
            set { _projectPaths = value; }
        }
        #endregion

        #region [Constructors]

        public Project(string projectName, string rootPath, List<string> filePathList, List<string> folderPathList)
        {
            _projectName = projectName;
            _rootPath = rootPath;
            _projectPaths = new List<Project>();
            FilePathList = filePathList;
            FolderPathList = folderPathList;
        }

        public Project(string projectName, string rootPath)
        {
            _projectPaths = new List<Project>();
            FilePathList = new List<string>();
            FolderPathList = new List<string>();
            _projectName = projectName;
            _rootPath = rootPath;
        }

        public List<string> FilePathList
        {
            get;
            set;
        }

        public List<string> FolderPathList
        {
            get;
            set;
        }

        #endregion
    }
}

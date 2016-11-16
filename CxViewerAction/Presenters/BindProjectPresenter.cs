using System;

// using CxViewerAction.Managers;
using CxViewerAction.Entities;
using CxViewerAction.Dispatchers;
using CxViewerAction.ServiceLocators;
using CxViewerAction.Views;

namespace CxViewerAction.Presenters
{
    /// <summary>
    /// Represent upload controller object
    /// </summary>
    public class BindProjectPresenter : IBindProjectPresenter
    {
        #region [ Private Members ]

        private IDispatcher _dispatcher;
        private IBindProjectView _view;

        #endregion [ Private Members ]

        #region [ Events Handlers ]

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public BindProjectPresenter(IBindProjectView view)
        {
            this._view = view;
            this._view.Load += View_Load;

            _dispatcher = ServiceLocator.GetDispatcher();
        }

        /// <summary>
        /// Load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void View_Load(object sender, EventArgs args)
        {

        }

        #endregion [ Event Handlers ]

        #region [ Public Methods ]

        /// <summary>
        /// Perform upload project
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="upload">Upload data</param>
        public void BindProject(IView parentView, BindProjectEntity upload)
        {
            MapViewData(upload);
            if (!upload.IsUploading)
            {
                upload.IsUploading = true;
                try
                {
                    bool isConfirmed = _view.ShowModalView(parentView) == System.Windows.Forms.DialogResult.OK;
                    MapObjectData(ref upload, isConfirmed);
					upload.CommandResult = (isConfirmed) ? System.Windows.Forms.DialogResult.OK : System.Windows.Forms.DialogResult.Cancel;
                    return;
                }
                finally
                {
                    upload.IsUploading = false;
                }
            }
            upload.SelectedProject = null;
            upload.CommandResult = System.Windows.Forms.DialogResult.Cancel;

        }

        /// <summary>
        /// Map upload object data to view
        /// </summary>
        /// <param name="upload"></param>
        private void MapViewData(BindProjectEntity upload)
        {
            _view.ProjectList = upload.CxProjectsDisplayData.projectList;
        }

        /// <summary>
        /// Map view data to upload object
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="isUploading"></param>
        private void MapObjectData(ref BindProjectEntity upload, bool isUploading)
        {
            if (isUploading)
            {
                upload.SelectedProject = _view.SelectedProject;
                upload.isPublic = _view.SelectedProject.IsPublic;
            }
            else
            {
                upload.SelectedProject = null;
            }
        }

        #endregion
    }
}


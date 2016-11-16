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
    public class UploadPresenter : IUploadPresenter
    {
        #region [ Private Members ]

        private IDispatcher _dispatcher;
        private IUploadView _view;

        #endregion [ Private Members ]

        #region [ Events Handlers ]

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public UploadPresenter(IUploadView view)
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
        public void Upload(IView parentView, Upload upload)
        {
            MapViewData(upload);
            if (!upload.IsUploading)
                MapObjectData(ref upload, _view.ShowModalView(parentView) == System.Windows.Forms.DialogResult.OK);
        }

        /// <summary>
        /// Map upload object data to view
        /// </summary>
        /// <param name="upload"></param>
        private void MapViewData(Upload upload)
        {
            _view.EntityId = upload.ID;
            _view.ProjectName = upload.ProjectName;
            _view.Description = upload.Description;
            _view.Presets = upload.Presets;
            _view.Preset = upload.Preset;
            _view.Team = upload.Team;
            _view.Teams = upload.Teams;
        }

        /// <summary>
        /// Map view data to upload object
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="isUploading"></param>
        private void MapObjectData(ref Upload upload, bool isUploading)
        {
            upload.ProjectName = _view.ProjectName;
            upload.Description = _view.Description;
            upload.Preset = _view.Preset;
            upload.Team = _view.Team;
            upload.IsUploading = isUploading;
            upload.IsPublic = _view.IsPublic;
        }

        #endregion
    }
}

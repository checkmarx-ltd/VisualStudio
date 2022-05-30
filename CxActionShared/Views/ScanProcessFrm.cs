using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using CxViewerAction2022.Entities.FormEntity;
using CxViewerAction2022.Entities;

namespace CxViewerAction2022.Views
{
    public partial class ScanProcessFrm : Form, IScanView
    {
        #region [Private Members]

        private EntityId _entityId;

        #endregion

        #region [Delegates]

        private delegate void UpdateLableTextDelegate(ScanProgress value);
        private delegate void UpdateProgressDelegate(ScanProgress value);
        private delegate void IncrementProgressDelegate(int value);
        private delegate void CloseViewDelegate();
        private delegate void SetVisiblityDelegate(bool visiblity);
        
        #endregion

        #region [Private Constants, label format]

        private const string CURRENT_ITERATION_NAME_FORMAT = "{0} \"{1}\"";
        private const string CURRENT_ITERATION_PROGRESS_FORMAT = "({0}%) {1}";
        private const string NOT_READY_TEXT = "Waiting server...";
        private const string PROJECT_SCAN_TOTAL_PROGRESS = "Project scan total progress: {0}%";
        private const string PROJECT_NAME = "Scanning project {0}";
        
        #endregion

        #region [Public Properties]

        /// <summary>
        /// Gets or sets custom entity identifier
        /// </summary>
        public EntityId EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Sets progress bar state
        /// </summary>
        public ScanProgress Progress
        {
            set
            {
                //Use delegate in case of async calls
                Invoke(new UpdateLableTextDelegate(UpdateCurrentIterationName), value);
                Invoke(new UpdateLableTextDelegate(UpdateProjectName), value);
                Invoke(new UpdateLableTextDelegate(UpdateCurrentIterationProgress), value);
                Invoke(new UpdateLableTextDelegate(UpdateProjectScanTotalProgress), value);
                Invoke(new UpdateLableTextDelegate(UpdateProjectScanTotalTopProgress), value);
                Invoke(new UpdateProgressDelegate(UpdateProgress), value);
            }
        }

        /// <summary>
        /// Gets alwaysInBackground checkbox state
        /// </summary>
        public bool AlwaysInBackground
        {
            get { return chBackground.Checked; }
        }

        /// <summary>
        /// Gets or sets dialog visibility
        /// </summary>
        public bool Visibility
        {
            get { return Visible; }
            set
            {
                SetVisiblityDelegate del = new SetVisiblityDelegate(SetVisiblity);
                Invoke(del, value);
            }
        }

        /// <summary>
        /// Gets or sets RunInBackground button handler
        /// </summary>
        public EventHandler RunInBackgroundHandler
        {
            get { return null; }
            set { btnRunBackground.Click += value; }
        }

        /// <summary>
        /// Gets or sets cancel button handler
        /// </summary>
        public EventHandler CancelHandler
        {
            get { return null; }
            set
            {
                btnCancel.Click += value;
                FormClosed += new FormClosedEventHandler(value);
            }
        }

        /// <summary>
        /// Gets or sets details button handler
        /// </summary>
        public EventHandler DetailsHandler
        {
            get { return null; }
            set { btnDetails.Click += value; }
        }

        #endregion

        public ScanProcessFrm()
        {
            InitializeComponent();
        }

        #region [Public methods]
        /// <summary>
        /// Show modal dialog
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowModalView()
        {
            return this.ShowDialog();
        }

        /// <summary>
        /// Show modal dialog
        /// </summary>
        /// <param name="parent">Parent view container</param>
        /// <returns></returns>
        public DialogResult ShowModalView(IView parent)
        {
            return this.ShowDialog((IWin32Window)parent);
        }

        /// <summary>
        /// Show non-modal dialog
        /// </summary>
        public void ShowView()
        {
            this.Show();
        }

        /// <summary>
        /// Show non-modal dialog
        /// </summary>
        /// <param name="parent">Parent view container</param>
        public void ShowView(IView parent)
        {
            this.Show((IWin32Window)parent);
        }

        /// <summary>
        /// Close view
        /// </summary>
        public void CloseView()
        {
            //Use delegate in case of async calls
            CloseViewDelegate del = new CloseViewDelegate(CloseDialogView);
            Invoke(del);
        }

        /// <summary>
        /// Increment progress state
        /// </summary>
        /// <param name="value">increment value</param>
        public void Increment(int value)
        {
            Invoke(new IncrementProgressDelegate(IncrementProgress), value);
            Invoke(new UpdateLableTextDelegate(UpdateCurrentIterationProgress), new ScanProgress(progressMargue1.Maximum, progressMargue1.Maximum, progressMargue1.Value));
        }

        #endregion

        #region [Private methods]

        /// <summary>
        /// Calculate percent difference by current and max values
        /// </summary>
        /// <param name="current"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private decimal CalcPercent(int current, int max)
        {
            return Math.Round((decimal)(current / max) * 100);
        }

        /// <summary>
        /// Update project name delegate function. Used for async calls
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProjectName(ScanProgress value)
        {
            lblProjectName.Text = string.Format(PROJECT_NAME, value.ProjectName);
        }

        /// <summary>
        /// Update project scan progress delegate function. Used for async calls
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProjectScanTotalProgress(ScanProgress value)
        {
            lblTotalScanProgress.Text = string.Format(PROJECT_SCAN_TOTAL_PROGRESS, value.CurrentPosition);
        }

        /// <summary>
        /// Update project project scan progress delegate function. Used for async calls
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProjectScanTotalTopProgress(ScanProgress value)
        {
            lblProgressTotalTop.Text = string.Format(PROJECT_SCAN_TOTAL_PROGRESS, value.CurrentPosition);
        }

        /// <summary>
        /// Update current iteration name delegate function. Used for async calls
        /// </summary>
        /// <param name="value"></param>
        private void UpdateCurrentIterationName(ScanProgress value)
        {
            lblCurrentIterationName.Text = string.Format(CURRENT_ITERATION_NAME_FORMAT, value.RunStatus, value.CurrentStageName);
        }

        /// <summary>
        /// Update current iteration progress status delegate function. Used for async calls
        /// </summary>
        /// <param name="value"></param>
        private void UpdateCurrentIterationProgress(ScanProgress value)
        {
            lblCurrentIterationProgress.Text = string.Format(CURRENT_ITERATION_PROGRESS_FORMAT, value.CurrentStagePercent, value.CurrentStageMessage);
        }

        /// <summary>
        /// Update progress status delegate function. Used for async calls
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgress(ScanProgress value)
        {
            progressTotal.Minimum = value.StartPosition;
            progressTotal.Maximum = value.EndPosition;
            progressTotal.Value = value.CurrentPosition;
        }

        /// <summary>
        /// Increment progress delegate function. Used for async calls
        /// </summary>
        /// <param name="value"></param>
        private void IncrementProgress(int value)
        {
            progressMargue1.Increment(value);
        }

        /// <summary>
        /// Clear progress. Fill by empty values
        /// </summary>
        public void Clear()
        {
            Progress = new ScanProgress(NOT_READY_TEXT, string.Empty, string.Empty, 0, 100, 0);
        }

        /// <summary>
        /// Close view
        /// </summary>
        private void CloseDialogView()
        {
            Close();
        }

        /// <summary>
        /// Set dialog visibility delegate function. Used for async calls
        /// </summary>
        /// <param name="visible"></param>
        private void SetVisiblity(bool visible)
        {
            Visible = visible;
        }

        /// <summary>
        /// Cancel button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnCancel.Text = "Canceling";
        }

        /// <summary>
        /// Details button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (pnlDetails.Visible) //if panel need close
            {
                pnlDetails.Visible = false;
                Height = 200;
                pnlDetails.Height = 0;
                btnDetails.Text = "Details >>";
            }
            else // if panel need open
            {
                pnlDetails.Visible = true;
                Height = 400;
                pnlDetails.Height = 200;
                btnDetails.Text = "<< Details";
            }
        }
        #endregion
    }
}

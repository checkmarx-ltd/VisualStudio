using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using CxViewerAction2022.Entities.FormEntity;
using CxViewerAction2022.Entities;

namespace CxViewerAction2022.Views.DockedView
{
    public partial class ScanProcessCtrl : UserControl, IScanView
    {
        #region [Private members]

        private EntityId _entityId;
        private bool _visibility;

        #endregion

        #region [Delegates]
        private delegate void UpdateTextDelegate(ScanProgress value);
        private delegate void UpdateProgressDelegate(ScanProgress value);
        private delegate void IncrementProgressDelegate(int value);
        private delegate void CloseViewDelegate();
        #endregion

        #region [Private Constants]
        private const string _lblCurrentIterationNameFormat = "{0} \"{1}\"";
        private const string _lblCurrentIterationProgressFormat = "{0} {1}";
        private const string _lblProjectNameFormat = "Process for {0}";
        private const string _notReadyText = "Waiting server...";
        #endregion

        #region [Public Properties]
        public EntityId EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }
        #endregion
        
        public ScanProcessCtrl()
        {
            InitializeComponent();
        }

        public DialogResult ShowModalView()
        {
            return DialogResult.None;
        }

        public DialogResult ShowModalView(IView parent)
        {
            return DialogResult.None;
        }

        public void ShowView()
        {
            this.Show();
        }

        public void ShowView(IView parent)
        {
        }

        public void CloseView()
        {
            CloseViewDelegate del = new CloseViewDelegate(CloseDialogView);
            Invoke(del);
        }

        public ScanProgress Progress
        {
            set
            {
                UpdateTextDelegate del1 = new UpdateTextDelegate(UpdateProjectName);
                Invoke(del1, value);

                UpdateTextDelegate del2 = new UpdateTextDelegate(UpdateCurrentIterationName);
                Invoke(del2, value);

                UpdateTextDelegate del3 = new UpdateTextDelegate(UpdateCurrentIterationProgress);
                Invoke(del3, value);

                UpdateProgressDelegate del4 = new UpdateProgressDelegate(UpdateProgress);
                Invoke(del4, value);
            }
        }

        public bool Visibility
        {
            get { return _visibility; }
            set { _visibility = value; }
        }

        public bool AlwaysInBackground
        {
            get { return false; }
        }

        public void Increment(int value)
        {
            IncrementProgressDelegate del1 = new IncrementProgressDelegate(IncrementProgress);
            Invoke(del1, value);

            UpdateTextDelegate del2 = new UpdateTextDelegate(UpdateCurrentIterationProgress);
            Invoke(del2, new ScanProgress(progressTotal.Maximum, progressTotal.Maximum, progressTotal.Value));
        }

        public void Clear()
        {
            Progress = new ScanProgress(_notReadyText, string.Empty, string.Empty, 0, 100, 0);
        }

        public EventHandler RunInBackgroundHandler
        {
            get { return null; }
            set {  }
        }

        public EventHandler CancelHandler
        {
            get { return null; }
            set {  }
        }

        public EventHandler DetailsHandler
        {
            get { return null; }
            set {  }
        }

        #region [Private methods]
        private decimal CalcPercent(int current, int max)
        {
            return Math.Round((decimal)(current / max) * 100);
        }

        private void UpdateCurrentIterationName(ScanProgress value)
        {
        }

        private void UpdateProjectName(ScanProgress value)
        {
            if (value.ProjectName == _notReadyText)
                lblProjectName.Text = _notReadyText;
            else
                lblProjectName.Text = string.Format(_lblProjectNameFormat, value.ProjectName);
        }

        private void UpdateCurrentIterationProgress(ScanProgress value)
        {
            lblCurrentIterationProgress.Text = string.Format(_lblCurrentIterationProgressFormat, value.RunStatus, value.CurrentStageMessage);
        }

        private void UpdateProgress(ScanProgress value)
        {
            progressTotal.Minimum = value.StartPosition;
            progressTotal.Maximum = value.EndPosition;
            progressTotal.Value = value.CurrentPosition;
        }

        private void IncrementProgress(int value)
        {
            progressTotal.Increment(value);
        }

        private void CloseDialogView()
        {
        }
        #endregion
    }
}

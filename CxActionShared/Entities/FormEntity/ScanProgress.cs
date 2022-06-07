using System;
using System.Collections.Generic;

using System.Text;

namespace CxViewerAction2022.Entities.FormEntity
{
    /// <summary>
    /// Represent scanb dialod data
    /// </summary>
    public class ScanProgress
    {
        #region [Private Members]

        private string _projectName = string.Empty;
        private int _startPosition = 0;
        private int _endPosition = 0;
        private int _currentPosition = 0;
        private string _runStatus = string.Empty;
        private string _currentStageName = string.Empty;
        private string _currentStageMessage = string.Empty;
        private int _currentStagePercent = 0;
        private int _currentStageNum = 0;

        #endregion

        #region [Public Properties]

        /// <summary>
        /// Scanned project name
        /// </summary>
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        /// <summary>
        /// Start position in progress
        /// </summary>
        public int StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }

        /// <summary>
        /// End position in progress
        /// </summary>
        public int EndPosition
        {
            get { return _endPosition; }
            set { _endPosition = value; }
        }

        /// <summary>
        /// Current progress position
        /// </summary>
        public int CurrentPosition
        {
            get { return _currentPosition; }
            set { _currentPosition = value; }
        }

        /// <summary>
        /// Current status activity name
        /// </summary>
        public string RunStatus
        {
            get { return _runStatus; }
            set { _runStatus = value; }
        }

        /// <summary>
        /// Current stage
        /// </summary>
        public string CurrentStageName
        {
            get { return _currentStageName; }
            set { _currentStageName = value; }
        }

        /// <summary>
        /// Current stage message
        /// </summary>
        public string CurrentStageMessage
        {
            get { return _currentStageMessage; }
            set { _currentStageMessage = value; }
        }

        /// <summary>
        /// Current stage complete percent
        /// </summary>
        public int CurrentStagePercent
        {
            get { return _currentStagePercent; }
            set { _currentStagePercent = value; }
        }

        /// <summary>
        /// Current stage number
        /// </summary>
        public int CurrentStageNum
        {
            get { return _currentStageNum; }
            set { _currentStageNum = value; }
        }

        #endregion

        public ScanProgress()
        {
        }

        public ScanProgress(int startPosition, int endPosition, int currentPosition)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            _currentPosition = currentPosition;
        }

        public ScanProgress(string projectName, string runStatus, string currentStageMessage, int startPosition, int endPosition, int currentPosition)
        {
            _projectName = projectName;
            _runStatus = runStatus;
            _currentStageMessage = currentStageMessage;
            _startPosition = startPosition;
            _endPosition = endPosition;
            _currentPosition = currentPosition;

        }

        public ScanProgress(string projectName, string runStatus, string currentStageName, string currentStageMessage, int currentStagePercent, int startPosition, int endPosition, int currentPosition)
        {
            _projectName = projectName;
            _runStatus = runStatus;
            _currentStageName = currentStageName;
            _currentStageMessage = currentStageMessage;
            _currentStagePercent = currentStagePercent;
            _startPosition = startPosition;
            _endPosition = endPosition;
            _currentPosition = currentPosition;

        }
    }
}

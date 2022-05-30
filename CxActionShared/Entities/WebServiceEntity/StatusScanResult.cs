using System;
using CxViewerAction2022.Helpers;
using CxViewerAction2022.CxVSWebService;

namespace CxViewerAction2022.Entities.WebServiceEntity
{
    /// <summary>
    /// Scan status
    /// </summary>
    public enum RunStatus
    {
        /// <summary>
        /// Unknown status
        /// </summary>
        None     = 0,

        /// <summary>
        /// Scan run
        /// </summary>
        Running  = 1,

        /// <summary>
        /// Scan finished
        /// </summary>
        Finished = 2,

        /// <summary>
        /// Project are queued for scan
        /// </summary>
        Queued   = 3,

        /// <summary>
        /// Scan cann't be processed
        /// </summary>
        Failed   = 4
    }

    /// <summary>
    /// Represent remote service status scan object
    /// </summary>
    public class StatusScanResult
    {
        #region [Private memebers]
        private bool _isSuccesfull       = false;
        private string _runId            = null;
        private CurrentStatusEnum _runStatus = CurrentStatusEnum.Unknown;
        private int _totalPercent        = 0;
        private int _currentStage        = 0;
        private string _stageName        = null;
        private int _currentStagePercent = 0;
        private string _stageMessage     = null;
        private string _stepMessage      = null;
        private string _details          = null;
        private string _timeStarted      = null;
        private string _timeFinished     = null;
        private int _queuePosition       = 0;
        #endregion

        #region [Public properties]

        /// <summary>
        /// Get or set service perform status
        /// </summary>
        public bool IsSuccesfull
        {
            get { return _isSuccesfull; }
            set { _isSuccesfull = value; }
        }

        /// <summary>
        /// Get or set scan identifier
        /// </summary>
        public string RunId
        {
            get { return _runId; }
            set { _runId = value; }
        }

        /// <summary>
        /// Get or set scan status
        /// </summary>
        public CurrentStatusEnum RunStatus
        {
            get { return _runStatus; }
            set { _runStatus = value; }
        }

        /// <summary>
        /// Get or set completed work status in percent
        /// </summary>
        public int TotalPercent
        {
            get { return _totalPercent; }
            set { _totalPercent = value; }
        }

        /// <summary>
        /// Get or set current stage number
        /// </summary>
        public int CurrentStage
        {
            get { return _currentStage; }
            set { _currentStage = value; }
        }

        /// <summary>
        /// Get or set current stage name
        /// </summary>
        public string StageName
        {
            get { return _stageName; }
            set { _stageName = value; }
        }

        /// <summary>
        /// Get or set current stage completed work in percent
        /// </summary>
        public int CurrentStagePercent
        {
            get { return _currentStagePercent; }
            set { _currentStagePercent = value; }
        }

        /// <summary>
        /// Get or set stage message
        /// </summary>
        public string StageMessage
        {
            get { return _stageMessage; }
            set { _stageMessage = value; }
        }

        /// <summary>
        /// Get or set step message
        /// </summary>
        public string StepMessage
        {
            get { return _stepMessage; }
            set { _stepMessage = value; }
        }

        /// <summary>
        /// Get or set scan details
        /// </summary>
        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>
        /// Get or set scan start time
        /// </summary>
        public string TimeStarted
        {
            get { return _timeStarted; }
            set { _timeStarted = value; }
        }

        /// <summary>
        /// Get or set scan end time
        /// </summary>
        public string TimeFinished
        {
            get { return _timeFinished; }
            set { _timeFinished = value; }
        }

        /// <summary>
        /// Get or set queue project position in scan status is 'Queue'
        /// </summary>
        public int QueuePosition
        {
            get { return _queuePosition; }
            set { _queuePosition = value; }
        }
        #endregion

        #region [Static methods]
        /// <summary>
        /// Convert StatusScanResult object from xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static StatusScanResult FromXml(string xml)
        {
            return XmlHelper.ParseStatusScanResult(xml);
        }

        /// <summary>
        /// Convert RunStatus object from string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static CurrentStatusEnum GetRunStatus(string xml)
        {
            CurrentStatusEnum status = CurrentStatusEnum.Unknown;

            switch(xml)
            {
                case "Finished": status = CurrentStatusEnum.Finished; break;
                case "Queued": status = CurrentStatusEnum.Queued; break;
                case "Failed": status = CurrentStatusEnum.Failed; break;
                case "Canceled": status = CurrentStatusEnum.Canceled; break;
                case "Deleted": status = CurrentStatusEnum.Deleted; break;
                case "Working": status = CurrentStatusEnum.Working; break;
            }

            return status;
        }
        #endregion
    }
}

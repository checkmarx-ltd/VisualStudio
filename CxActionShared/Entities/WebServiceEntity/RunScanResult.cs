using System;
using System.Collections.Generic;
using CxViewerAction.Helpers;

namespace CxViewerAction.Entities.WebServiceEntity
{
    /// <summary>
    /// Run scan object
    /// </summary>
    public class RunScanResult
    {
        #region [Private memebers]
        private bool _isSuccesfull = false;
        private string _scanId = null;
        private StatusScanResult _statusResult = null;
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
        public string ScanId
        {
            get { return _scanId; }
            set { _scanId = value; }
        }

        /// <summary>
        /// Get or set status scan result
        /// </summary>
        public StatusScanResult StatusResult
        {
            get { return _statusResult; }
            set { _statusResult = value; }
        }

        public long ProjectId { get; set; }
        #endregion

        #region [Static methods]

        /// <summary>
        /// Convert RunScanResult from xml
        /// </summary>
        /// <param name="xml">xml string</param>
        /// <returns></returns>
        public static RunScanResult FromXml(string xml)
        {
            return XmlHelper.ParseRunScanResult(xml);
        }

        #endregion
    }
}

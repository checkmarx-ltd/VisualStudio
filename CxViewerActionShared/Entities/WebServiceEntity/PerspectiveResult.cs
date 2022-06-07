using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction2022.Helpers;

namespace CxViewerAction2022.Entities
{
    /// <summary>
    /// Represent service perspective object
    /// </summary>
    public class PerspectiveResult
    {
        #region [Private Members]

        private bool _isSuccesfull = false;
        private string _reportUrl = string.Empty;

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
        /// Generated report URL
        /// </summary>
        public string ReportUrl
        {
            get { return _reportUrl; }
            set { _reportUrl = value; }
        }

        #endregion

        /// <summary>
        /// Convert PerspectiveResult from xml
        /// </summary>
        /// <param name="xml">xml string</param>
        /// <returns></returns>
        public static PerspectiveResult FromXml(string xml)
        {
            return XmlHelper.ParsePerspectiveResult(xml);
        }
    }
}

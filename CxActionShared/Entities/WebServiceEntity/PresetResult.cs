using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Helpers;

namespace CxViewerAction.Entities.WebServiceEntity
{
    /// <summary>
    /// Represent service preset object
    /// </summary>
    public class PresetResult
    {
        #region [Private memebers]
        private bool _isSuccesfull = false;
        private Dictionary<int, string> _presets = null;
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
        /// Get or set list of presets
        /// </summary>
        public Dictionary<int, string> Presets
        {
            get { return _presets; }
            set { _presets = value; }
        }
        #endregion

        #region [Static methods]

        /// <summary>
        /// Convert PresetResult from xml
        /// </summary>
        /// <param name="xml">xml string</param>
        /// <returns></returns>
        public static PresetResult FromXml(string xml)
        {
            return XmlHelper.ParsePresetResult(xml);
        }

        #endregion
    }
}

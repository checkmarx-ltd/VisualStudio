using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Helpers;

namespace CxViewerAction.Entities.WebServiceEntity
{
    /// <summary>
    /// Represent service configuration object
    /// </summary>
    public class ConfigurationResult
    {
        #region [Private memebers]
        private bool _isSuccesfull = false;
        private Dictionary<long, string> _configurations = null;
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
        /// Get or set configuration property list
        /// </summary>
        public Dictionary<long, string> Configurations
        {
            get { return _configurations; }
            set { _configurations = value; }
        }

        /// <summary>
        /// Get first configuration key in list
        /// </summary>
        public long FirstConfigurationKey
        {
            get 
            {
                KeyValuePair<long, string> first = new KeyValuePair<long,string>();

                foreach (KeyValuePair<long, string> key in _configurations)
                {
                    first = key;
                    break;
                }

                return first.Key; 
            }
        }
        #endregion

        #region [Static methods]

        /// <summary>
        /// Convert ConfigurationResult from xml
        /// </summary>
        /// <param name="xml">xml string</param>
        /// <returns></returns>
        public static ConfigurationResult FromXml(string xml)
        {
            return XmlHelper.ParseConfigurationResult(xml);
        }

        #endregion
    }
}

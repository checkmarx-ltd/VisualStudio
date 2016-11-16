using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Helpers;

namespace CxViewerAction.Entities.WebServiceEntity
{
    /// <summary>
    /// Represent remote service team object
    /// </summary>
    public class TeamResult
    {
        #region [Private memebers]
        /// <summary>
        /// Service perform request successfully
        /// </summary>
        private bool _isSuccesfull = false;

        /// <summary>
        /// Teams dictionary
        /// </summary>
        private Dictionary<string, string> _teams = null;
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
        /// Get or set teams dictionary
        /// </summary>
        public Dictionary<string, string> Teams
        {
            get { return _teams; }
            set { _teams = value; }
        }

        /// <summary>
        /// Get first team name
        /// </summary>
        public string FirstTeamKey
        {
            get 
            {
                KeyValuePair<string, string> fisrt = new KeyValuePair<string, string>();

                foreach (KeyValuePair<string, string> item in _teams)
                {
                    fisrt = item;
                    break;
                }

                return fisrt.Key;
            }
        }
        #endregion

        #region [Static methods]
        
        /// <summary>
        /// Convert xml to TeamResult object
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static TeamResult FromXml(string xml)
        {
            return XmlHelper.ParseTeamResult(xml);
        }
        
        #endregion
    }
}

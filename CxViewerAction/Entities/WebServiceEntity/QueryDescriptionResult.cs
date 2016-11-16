using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Helpers;
using CxViewerAction.Services;
using CxViewerAction.CxVSWebService;

namespace CxViewerAction.Entities.WebServiceEntity
{
    public class QueryDescriptionResult
    {
        #region [Private Constants]

        private const string _fileFormat = "cxQuery{0}.htm";

        #endregion

        #region [Private Members]

        private int _queryId = 0;
        private bool _isSuccesfull = false;
        private string _description = string.Empty;

        #endregion

        #region [Public Properties]

        /// <summary>
        /// Gets or sets query identifier
        /// </summary>
        public int QueryId
        {
            get { return _queryId; }
            set { _queryId = value; }
        }

        /// <summary>
        /// Gets or sets value indication that object received succesfull
        /// </summary>
        public bool IsSuccesfull
        {
            get { return _isSuccesfull; }
            set { _isSuccesfull = value; }
        }

        /// <summary>
        /// Gets or sets query description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        #region [Public Methods]

        /// <summary>
        /// Save query description object to file
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return StorageHelper.Save(Description, string.Format(_fileFormat, QueryId));
        }

        /// <summary>
        /// Read query description from file
        /// </summary>
        /// <returns></returns>
        public void Read()
        {
            Description = StorageHelper.Read(string.Format(_fileFormat, QueryId));
        }

        public string GetFileDescription()
        {
            string fileOut;
            return (StorageHelper.FileExist(string.Format(_fileFormat, QueryId), out fileOut)) ? fileOut : null;
        }

        #endregion

        #region [Static Methods]

        /// <summary>
        /// Get query object from xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static QueryDescriptionResult FromXml(string xml)
        {
            return XmlHelper.ParseQueryDescriptionResult(xml);
        }

        public static CxWSResponseQueryDescription GetById(int queryId)
        {            
            return CxWebServiceClient.GetQueryDesription(queryId);
        }

        #endregion

    }
}

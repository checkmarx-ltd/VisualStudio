using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using CxViewerAction.Entities;
using CxViewerAction.CxVSWebService;
using CxViewerAction.Entities.WebServiceEntity;
using System.IO;
using Common;

namespace CxViewerAction.Helpers
{
    public class SavedResultsManager
    {
        static readonly object lockObj = new object();
        private static SavedResultsManager instance = new SavedResultsManager();
        private static long scanId;
        private static XmlDocument xml = null;
        public const string RESULT_STATUS_NEW_TEXT = "New";
        public const string RESULT_STATUS_RECURRENT_TEXT = "Recurrent";
        public const string RESULT_STATUS_FIXED_TEXT = "Fixed";

        public const string RESULT_COMMENT_DETAILS_SEPARATOR = "]:";

        public static SavedResultsManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new SavedResultsManager();
                    }
                    return instance;
                }
            }
        }


        internal CxWSQueryVulnerabilityData[] LoadStoredScanData(long id)
        {
            if (scanId != id)
            {
                XML = null;
                scanId = id;
            }
            List<CxWSQueryVulnerabilityData> list = new List<CxWSQueryVulnerabilityData>();
            if (XML != null)
            {
                XmlNodeList queryNodes = XML.SelectNodes("/CxXMLResults/Query");
                foreach (XmlNode xmlNode in queryNodes)
                {

                    CxWSQueryVulnerabilityData query = new CxWSQueryVulnerabilityData();
                    string idValue = xmlNode.Attributes["id"].Value ?? xmlNode.Attributes["Id"].Value;
                    if (idValue != null)
                        query.QueryId = int.Parse(idValue);

                    string cweIdValue = xmlNode.Attributes["cweId"].Value ?? xmlNode.Attributes["CweId"].Value;
                    if (cweIdValue != null)
                        query.CWE = int.Parse(cweIdValue);

                    query.QueryName = xmlNode.Attributes["name"].Value ?? xmlNode.Attributes["Name"].Value;
                    query.GroupName = xmlNode.Attributes["group"].Value ?? xmlNode.Attributes["Group"].Value;
                    XmlAttribute severityAttribute = xmlNode.Attributes["severity"] ?? xmlNode.Attributes["Severity"];
                    if (severityAttribute.Value != null)
                        query.Severity = (int)ReportQueryResult.SeverityTypeFromString(severityAttribute.Value);

                    query.AmountOfResults = xmlNode.ChildNodes.Count;

                    list.Add(query);

                }
            }

            return list.ToArray();
        }

        private XmlDocument XML
        {
            get
            {
                if (xml != null)
                {
                    return xml;
                }
                else
                {
                    string xmlFile = "";
                    LoginData login = LoginHelper.LoadSaved();
                    LoginData.BindProject bindPro = login.BindedProjects.Find(delegate(LoginData.BindProject bp)
                    {
                        return bp.BindedProjectId == CommonData.ProjectId;
                    });

                    if (bindPro != null)
                    {
                        Dictionary<string, long> list = new Dictionary<string, long>();
                        ScanReportInfo item = bindPro.ScanReports.Find(delegate(ScanReportInfo sri)
                        {
                            return sri.Id == scanId;
                        });
                        if (item != null && !String.IsNullOrEmpty(item.Path))
                        {
                            xmlFile = StorageHelper.Read(item.Path);
                        }
                        if (!String.IsNullOrEmpty(xmlFile))
                        {
                            xml = new XmlDocument();
                            xml.LoadXml(xmlFile);
                        }
                    }
                    
                    return xml;
                }

            }
            set
            {
                xml = value;
            }
        }

        internal CxWSSingleResultData[] GetScanResultsForQuery(long id, long queryId)
        {
            if (scanId != id)
            {
                XML = null;
                scanId = id;
            }
            String query = "/CxXMLResults/Query[@id=" + queryId + "]";
            XmlNodeList list = XML.SelectNodes(query);

            if (list != null && list.Count > 0)
            {
                return ParseQueryItemPathResult(list[0]).ToArray();
            }

            return null;
        }

        internal static List<CxWSSingleResultData> ParseQueryItemPathResult(XmlNode xmlNode)
        {
            List<CxWSSingleResultData> outputItems = new List<CxWSSingleResultData>();

            foreach (XmlNode resultNode in xmlNode.ChildNodes)
            {
                int severity = 0;
                string assignedUser = string.Empty;
                string status = string.Empty;
                string fullSourceFileName = resultNode.FirstChild.FirstChild.ChildNodes[0].InnerText;
                string sourceFile = fullSourceFileName;
                string sourceFolder = Path.DirectorySeparatorChar.ToString();
                string fullDestinationFileName = resultNode.FirstChild.LastChild.ChildNodes[0].InnerText;
                string destinationFile = fullDestinationFileName;
                string destinationFolder = Path.DirectorySeparatorChar.ToString();
                int state = 0;
                string lineValue = resultNode.Attributes["Line"].Value ?? resultNode.Attributes["line"].Value;
                string falsePositiveValue = resultNode.Attributes["FalsePositive"].Value ?? resultNode.Attributes["falsePositive"].Value;
                string pathIdValue = resultNode.FirstChild.Attributes["PathId"].Value ?? resultNode.FirstChild.Attributes["pathId"].Value;
                try
                {
                    XmlAttribute severityAttribute = xmlNode.Attributes["severity"] ?? xmlNode.Attributes["Severity"];
                    if (severityAttribute.Value != null)
                    {
                        severity = (int)ReportQueryResult.SeverityTypeFromString(severityAttribute.Value);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }
                try
                {
                    sourceFolder = Path.GetDirectoryName(fullSourceFileName);
                    if (string.IsNullOrEmpty(sourceFolder))
                    {
                        sourceFolder = Path.DirectorySeparatorChar.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());  
                }
                try
                {
                    sourceFile = Path.GetFileName(fullSourceFileName);
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }

                try
                {
                    destinationFolder = Path.GetDirectoryName(fullDestinationFileName);
                    if (string.IsNullOrEmpty(destinationFolder))
                    {
                        destinationFolder = Path.DirectorySeparatorChar.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }
                try
                {
                    destinationFile = Path.GetFileName(fullDestinationFileName);
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }
                
                try
                {
                    assignedUser = resultNode.Attributes["AssignToUser"].Value;
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }
                try
                {
                    status = resultNode.Attributes["Status"].Value ?? resultNode.Attributes["status"].Value;
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }

                try
                {
                    state = Convert.ToInt32(resultNode.Attributes["state"].Value);
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                    try
                    {
                        state = falsePositiveValue != null ? Convert.ToInt32(Convert.ToBoolean(falsePositiveValue)) : 0;
                    }
                    catch (Exception exc)
                    {
                        Logger.Create().Error(exc.ToString());
                    }
                }


                CxWSSingleResultData reportQueryItemResult = new CxWSSingleResultData
                {
                    SourceFile = sourceFile,
                    SourceFolder = sourceFolder,
                    SourceLine = lineValue != null ? Convert.ToInt32(lineValue) : 0,
                    SourceObject = resultNode.FirstChild.FirstChild.ChildNodes[4].InnerText,
                    Comment = resultNode.Attributes["Remark"].Value ?? resultNode.Attributes["remark"].Value,
                    PathId = pathIdValue != null ? Convert.ToInt32(pathIdValue) : 0,
                    Severity = severity,
                    AssignedUser = assignedUser,
                    ResultStatus = ConvertResultStatusToEnum(status),
                    State = state,
                    DestFile = destinationFile,
                    DestFolder = destinationFolder,
                    DestLine = Convert.ToInt64(resultNode.FirstChild.LastChild.ChildNodes[1].InnerText),
                    DestObject = resultNode.FirstChild.LastChild.ChildNodes[4].InnerText,
                };

                outputItems.Add(reportQueryItemResult);

            }


            return outputItems;
        }


        internal CxWSResultPath[] GetResultPathsForQuery(long id, long queryId)
        {
            if (scanId != id)
            {
                XML = null;
                scanId = id;
            }
            String query = "/CxXMLResults/Query[@id=" + queryId + "]";
            XmlNodeList list = XML.SelectNodes(query);

            if (list != null && list.Count > 0)
            {
                return GetResultPathsForQuery(list[0]);
            }

            return null;
        }

        private CxWSResultPath[] GetResultPathsForQuery(XmlNode xmlNode)
        {
            List<CxWSResultPath> outputItems = new List<CxWSResultPath>();

            foreach (XmlNode resultNode in xmlNode.ChildNodes)
            {
                CxWSResultPath reportQueryItemResult = GetResultPath(resultNode);

                outputItems.Add(reportQueryItemResult);

            }


            return outputItems.ToArray();
        }

        private CxWSPathNode[] GetNodesForPath(XmlNode Path)
        {
            List<CxWSPathNode> outputItems = new List<CxWSPathNode>();

            if (Path.FirstChild == null)
                return outputItems.ToArray();

            foreach (XmlNode pathNode in Path.ChildNodes)
            {
                CxWSPathNode node = new CxWSPathNode();
                foreach (XmlNode pathSubNode in pathNode.ChildNodes)
                {
                    switch (pathSubNode.Name.ToLower())
                    {
                        case "filename": node.FileName = pathSubNode.InnerText; break;
                        case "line": node.Line = int.Parse(pathSubNode.InnerText); break;
                        case "column": node.Column = int.Parse(pathSubNode.InnerText); break;
                        case "name": node.Name = pathSubNode.InnerText; break;
                        case "length": node.Length = int.Parse(pathSubNode.InnerText); break;
                        case "nodeid": node.PathNodeId = int.Parse(pathSubNode.InnerText); break;
                    }
                }
                outputItems.Add(node);
            }

            return outputItems.ToArray();
        }

        internal CxWSResultPath GetResultPath(long id, long pathId)
        {
            if (scanId != id)
            {
                XML = null;
                scanId = id;
            }
            String query = "/CxXMLResults/Query/Result/Path[@PathId=" + pathId + "]";
            XmlNodeList list = XML.SelectNodes(query);
            if (list != null && list.Count > 0)
            {
                return GetResultPath(list[0].ParentNode);       
            }

            return null;
        }

        private CxWSResultPath GetResultPath(XmlNode resultNode)
        {
            int severity = 0;
            string assignedUser = "";
            int state = 0;
            string lineValue = resultNode.Attributes["Line"].Value ?? resultNode.Attributes["line"].Value;
            string falsePositiveValue = resultNode.Attributes["FalsePositive"].Value ?? resultNode.Attributes["falsePositive"].Value;
            string pathIdValue = resultNode.FirstChild.Attributes["PathId"].Value ?? resultNode.FirstChild.Attributes["pathId"].Value;
            string resultIdValue = resultNode.FirstChild.Attributes["ResultId"].Value ?? resultNode.FirstChild.Attributes["resultId"].Value;
            try
            {
                XmlAttribute severityAttribute = resultNode.Attributes["severity"] ?? resultNode.Attributes["Severity"];
                if (severityAttribute.Value != null)
                {
                    severity = (int)ReportQueryResult.SeverityTypeFromString(severityAttribute.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
            try
            {
                assignedUser = resultNode.Attributes["AssignToUser"].Value;
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }

            try
            {
                state = Convert.ToInt32(resultNode.Attributes["state"].Value);
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
                try
                {
                    state = falsePositiveValue != null ? Convert.ToInt32(Convert.ToBoolean(falsePositiveValue)) : 0;
                }
                catch (Exception exc)
                {
                    Logger.Create().Error(exc.ToString());
                }
            }

            CxWSPathNode[] nodes = GetNodesForPath(resultNode.FirstChild);

            CxWSResultPath reportQueryItemResult = new CxWSResultPath
            {
                Comment = resultNode.Attributes["Remark"].Value ?? resultNode.Attributes["remark"].Value,
                PathId = pathIdValue != null ? Convert.ToInt32(pathIdValue) : 0,
                Severity = severity,
                AssignedUser = assignedUser,
                State = state,
                SimilarityId = Convert.ToInt64(resultNode.FirstChild.Attributes["SimilarityId"].Value),
                Nodes = nodes,
            };

            return reportQueryItemResult;
        }

        public static string ConvertResultStatusToString(CompareStatusType paramState)
        {
            string result = string.Empty;
            switch (paramState)
            {
                case CompareStatusType.New:
                    result = RESULT_STATUS_NEW_TEXT;
                    break;
                case CompareStatusType.Reoccured:
                    result = RESULT_STATUS_RECURRENT_TEXT;
                    break;
                case CompareStatusType.Fixed:
                    result = RESULT_STATUS_FIXED_TEXT;
                    break;
                default:
                    result = string.Empty;
                    Logger.Create().Error("Unknown result state exists");
                    break;
            }
            return result;
        }

        public static CompareStatusType ConvertResultStatusToEnum(string paramState)
        {
            CompareStatusType result = CompareStatusType.New; //the default is a new result
            if (paramState != null)
            {
                switch (paramState)
                {
                    case RESULT_STATUS_NEW_TEXT:
                        result = CompareStatusType.New;
                        break;
                    case RESULT_STATUS_RECURRENT_TEXT:
                        result = CompareStatusType.Reoccured;
                        break;
                    case RESULT_STATUS_FIXED_TEXT:
                        result = CompareStatusType.Fixed;
                        break;
                    default:
                        Logger.Create().Error("Unknown result state exists");
                        result = CompareStatusType.New;
                        break;
                }
            }
            return result;
        }
    }
}

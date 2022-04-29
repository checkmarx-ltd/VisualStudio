using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Entities.WebServiceEntity;
using System.Xml;
using CxViewerAction.Entities;

namespace CxViewerAction.Helpers
{
    /// <summary>
    /// Class encapsulates methods for transforming XML string to system objects
    /// </summary>
    public class XmlHelper
    {

        /// <summary>
        /// Convert XML string into PresetResult object
        /// </summary>
        /// <param name="response">xml string</param>
        /// <returns></returns>
        public static PresetResult ParsePresetResult(string response)
        {
            PresetResult r = new PresetResult();
            r.Presets = new Dictionary<int, string>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "IsSuccesfull": r.IsSuccesfull = bool.Parse(xmlNode.InnerText); break;
                        case "ReturnValue":
                            foreach (XmlNode presetNode in xmlNode.ChildNodes)
                            {
                                r.Presets.Add(int.Parse(presetNode.Attributes["Id"].Value), presetNode.Attributes["Name"].Value);
                            }
                            break;
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return r;
        }

        /// <summary>
        /// Convert XML string into TeamResult object
        /// </summary>
        /// <param name="response">xml string</param>
        /// <returns></returns>
        public static TeamResult ParseTeamResult(string response)
        {
            TeamResult t = new TeamResult();
            t.Teams = new Dictionary<string, string>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "IsSuccesfull": t.IsSuccesfull = bool.Parse(xmlNode.InnerText); break;
                        case "ReturnValue":
                            foreach (XmlNode presetNode in xmlNode.ChildNodes)
                            {
                                t.Teams.Add(presetNode.Attributes["Id"].Value, presetNode.Attributes["Name"].Value);
                            }
                            break;
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return t;
        }

        /// <summary>
        /// Convert XML string into ConfigurationResult object
        /// </summary>
        /// <param name="response">xml string</param>
        /// <returns></returns>
        public static ConfigurationResult ParseConfigurationResult(string response)
        {
            ConfigurationResult r = new ConfigurationResult();
            r.Configurations = new Dictionary<long, string>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "IsSuccesfull": r.IsSuccesfull = bool.Parse(xmlNode.InnerText); break;
                        case "ReturnValue":
                            foreach (XmlNode presetNode in xmlNode.ChildNodes)
                            {
                                r.Configurations.Add(int.Parse(presetNode.Attributes["Id"].Value), presetNode.Attributes["Name"].Value);
                            }
                            break;
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return r;
        }

        /// <summary>
        /// Convert XML string into RunScanResult object
        /// </summary>
        /// <param name="response">xml string</param>
        /// <returns></returns>
        public static RunScanResult ParseRunScanResult(string response)
        {
            RunScanResult r = new RunScanResult();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "IsSuccesfull": r.IsSuccesfull = bool.Parse(xmlNode.InnerText); break;
                        case "ReturnValue": r.ScanId = xmlNode.InnerText; break;
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return r;
        }

        /// <summary>
        /// Convert XML string into StatusScanResult object
        /// </summary>
        /// <param name="response">xml string</param>
        /// <returns></returns>
        internal static StatusScanResult ParseStatusScanResult(string response)
        {
            StatusScanResult r = new StatusScanResult();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "IsSuccesfull": r.IsSuccesfull = bool.Parse(xmlNode.InnerText); break;
                        case "ReturnValue":
                            r.RunId = xmlNode.ChildNodes[0].Attributes["RunId"].Value;
                            r.RunStatus = StatusScanResult.GetRunStatus(xmlNode.ChildNodes[0].Attributes["RunStatus"].Value);
                            r.TotalPercent = int.Parse(xmlNode.ChildNodes[0].Attributes["TotalPercent"].Value);
                            r.CurrentStage = int.Parse(xmlNode.ChildNodes[0].Attributes["CurrentStage"].Value);
                            r.StageName = xmlNode.ChildNodes[0].Attributes["StageName"].Value;
                            r.CurrentStagePercent = int.Parse(xmlNode.ChildNodes[0].Attributes["CurrentStagePercent"].Value);
                            r.StageMessage = xmlNode.ChildNodes[0].Attributes["StageMessage"].Value;
                            r.StepMessage = xmlNode.ChildNodes[0].Attributes["StepMessage"].Value;
                            r.Details = xmlNode.ChildNodes[0].Attributes["Details"].Value;
                            r.TimeStarted = xmlNode.ChildNodes[0].Attributes["TimeStarted"].Value;
                            r.TimeFinished = xmlNode.ChildNodes[0].Attributes["TimeFinished"].Value;
                            r.QueuePosition = int.Parse(xmlNode.ChildNodes[0].Attributes["QueuePosition"].Value);
                            break;
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return r;
        }

        /// <summary>
        /// Convert XML string into PerspectiveResult object
        /// </summary>
        /// <param name="response">xml string</param>
        /// <returns></returns>
        internal static PerspectiveResult ParsePerspectiveResult(string response)
        {
            PerspectiveResult r = new PerspectiveResult();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "IsSuccesfull": r.IsSuccesfull = bool.Parse(xmlNode.InnerText); break;
                        case "ReturnValue": r.ReportUrl = xmlNode.InnerText; break;
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return r;
        }

        /// <summary>
        /// Convert XML string into ReportResult object
        /// </summary>
        /// <param name="response">xml string</param>
        /// <returns></returns>
        internal static ReportResult ParseReportResult(string response)
        {
            ReportResult r = new ReportResult();
            r.Problems = new List<ReportQueryResult>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    if (xmlNode.Name.ToLower() == "query")
                    {
                        ReportQueryResult query = new ReportQueryResult();
                        string idValue = xmlNode.Attributes["id"].Value ?? xmlNode.Attributes["Id"].Value;
                        if (idValue != null)
                            query.Id = int.Parse(idValue);

                        string cweIdValue = xmlNode.Attributes["cweId"].Value ?? xmlNode.Attributes["CweId"].Value;
                        if (cweIdValue != null)
                            query.CweId = int.Parse(cweIdValue);
                        
                        query.Name = xmlNode.Attributes["name"].Value ?? xmlNode.Attributes["Name"].Value;
                        query.Group = xmlNode.Attributes["group"].Value ?? xmlNode.Attributes["Group"].Value;
                        XmlAttribute severityAttribute = xmlNode.Attributes["severity"]?? xmlNode.Attributes["Severity"];
                        if (severityAttribute.Value != null)
                            query.Severity = ReportQueryResult.SeverityTypeFromString(severityAttribute.Value);
                        
                        query.Paths = ParseQueryResult(xmlNode);
                        query.Report = r;

                        query.Paths.ForEach(delegate(ReportQueryItemResult p) { p.Query = query; });
                        r.Problems.Add(query);
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return r;
        }

        /// <summary>
        /// Convert XML string into ReportQueryItemPathResult list
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private static List<ReportQueryItemResult> ParseQueryResult(XmlNode xmlNode)
        {
            List<ReportQueryItemResult> outputItems = new List<ReportQueryItemResult>();

            if (xmlNode.FirstChild == null)
                return outputItems;
            foreach (XmlNode resultNode in xmlNode.ChildNodes)
            {
                string lineValue = resultNode.Attributes["Line"].Value ?? resultNode.Attributes["line"].Value;
                string columnValue = resultNode.Attributes["Column"].Value ?? resultNode.Attributes["column"].Value;
                string nodeIdValue = resultNode.Attributes["NodeId"].Value ?? resultNode.Attributes["nodeId"].Value;
                string falsePositiveValue = resultNode.Attributes["FalsePositive"].Value ?? resultNode.Attributes["falsePositive"].Value;
                string pathIdValue = resultNode.FirstChild.Attributes["PathId"].Value ?? resultNode.FirstChild.Attributes["pathId"].Value;
                string resultIdValue = resultNode.FirstChild.Attributes["ResultId"].Value ?? resultNode.FirstChild.Attributes["resultId"].Value;
                ReportQueryItemResult reportQueryItemResult = new ReportQueryItemResult
                                                                  {
                                                                      FileName = resultNode.Attributes["FileName"].Value ?? resultNode.Attributes["fileName"].Value,
                                                                      Line = lineValue != null ? Convert.ToInt32(lineValue) : 0,
                                                                      Column = columnValue != null ? Convert.ToInt32(columnValue) : 0,
                                                                      NodeId = nodeIdValue != null ? Convert.ToInt32(nodeIdValue) : 0,
                                                                      FalsePositive = falsePositiveValue != null ? Convert.ToBoolean(falsePositiveValue) : false,
                                                                      Remark = resultNode.Attributes["Remark"].Value ?? resultNode.Attributes["remark"].Value,
                                                                      PathId = pathIdValue != null ? Convert.ToInt32(pathIdValue) : 0,
                                                                      ResultId = resultIdValue != null ? Convert.ToInt32(resultIdValue) : 0
                                                                  };
                reportQueryItemResult.Paths = ParseQueryItemPathResult(resultNode.FirstChild, reportQueryItemResult);                
                if (outputItems.Find(r => r.CompareTo(reportQueryItemResult) == 0) == null)
                    outputItems.Add(reportQueryItemResult);
            }

            return outputItems;
        }

        internal static List<ReportQueryItemPathResult> ParseQueryItemPathResult(XmlNode xmlNode, ReportQueryItemResult parent)
        {
            List<ReportQueryItemPathResult> outputItems = new List<ReportQueryItemPathResult>();


            foreach (XmlNode pathNode in xmlNode.ChildNodes)
            {
                ReportQueryItemPathResult reportQueryItemPathResult = new ReportQueryItemPathResult();
                reportQueryItemPathResult.QueryItem = parent;
                foreach (XmlNode pathSubNode in pathNode.ChildNodes)
                {
                    switch (pathSubNode.Name.ToLower())
                    {
                        case "filename": reportQueryItemPathResult.FileName = pathSubNode.InnerText; break;
                        case "line": reportQueryItemPathResult.Line = int.Parse(pathSubNode.InnerText); break;
                        case "column": reportQueryItemPathResult.Column = int.Parse(pathSubNode.InnerText); break;
                        case "name": reportQueryItemPathResult.Name = pathSubNode.InnerText; break;
                        case "length": reportQueryItemPathResult.Length = int.Parse(pathSubNode.InnerText); break;
                        case "nodeid": reportQueryItemPathResult.NodeId = int.Parse(pathSubNode.InnerText); break;
                    }
                }
                outputItems.Add(reportQueryItemPathResult);
            }


            return outputItems;
        }

        /// <summary>
        /// Convert XML string into QueryDescriptionResult object
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static QueryDescriptionResult ParseQueryDescriptionResult(string response)
        {
            QueryDescriptionResult r = new QueryDescriptionResult();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);

                foreach (XmlNode xmlNode in xmlDoc.ChildNodes[1].ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "IsSuccesfull": r.IsSuccesfull = bool.Parse(xmlNode.InnerText); break;
                        case "ReturnValue": r.Description = xmlNode.InnerText; break;
                    }
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }

            return r;
        }
    }
}

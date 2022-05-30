using System;
using CxViewerAction2022.Helpers;
using System.Collections.Generic;

namespace CxViewerAction2022.Entities.WebServiceEntity
{
    /// <summary>
    /// Represent report result object
    /// </summary>
    public class ReportResult
    {
        #region [Private members]

        List<ReportQueryResult> _problems = new List<ReportQueryResult>();
        Dictionary<ReportQuerySeverityType, List<ReportQueryResult>> _tree;

        #endregion


        /// <summary>
        /// Get or set list detected source problems
        /// </summary>
        public List<ReportQueryResult> Problems
        {
            get { return _problems; }
            set { _problems = value; }
        }

        /// <summary>
        /// Get tree list of problems where top level are severity types with personal report list
        /// </summary>
        public Dictionary<ReportQuerySeverityType, List<ReportQueryResult>> Tree
        {
            get
            {
                if (_tree != null)
                    return _tree;

                Dictionary<ReportQuerySeverityType, List<ReportQueryResult>> tepmTree = new Dictionary<ReportQuerySeverityType, List<ReportQueryResult>>();

                _tree = new Dictionary<ReportQuerySeverityType, List<ReportQueryResult>>();
                _problems.Sort(new ReportQueryResultComparer());

                foreach (ReportQueryResult problem in _problems)
                {
                    if (!tepmTree.ContainsKey(problem.Severity))
                        tepmTree.Add(problem.Severity, new List<ReportQueryResult>());

                    tepmTree[problem.Severity].Add(problem);
                }

                if (tepmTree.ContainsKey(ReportQuerySeverityType.High))
                {
                    _tree.Add(ReportQuerySeverityType.High, tepmTree[ReportQuerySeverityType.High]);
                }

                if (tepmTree.ContainsKey(ReportQuerySeverityType.Medium))
                {
                    _tree.Add(ReportQuerySeverityType.Medium, tepmTree[ReportQuerySeverityType.Medium]);
                }

                if (tepmTree.ContainsKey(ReportQuerySeverityType.Low))
                {
                    _tree.Add(ReportQuerySeverityType.Low, tepmTree[ReportQuerySeverityType.Low]);
                }

                if (tepmTree.ContainsKey(ReportQuerySeverityType.Information))
                {
                    _tree.Add(ReportQuerySeverityType.Information, tepmTree[ReportQuerySeverityType.Information]);
                }

                if (tepmTree.ContainsKey(ReportQuerySeverityType.None))
                {
                    _tree.Add(ReportQuerySeverityType.None, tepmTree[ReportQuerySeverityType.None]);
                }

                return _tree;
            }
            set { _tree = value; }
        }

        /// <summary>
        /// Convert ReportResult object from xml
        /// </summary>
        /// <param name="xml">xml string</param>
        /// <returns></returns>
        public static ReportResult FromXml(string xml)
        {
            return XmlHelper.ParseReportResult(xml);
        }
    }
}

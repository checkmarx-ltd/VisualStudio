using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Views.DockedView;

namespace CxViewerAction.Entities.WebServiceEntity
{
    public enum ReportQuerySeverityType
    {
        None = -1,
        Information = 0,
        Low         = 1,
        Medium      = 2,
        High        = 3
    }

    public enum ViewerTreeNodeType
    {
        Root = 1,
        QueryGroup = 2,
        Query = 3,
        Uknown = 10000
    }

    public class TreeNodeData : EventArgs
    {
        public TreeNodeData(ViewerTreeNodeType type, int id, string name, long scanId, ReportQuerySeverityType severity, ReportQueryResult queryResult)
        {
            Type = type;
            Id = id;
            Name = name;
            ScanId = scanId;
            Severity = severity;
            QueryResult = queryResult;
        }

        public long ScanId
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ViewerTreeNodeType Type
        {
            get;
            set;
        }

        public ReportQuerySeverityType Severity
        {
            get;
            set;
        }

        public ReportQueryResult QueryResult
        {
            get;
            set;
        }
    }

    public class ReportQueryItemPathResult : IPerspectiveProblemFile, IComparable
    {
        #region [Private Members]

        private string _fileName = string.Empty;
        private int _line = 0;
        private int _column = 0;
        private int _length = 0;
        private string _name = string.Empty;
        private int _nodeId = 0;
        private ReportQueryResult _query = null;
        private ReportQueryItemResult _queryItem = null;

        #endregion

        #region [Public Properties]
        /// <summary>
        /// Gets or sets query problem file name
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Gets or sets query problem line position
        /// </summary>
        public int Line
        {
            get { return _line; }
            set { _line = value; }
        }

        /// <summary>
        /// Gets or sets query problem column position
        /// </summary>
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        /// <summary>
        /// Gets or sets query problem name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets query problem length
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// Gets or sets node position
        /// </summary>
        public int NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }

        public string UniqueID
        {
            get
            {
                return string.Format("{0}_{1}_{2}_{3}", Name, Line, Column, FileName);
            }
        }

        /// <summary>
        /// Gets or sets parent element
        /// </summary>
        public ReportQueryResult Query
        {
            get { return _query; }
            set { _query = value; }
        }

        /// <summary>
        /// Gets or seta parent container
        /// </summary>
        public ReportQueryItemResult QueryItem
        {
            get { return _queryItem; }
            set { _queryItem = value; }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            IPerspectiveProblemFile r = (IPerspectiveProblemFile)obj;

            if(
                r.Line == Line && 
                r.Column == Column &&
                r.FileName == FileName && 
                r.Length == Length && 
                r.Name == Name
              )
            {
                return 0;
            }
            else
                return 1;
        }

        #endregion

        #region IPerspectiveProblemFile Members

        int currentPathIndex;
        public int CurrentPathIndex
        {
            get
            {
                return currentPathIndex;
            }
            set
            {
                currentPathIndex = value;
            }
        }

        public TreeNodeData NodeData
        {
            get;
            set;
        }
        #endregion
    }

    public class ReportQueryItemResult : IComparable
    {
        #region [Private Members]

        private ReportQueryResult _query = null;
        private int _nodeId = 0;
        private string _fileName = string.Empty;
        private int _line = 0;
        private int _column = 0;
        private List<ReportQueryItemPathResult> _paths = null;

        #endregion

        #region [Public Properties]

        /// <summary>
        /// Gets or sets query report node identifier
        /// </summary>
        public ReportQueryResult Query
        {
            get { return _query; }
            set { _query = value; }
        }

        /// <summary>
        /// Gets or sets query report node identifier
        /// </summary>
        public int NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }

        bool falsePositive;
        public bool FalsePositive
        {
            get { return falsePositive; }
            set { falsePositive = value; }
        }

        string remark = string.Empty;
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        long pathId = 0;
        public long PathId
        {
            get { return pathId; }
            set { pathId = value; }
        }

        long resultId = 0;
        public long ResultId
        {
            get { return resultId; }
            set { resultId = value; }
        }

        /// <summary>
        /// Gets or sets query problem file name
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Gets or sets query problem line position
        /// </summary>
        public int Line
        {
            get { return _line; }
            set { _line = value; }
        }

        /// <summary>
        /// Gets or sets query problem column position
        /// </summary>
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        /// <summary>
        /// Gets or sets query problem path
        /// </summary>
        public List<ReportQueryItemPathResult> Paths
        {
            get { return _paths; }
            set { _paths = value; }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            ReportQueryItemResult item = (ReportQueryItemResult)obj;

            if (item.Column == Column && 
                item.FileName == FileName && 
                item.Line == Line && 
                item.Paths.Count == Paths.Count)
            {
                int compare = 0;
                for (int i = 0; i < Paths.Count; i++)
                {
                    ReportQueryItemPathResult r1 = Paths[i];
                    ReportQueryItemPathResult r2 = item.Paths[i];

                    if (r1.CompareTo(r2) != 0)
                    {
                        compare = 1;
                        break;
                    }
                }

                return compare;
            }
            else
                return 1;
        }

        #endregion
    }

    public class ReportQueryResult
    {
        #region [Private Members]

        private int _id = 0;
        private int _cweId = 0;
        private string _name = string.Empty;
        private string _group = string.Empty;
        private ReportQuerySeverityType _severity = ReportQuerySeverityType.None;
        private List<ReportQueryItemResult> _paths = null;
        private ReportResult _report = null;

        #endregion

        #region [Public Properties]

        /// <summary>
        /// Gets or sets query identifier
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets query Cwe identifier
        /// </summary>
        public int CweId
        {
            get { return _cweId; }
            set { _cweId = value; }
        }

        /// <summary>
        /// Gets or sets query name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets query group
        /// </summary>
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        /// <summary>
        /// Gets or sets query problem importance
        /// </summary>
        public ReportQuerySeverityType Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        /// <summary>
        /// Gets or sets query report list
        /// </summary>
        public List<ReportQueryItemResult> Paths
        {
            get { return _paths; }
            set { _paths = value; }
        }

        /// <summary>
        /// Gets or sets parent element
        /// </summary>
        public ReportResult Report
        {
            get { return _report; }
            set { _report = value; }
        }

        public int AmountOfResults
        {
            get;
            set;
        }

        public long ScanId
        {
            get;
            set;
        }
        
        public long QueryVersionCode
        {
            get;
            set;
        }

        #endregion

        #region [Static methods]
        public static ReportQuerySeverityType SeverityTypeFromString(string str)
        {
            ReportQuerySeverityType type;
            switch (str)
            {
                case "Information": type = ReportQuerySeverityType.Information; break;
                case "Low": type = ReportQuerySeverityType.Low; break;
                case "Medium": type = ReportQuerySeverityType.Medium; break;
                case "High": type = ReportQuerySeverityType.High; break;
                default: type = ReportQuerySeverityType.None; break;
            }

            return type;
        }
        #endregion
    }

    public class ReportQueryResultComparer : IComparer<ReportQueryResult>
    {
        #region IComparer<ReportQueryResult> Members

        public int Compare(ReportQueryResult x, ReportQueryResult y)
        {
            return y.Severity.CompareTo(x.Severity);
        }

        #endregion
    }
}

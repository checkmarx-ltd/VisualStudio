using System;
using CxViewerAction.BaseInterfaces;
using System.Collections.Generic;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Helpers;

namespace CxViewerAction.Entities
{
    /// <summary>
    /// Ordered graph data representation
    /// </summary>
    public class Graph : IGraph
    {
        #region [Private members]

        private List<GraphPath> _paths = null;
        private ReportQuerySeverityType _severity = ReportQuerySeverityType.None;
        private int _maxRelations = 2;

        #endregion
        
        #region [Constructors]

        public Graph()
        {
        }

        public Graph(ReportQueryResult query)
        {
            IGraph graph = GraphHelper.Convert(query);

            _paths = graph.Paths;
            _severity = graph.Severity;
            _maxRelations = graph.MaxRelations;
        }

        public Graph(ReportQueryItemResult queryItem)
        {
            _paths = new List<GraphPath>();
            _paths.Add((GraphPath)GraphHelper.Convert(queryItem, 0));
            _severity = queryItem.Query.Severity;
        }

        public Graph(TreeNodeData treeNode)
        {
            IGraph graph = GraphHelper.Convert(treeNode);

            _paths = graph.Paths;
            _severity = graph.Severity;
            _maxRelations = graph.MaxRelations;
        }

        
        #endregion

        #region [IGraph implementation]
        public IGraphPath Current
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get { return _paths.Count; }
        }

        public int Height
        {
            get 
            {
                int max = 0;

                foreach (IGraphPath path in _paths)
                {
                    if (path.DirectFlow.Count > max)
                        max = path.DirectFlow.Count;
                }

                return max;
            }
        }

        public List<GraphPath> Paths
        {
            get { return _paths; }
            set { _paths = value; }
        }

        public int MaxRelations
        {
            get { return _maxRelations; }
        }

        public System.Drawing.Point GetPosition(IGraphItem item)
        {
            throw new NotImplementedException();
        }

        public void AddNewPath(IGraphPath path)
        {
            if (_paths == null)
                _paths = new List<GraphPath>();

            _paths.Add((GraphPath)path);

            AddRelations();
        }
        #endregion

        #region [Private methods]

        /// <summary>
        /// Find item dependences in previous paths
        /// </summary>
        private void AddRelations()
        {
            if (_paths.Count <= 1)
                return;

            IGraphItem related = null;
            foreach (IGraphItem graphItem in _paths[_paths.Count - 1].DirectFlow)
            {
                for (int i = 0; i < _paths.Count - 1; i++)
                {
                    foreach (IGraphItem prevItem in _paths[i].DirectFlow)
                    {
                        if (prevItem.CompareTo(graphItem) == 0)
                        {
                            related = prevItem;

                            while (related.RelatedTo != null)
                                related = related.RelatedTo;

                            graphItem.IsPrimary = false;
                            graphItem.RelatedTo = related;
                            related.RelationsFrom.Add((GraphItem)graphItem);

                            if (_maxRelations < related.RelationsFrom.Count)
                                _maxRelations = related.RelationsFrom.Count;
                        }
                    }
                }
            }
        }

        #endregion

        #region IGraph Members

        public ReportQuerySeverityType Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        #endregion
    }

    /// <summary>
    /// Sequence of graph elements
    /// </summary>
    public class GraphPath : IGraphPath
    {
        private List<GraphItem> _items = null;

        public IGraphItem Current
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get { throw new NotImplementedException(); }
        }

        public List<GraphItem> DirectFlow
        {
            get { return _items; }
            set { _items = value; }
        }

        public int Top
        {
            get { return 0; }
        }

        public int Left
        {
            get { return 0; }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            GraphPath path = (GraphPath)obj;

            if (path.DirectFlow.Count == DirectFlow.Count)
            {
                bool equal = true;
                for (int i = 0; i < DirectFlow.Count; i++)
                {
                    if (path.DirectFlow[i].CompareTo(DirectFlow[i]) != 0)
                    {
                        equal = false;
                        break;
                    }
                }

                return equal ? 0 : 1;
            }
            else
                return 1;
        }

        #endregion

        #region IGraphPath Members


        ReportQueryItemResult queryItemResult;
        public ReportQueryItemResult QueryItemResult
        {
            get { return queryItemResult; }
            set { queryItemResult = value; }
        }

        #endregion
    }

    /// <summary>
    /// Grapth element
    /// </summary>
    public class GraphItem : IGraphItem
    {
        public GraphItem()
        {
            guid = Guid.NewGuid().ToString();
        }

        #region [Private members]
        private string _name = null;
        private string _fileName = null;
        private IGraphItem _relatedTo = null;
        private List<GraphItem> _relationsFrom = new List<GraphItem>();
        private IGraphPath _parent = null;
        private bool _isPrimary = true;
        private int _line = 0;
        private int _column = 0;
        private int _length = 0;
        private int _x = 0;
        private int _y = 0;
        private string guid;

        private ReportQueryItemResult _queryItem;
        #endregion

        #region [Public Properties]

        public string ID
        {
            get { return string.Format("{0}{1}{2}", Line, Column, FileName); }
        }

        public string Name
        {
            get { return _name; }
            set { _name=value; }
        }

        public IGraphItem RelatedTo
        {
            get { return _relatedTo; }
            set {_relatedTo = value; }
        }

        public List<GraphItem> RelationsFrom
        {
            get { return _relationsFrom; }
            set { _relationsFrom = value; }
        }

        public IGraphPath Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public bool IsPrimary
        {
            get { return _isPrimary; }
            set { _isPrimary = value; }
        }


        public int Line
        {
            get { return _line; }
            set { _line = value; }
        }

        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public string UniqueID
        {
            get
            {
                //return string.Format("{0}_{1}_{2}_{3}", Name, Line, Column, guid);
                return string.Format("{0}_{1}_{2}_{3}", Name, Line, Column, FileName);
            }
        }

        int currentPathIndex = 1;
        public int CurrentPathIndex
        {
            get { return currentPathIndex; }
            set { currentPathIndex = value; }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is IGraphItem)
            {
                IGraphItem compareItem = (IGraphItem)obj;

                if (compareItem.Name == Name &&
                    compareItem.Line == Line &&
                    compareItem.FileName == FileName &&
                    compareItem.Column == Column && string.Compare(((GraphItem)compareItem).ID, ID) == 0)
                {
                    return 0;
                }
                else
                    return 1;
            }
            else if (obj is ReportQueryItemPathResult)
            {
                ReportQueryItemPathResult compareItem = (ReportQueryItemPathResult)obj;

                if (compareItem.Name == Name &&
                    compareItem.FileName == FileName &&
                   compareItem.Line == Line &&
                   compareItem.Column == Column)
                {
                    return 0;
                }
                else
                    return 1;
            }
            else if (obj is CxViewerAction.CxVSWebService.CxWSPathNode)
            {
                CxViewerAction.CxVSWebService.CxWSPathNode compareItem = (CxViewerAction.CxVSWebService.CxWSPathNode)obj;

                if (compareItem.Name == Name &&
                    compareItem.FileName == FileName &&
                   compareItem.Line == Line &&
                   compareItem.Column == Column)
                {
                    return 0;
                }
                else
                    return 1;
            }

            return 1;
        }

        #endregion

        #region IGraphItem Members


        public int GraphX
        {
            get { return _x; }
            set { _x = value; }
        }

        public int GraphY
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        #region IGraphItem Members

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        #endregion

        #region IPerspectiveProblemFile Members


        public ReportQueryItemResult QueryItem
        {
            get { return _queryItem; }
            set { _queryItem = value; }
        }

        public TreeNodeData NodeData
        {
            get;
            set;
        }

        #endregion

        #region IGraphItem Members

        bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        bool isMultiReletions;
        public bool IsMultiReletions
        {
            get { return isMultiReletions; }
            set { isMultiReletions = value; }
        }

        #endregion


    }
}

using System;
using System.Windows.Forms;
using CxViewerAction.BaseInterfaces;
using CxViewerAction.Helpers;
using CxViewerAction.Views.DockedView;
using System.Drawing;
using System.Collections.Generic;
using CxViewerAction.Entities;
using System.Drawing.Drawing2D;
using Microsoft.Msagl.Drawing;
using CxViewerAction.Helpers.DrawingHelper;
using CxViewerAction.Resources;
using System.IO;
using System.Drawing.Imaging;
using P2 = Microsoft.Msagl.Point;
using GeomNode = Microsoft.Msagl.Node;
using CxViewerAction.CxVSWebService;

namespace CxViewerAction.Views.Shapes
{
    public class TableLayout : TableLayoutPanel
    {
        #region Constructor

        public TableLayout()
        {

        }

        #endregion

        #region Variables

        private IGraph _graph;
        private IGraphPath _selectedPath;
        private EventHandler _pathItemClick;
        private Node _prevSelectedNode;
        public Microsoft.Msagl.GraphViewerGdi.GViewer gLocalViewer;
        internal PointF PointF(P2 p) { return new PointF((float)p.X, (float)p.Y); }

        #endregion

        #region Properties

        public IGraph Graph
        {
            get { return _graph; }
            set { _graph = value; }
        }

        public IGraphPath SelectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value; }
        }

        public EventHandler PathItemClick
        {
            get { return _pathItemClick; }
            set { _pathItemClick = value; }
        }

        #endregion

        #region GViewer logic
        public void SetGViewer(Microsoft.Msagl.GraphViewerGdi.GViewer gViewer)
        {
            gLocalViewer = gViewer;
            gLocalViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(gViewer_MouseClick);
        }

        void gViewer_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (gLocalViewer.SelectedObject != null)
                {
                    if (gLocalViewer.SelectedObject is Microsoft.Msagl.Drawing.Node)
                    {
                        SelectNodeGraph();
                    }
                    else
                    {
                        if (gLocalViewer.SelectedObject is Microsoft.Msagl.Drawing.Edge)
                        {
                            SelectEdgeGraph();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Common.Logger.Create().Error(err.ToString());
            }
        }

        void SelectNodeGraph()
        {
            // TODO: select nodes in selected Path
            Node selectedNode = gLocalViewer.SelectedObject as Node;
            if (selectedNode != null)
            {
                if (_prevSelectedNode != null)
                {
                    IGraphItem prevItem = _prevSelectedNode.UserData as GraphItem;
                    if (prevItem != null)
                    {
                        prevItem.IsSelected = false;
                        _prevSelectedNode.UserData = prevItem;
                    }
                }

                GraphItem item = selectedNode.UserData as GraphItem;

                ColorButton.ColorButton nodeButton = new ColorButton.ColorButton();
                nodeButton.Text = selectedNode.Id;
                nodeButton.Anchor = AnchorStyles.Top;
                nodeButton.Tag = item;
                _selectedPath = null;
                if (item.CurrentPathIndex > 0)
                    DrawingHelper.SelectedPathItemUniqueID = item.QueryItem.Paths[item.CurrentPathIndex - 1].UniqueID;
                DrawingHelper.SelectedNodeUniqueID = item.UniqueID;
                DrawingHelper.isEdgeSelected = false;
                //ChangeSelectedFile(nodeButton, null);
                if (_pathItemClick != null)
                {
                    _pathItemClick(item.QueryItem.Paths[item.CurrentPathIndex - 1], null);
                }
                gLocalViewer.Refresh();
                gLocalViewer.ResumeLayout();
                gLocalViewer.Update();
                //_prevSelectedNode = selectedNode;
            }
        }

        void SelectEdgeGraph()
        {
            try
            {
                // Fix problem with redrawing selected edge.(msagl bug)                  
                gLocalViewer.PanButtonPressed = true;
                Point pt = Cursor.Position;
                pt.X += 120;
                pt.Y += 100;
                Cursor.Position = pt;
                
                // TODO: select nodes in selected Path
                Edge selectedEdge = gLocalViewer.SelectedObject as Edge;
                if (selectedEdge != null)
                {
                    selectedEdge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                    selectedEdge.Attr.LineWidth = 2;
                    selectedEdge.Attr.Weight = 2;

                    Node sourceNode = selectedEdge.SourceNode as Node;
                    Node selectedNode = sourceNode;
                    Node targetNode = selectedEdge.TargetNode as Node;
                    selectedNode = targetNode;

                    GraphItem item = targetNode.UserData as GraphItem;
                    if (item == null)
                        return;

                    GraphItem itemSource = sourceNode.UserData as GraphItem;

                    if (itemSource.CurrentPathIndex > 0)
                        DrawingHelper.SelectedPathItem1UniqueID = itemSource.QueryItem.Paths[itemSource.CurrentPathIndex - 1].UniqueID;

                    if (item.CurrentPathIndex > 0)
                        DrawingHelper.SelectedPathItem2UniqueID = item.QueryItem.Paths[item.CurrentPathIndex - 1].UniqueID;
                    DrawingHelper.SelectedNodeUniqueID = item.UniqueID;
                    DrawingHelper.isEdgeSelected = true;
                    ColorButton.ColorButton nodeButton = new ColorButton.ColorButton();
                    nodeButton.Text = selectedNode.Id;
                    nodeButton.Anchor = AnchorStyles.Top;
                    nodeButton.Tag = item;
                    _selectedPath = selectedEdge.UserData as IGraphPath;
                    //ChangeSelectedFile(nodeButton, null);
                    if (_pathItemClick != null)
                    {
                        _pathItemClick(item.QueryItem.Paths[item.CurrentPathIndex - 1], null);
                    }
                    gLocalViewer.Refresh();
                    gLocalViewer.ResumeLayout();
                    gLocalViewer.Update();
                    _prevSelectedNode = selectedNode;
                }
            }
            finally
            {
                gLocalViewer.PanButtonPressed = false;
            }
        }

        public void SelectEdgeGraphByPath(GraphItem itemSource, GraphItem item, IGraphPath selectedPath)
        {                       
            if (itemSource.CurrentPathIndex > 0)
                DrawingHelper.SelectedPathItem1UniqueID = itemSource.UniqueID;

            if (item.CurrentPathIndex > 0)
                DrawingHelper.SelectedPathItem2UniqueID = item.UniqueID;
            DrawingHelper.SelectedNodeUniqueID = item.UniqueID;
            DrawingHelper.isEdgeSelected = true;
            ColorButton.ColorButton nodeButton = new ColorButton.ColorButton();
            nodeButton.Text = itemSource.ID;
            nodeButton.Anchor = AnchorStyles.Top;
            nodeButton.Tag = item;
            _selectedPath = selectedPath;
            gLocalViewer.Refresh();
            gLocalViewer.ResumeLayout();
            gLocalViewer.Update();
        }

        public IGraphPath FindPath(CxWSResultPath queryItem)
        {
            if (_graph == null || _graph.Paths == null)
                return null;
            //gLocalViewer.Graph.Edges.Clear();
            //gLocalViewer.Graph.NodeMap.Clear();
            foreach (IGraphPath path in _graph.Paths)
            {
                if (path.DirectFlow.Count == queryItem.Nodes.Length)
                {
                    bool isFound = true;
                    for (int i = 0; i < path.DirectFlow.Count; i++)
                    {
                        IGraphItem item = path.DirectFlow[i];
                        CxWSPathNode pathItem = queryItem.Nodes[i];

                        if (item.Name != pathItem.Name || item.Line != pathItem.Line || item.Column != pathItem.Column)
                        {
                            isFound = false;
                            break;
                        }
                    }

                    if (isFound)
                        return path;
                } // Check in cases when path contain 1 element, graph contain 2
                else if (path.DirectFlow.Count == 2 && queryItem.Nodes.Length == 1)
                {
                    IGraphItem item1 = path.DirectFlow[0];
                    IGraphItem item2 = path.DirectFlow[1];
                    CxWSPathNode pathItem = queryItem.Nodes[0];

                    if (item1.CompareTo(item2) == 0 && item1.CompareTo(pathItem) == 0)
                        return path;
                }
                else if (path.DirectFlow.Count == 2 && queryItem.Nodes.Length > 2)
                {
                    GraphItem item1 = path.DirectFlow[0];
                    GraphItem item2 = path.DirectFlow[1];

                    CxWSPathNode pathItem1 = queryItem.Nodes[0];
                    CxWSPathNode pathItem2 = queryItem.Nodes[queryItem.Nodes.Length - 1];
                    if (item1.CompareTo(pathItem1) == 0 && item2.CompareTo(pathItem2) == 0)
                        return path;
                }
            }

            return null;
        }

        public IGraphPath FindPath(Entities.WebServiceEntity.ReportQueryItemResult queryItem)
        {
            if (_graph == null || _graph.Paths == null)
                return null;
            //gLocalViewer.Graph.Edges.Clear();
            //gLocalViewer.Graph.NodeMap.Clear();
            foreach (IGraphPath path in _graph.Paths)
            {
                if (path.DirectFlow.Count == queryItem.Paths.Count)
                {
                    bool isFound = true;
                    for (int i = 0; i < path.DirectFlow.Count; i++)
                    {
                        IGraphItem item = path.DirectFlow[i];
                        Entities.WebServiceEntity.ReportQueryItemPathResult pathItem = queryItem.Paths[i];

                        if (item.Name != pathItem.Name || item.Line != pathItem.Line || item.Column != pathItem.Column)
                        {
                            isFound = false;
                            break;
                        }
                    }

                    if (isFound)
                        return path;
                } // Check in cases when path contain 1 element, graph contain 2
                else if (path.DirectFlow.Count == 2 && queryItem.Paths.Count == 1)
                {
                    IGraphItem item1 = path.DirectFlow[0];
                    IGraphItem item2 = path.DirectFlow[1];
                    Entities.WebServiceEntity.ReportQueryItemPathResult pathItem = queryItem.Paths[0];

                    if (item1.CompareTo(item2) == 0 && item1.CompareTo(pathItem) == 0)
                        return path;
                }
                else if (path.DirectFlow.Count == 2 && queryItem.Paths.Count > 2)
                {
                    GraphItem item1 = path.DirectFlow[0];
                    GraphItem item2 = path.DirectFlow[1];

                    Entities.WebServiceEntity.ReportQueryItemPathResult pathItem1 = queryItem.Paths[0];
                    Entities.WebServiceEntity.ReportQueryItemPathResult pathItem2 = queryItem.Paths[queryItem.Paths.Count - 1];
                    if (item1.CompareTo(pathItem1) == 0 && item2.CompareTo(pathItem2) == 0)
                        return path;
                }
            }

            return null;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Execute graph drawing
        /// </summary>
        public void DrawGraph()
        {
            for (int j = 0; j < _graph.Paths.Count; j++)
            {
                IGraphPath path = _graph.Paths[j];
                DrawGraphPath(path);
            }

        }

        /// <summary>
        /// Generate sequence of controls for graph path
        /// </summary>
        /// <param name="path"></param>
        private void DrawGraphPath(IGraphPath path)
        {
            foreach (GraphItem item in path.DirectFlow)
            {
                DrawGraphItem(item);
            }
        }

        /// <summary>
        /// Generate sequence of controls for graph path item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="leftLevel"></param>
        /// <param name="topLevel"></param>
        private void DrawGraphItem(GraphItem item)
        {
            if (item.IsPrimary)
            {
                #region [Verify that element is currently selected]
                bool selected = item.Parent == _selectedPath;

                // or one of related to current element is in selected state
                if (!selected)
                {
                    foreach (GraphItem i in item.RelationsFrom)
                    {
                        if (i.Parent == _selectedPath)
                        {
                            break;
                        }
                    }
                }
                item.IsSelected = selected;
                #endregion

                if (item.RelationsFrom.Count >= _graph.MaxRelations)
                {
                    item.IsMultiReletions = true;
                }
            }
        }

        public void DrawConnectors()
        {
            if (_graph == null)
                return;

            try
            {
                DrawingHelper.maxReletions = 2;
                DrawingHelper.IsBuilding = true;

                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph();
                if (gLocalViewer.Graph != null)
                    graph = gLocalViewer.Graph;

                graph.Attr.NodeSeparation = 100;
                graph.Attr.Margin = 100;

                foreach (IGraphPath path in _graph.Paths)
                    DrawPathConnections(path, false, graph);

                DrawPathConnections(_selectedPath, true, graph);

                DrawingHelper.SetColorOfMultiReletionNodes(graph);
                
                DrawGraph(graph);
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
            }
            finally
            {
                DrawingHelper.IsBuilding = false;
            }
        }

        void DrawGraph(Microsoft.Msagl.Drawing.Graph graph)
        {
            if (gLocalViewer.InvokeRequired)
            {
                gLocalViewer.Invoke(new MethodInvoker(delegate() { DrawGraph(graph); }));
                return;
            }
            try
            {
                gLocalViewer.Graph = graph;
            }
            catch (Exception err)
            {
                Common.Logger.Create().Error(err.ToString());
            }
        }

        private void DrawPathConnections(IGraphPath path, bool isSelected, Microsoft.Msagl.Drawing.Graph graph)
        {
            if (path == null || path.DirectFlow.Count <= 1)
                return;

            graph.Attr.MinNodeHeight = 40;
            graph.Attr.MinNodeWidth = 110;

            GraphItem prev = path.DirectFlow[0];
            GraphItem next = path.DirectFlow[1];

            if (prev.RelatedTo != null)
                prev = (GraphItem)prev.RelatedTo;

            if (next.RelatedTo != null)
                next = (GraphItem)next.RelatedTo;

            if (prev == null || next == null)
                return;

            DrawingHelper.DrawArrow(gLocalViewer, prev, next, isSelected, graph, path);

        }

        #endregion
    }
}

using System;
using System.Windows.Forms;
using System.Drawing;
using CxViewerAction.Views.Shapes;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.BaseInterfaces;
using CxViewerAction.Entities;
using CxViewerAction.Views.DockedView;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using P2 = Microsoft.Msagl.Point;
using GeomNode = Microsoft.Msagl.Node;
using System.Reflection;
using CxViewerAction.Resources;
using System.IO;
using System.Drawing.Imaging;

namespace CxViewerAction.Helpers.DrawingHelper
{
    public class DrawingHelper
    {
        #region Properties

        private static readonly Pen _regularPen = new Pen(System.Drawing.Color.Gray, 1);
        private static readonly Pen _selectedPen = new Pen(System.Drawing.Color.Black, 2);
        private static readonly Pen _maxRelationsPen = new Pen(System.Drawing.Color.Red, 2);
        public static bool IsBuilding;
        static internal PointF PointF(P2 p) { return new PointF((float)p.X, (float)p.Y); }
        public static string SelectedPathItemUniqueID;
        public static string SelectedNodeUniqueID;

        public static string SelectedPathItem1UniqueID;
        public static string SelectedPathItem2UniqueID;
        public static bool isEdgeSelected;
        public static int maxReletions = 0;
        #endregion

        #region Draw Arrows

        public static void DrawArrow(GViewer gLocalViewer, GraphItem nodeId1, GraphItem nodeId2, bool isSelected, Microsoft.Msagl.Drawing.Graph graph, IGraphPath path)
        {
            ReportQueryItemPathResult prevItem = null;
            int index = 1;
            bool isSelectedEdgeDrawn = false;
            foreach (ReportQueryItemPathResult item in nodeId1.QueryItem.Paths)
            {
                Node sourceNode = graph.FindNode(item.UniqueID);
                if (sourceNode == null)
                {
                    sourceNode = new Node(item.UniqueID);
                    if (item.Name.Length > 12)
                    {
                        sourceNode.Label.Text = string.Format("{0}...", item.Name.Substring(0, 6));
                    }
                    else
                    {
                        sourceNode.Label.Text = item.Name;
                    }

                    graph.AddNode(sourceNode);
                }
                GraphItem newItem = new GraphItem();
                newItem.Column = nodeId1.Column;
                newItem.CurrentPathIndex = item.NodeId;
                newItem.FileName = nodeId1.FileName;
                newItem.GraphX = nodeId1.GraphX;
                newItem.GraphY = nodeId1.GraphY;
                newItem.IsMultiReletions = nodeId1.IsMultiReletions;
                newItem.IsPrimary = nodeId1.IsPrimary;
                newItem.IsSelected = nodeId1.IsSelected;
                newItem.Length = nodeId1.Length;
                newItem.Line = nodeId1.Line;
                newItem.Name = nodeId1.Name;
                newItem.Parent = nodeId1.Parent;
                newItem.QueryItem = nodeId1.QueryItem;
                newItem.RelatedTo = nodeId1.RelatedTo;
                newItem.RelationsFrom = nodeId1.RelationsFrom;
                sourceNode.UserData = newItem;


                if (isSelected && item.UniqueID == DrawingHelper.SelectedPathItemUniqueID && !isEdgeSelected)
                {
                    newItem.IsSelected = true;
                    SetDrawDelegateByNode(sourceNode, NodeTypes.NormalSelected);
                }
                else
                {
                    newItem.IsSelected = false;
                    SetDrawDelegateByNode(sourceNode, NodeTypes.Normal);
                }

                if (prevItem != null)
                {
                    Edge edge = IsEdgeExisted(sourceNode, prevItem.UniqueID);
                    if (edge == null)
                    {
                        edge = graph.AddEdge(prevItem.UniqueID, item.UniqueID);
                        edge.Attr.ArrowheadAtTarget = ArrowStyle.Normal;
                        edge.Attr.ArrowheadLength = 10;
                        edge.Attr.LineWidth = 2;
                        edge.Attr.Weight = 2;
                        path.QueryItemResult = nodeId1.QueryItem;
                        edge.UserData = path;                        
                    }
                    SetMaxReletions(edge);

                    if (isSelected &&
                        ((SelectedNodeUniqueID == nodeId1.UniqueID && !isEdgeSelected) ||
                        (IsContainPath(nodeId1.QueryItem.Paths) && isEdgeSelected) ||
                        (DrawingHelper.SelectedNodeUniqueID == null)
                        ))
                    {
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                        edge.Attr.Weight = 2;
                        edge.Attr.LineWidth = 2;
                        isSelectedEdgeDrawn = true;
                    }
                    else
                    {
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.DarkGray;
                        edge.Attr.Weight = 2;
                        edge.Attr.LineWidth = 2;
                    }

                }
                prevItem = item;
                index++;
            }

            prevItem = null;
            index = 1;
            foreach (ReportQueryItemPathResult item in nodeId2.QueryItem.Paths)
            {
                Node sourceNode = graph.FindNode(item.UniqueID);
                if (sourceNode == null)
                {
                    sourceNode = new Node(item.UniqueID);
                    if (item.Name.Length > 12)
                    {
                        sourceNode.Label.Text = string.Format("{0}...", item.Name.Substring(0, 6));
                    }
                    else
                    {
                        sourceNode.Label.Text = item.Name;
                    }
                    graph.AddNode(sourceNode);
                }

                GraphItem newItem = new GraphItem();
                newItem.Column = nodeId2.Column;
                newItem.CurrentPathIndex = item.NodeId;
                newItem.FileName = nodeId2.FileName;
                newItem.GraphX = nodeId2.GraphX;
                newItem.GraphY = nodeId2.GraphY;
                newItem.IsMultiReletions = nodeId2.IsMultiReletions;
                newItem.IsPrimary = nodeId2.IsPrimary;
                newItem.IsSelected = nodeId2.IsSelected;
                newItem.Length = nodeId2.Length;
                newItem.Line = nodeId2.Line;
                newItem.Name = nodeId2.Name;
                newItem.Parent = nodeId2.Parent;
                newItem.QueryItem = nodeId2.QueryItem;
                newItem.RelatedTo = nodeId2.RelatedTo;
                newItem.RelationsFrom = nodeId2.RelationsFrom;
                sourceNode.UserData = newItem;

                if (isSelected && item.UniqueID == DrawingHelper.SelectedPathItemUniqueID && !isEdgeSelected)
                {
                    newItem.IsSelected = true;
                    SetDrawDelegateByNode(sourceNode, NodeTypes.NormalSelected);
                }
                else
                {
                    newItem.IsSelected = false;
                    SetDrawDelegateByNode(sourceNode, NodeTypes.Normal);
                }

                if (prevItem != null)
                {
                    Edge edge = IsEdgeExisted(sourceNode, prevItem.UniqueID);
                    if (edge == null)
                    {
                        edge = graph.AddEdge(prevItem.UniqueID, item.UniqueID);
                        edge.Attr.ArrowheadAtTarget = ArrowStyle.Normal;
                        edge.Attr.ArrowheadLength = 10;
                        path.QueryItemResult = nodeId2.QueryItem;
                        edge.UserData = path;                        
                    }
                    SetMaxReletions(edge);

                    if (isSelected &&
                        ((SelectedNodeUniqueID == nodeId2.UniqueID && !isEdgeSelected) ||
                        (IsContainPath(nodeId2.QueryItem.Paths) && isEdgeSelected) ||
                        (DrawingHelper.SelectedNodeUniqueID == null)
                        ))
                    {
                        if (!isSelectedEdgeDrawn || isEdgeSelected)
                        {
                            edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                            edge.Attr.Weight = 2;
                            edge.Attr.LineWidth = 2;
                        }
                    }
                    else
                    {
                        //if (!isSelectedEdgeDrawn || !isEdgeSelected)
                        if (!isSelectedEdgeDrawn)
                        {
                            edge.Attr.Color = Microsoft.Msagl.Drawing.Color.DarkGray;
                            edge.Attr.Weight = 2;
                            edge.Attr.LineWidth = 2;
                        }
                    }
                }
                prevItem = item;
                index++;
            }


            bool isTopNodeFound = false;

            if (isSelected &&
                   ((IsContainPath(nodeId1.QueryItem.Paths) && isEdgeSelected) ||
                   (DrawingHelper.SelectedNodeUniqueID == null))
                  )
            {
                Node firstNode = graph.FindNode(nodeId1.QueryItem.Paths[0].UniqueID);
                IGraphItem item = firstNode.UserData as IGraphItem;
                item.IsSelected = true;
                SetDrawDelegateByNode(firstNode, NodeTypes.NormalSelected);

                isTopNodeFound = true;

                SelectedNodeUniqueID = nodeId1.UniqueID;
            }

            if (!isTopNodeFound && isSelected &&
                ((IsContainPath(nodeId2.QueryItem.Paths) && isEdgeSelected) ||
                (DrawingHelper.SelectedNodeUniqueID == null))
                )
            {
                Node firstNode = graph.FindNode(nodeId2.QueryItem.Paths[0].UniqueID);
                IGraphItem item = firstNode.UserData as IGraphItem;
                item.IsSelected = true;
                SetDrawDelegateByNode(firstNode, NodeTypes.NormalSelected);

                SelectedNodeUniqueID = nodeId2.UniqueID;
            }
        }

        public static void SetColorOfMultiReletionNodes(Microsoft.Msagl.Drawing.Graph graph)
        {
            foreach (Edge item in graph.Edges)
            {
                List<Edge> list = new List<Edge>(item.SourceNode.Edges);

                if (maxReletions == list.Count && maxReletions != 2)
                {
                    IGraphItem graphItem = item.SourceNode.UserData as IGraphItem;
                    if (graphItem.IsSelected)
                        SetDrawDelegateByNode(item.SourceNode, NodeTypes.MultiRelaitionsSelected);
                    else
                        SetDrawDelegateByNode(item.SourceNode, NodeTypes.MultiRelaitions);
                }

                list = new List<Edge>(item.TargetNode.Edges);

                if (maxReletions == list.Count && maxReletions != 2)
                {
                    IGraphItem graphItem = item.TargetNode.UserData as IGraphItem;
                    if (graphItem.IsSelected)
                        SetDrawDelegateByNode(item.TargetNode, NodeTypes.MultiRelaitionsSelected);
                    else
                        SetDrawDelegateByNode(item.TargetNode, NodeTypes.MultiRelaitions);
                }
            }
        }

        static void SetMaxReletions(Edge edge)
        {
            List<Edge> list = new List<Edge>(edge.SourceNode.Edges);
            if (maxReletions < list.Count)
            {
                maxReletions = list.Count;
            }

            list = new List<Edge>(edge.TargetNode.Edges);
            if (maxReletions < list.Count)
            {
                maxReletions = list.Count;
            }
        }

        static bool IsContainPath(IEnumerable<ReportQueryItemPathResult> list)
        {
            int foundIndex = 0;
            foreach (ReportQueryItemPathResult item in list)
            {
                if (item.UniqueID == SelectedPathItem1UniqueID || item.UniqueID == SelectedPathItem2UniqueID)
                {
                    foundIndex++;
                }
            }

            if (foundIndex == 2)
                return true;
            return false;
        }

        static Edge IsEdgeExisted(Node node, string node2Id)
        {
            foreach (Edge item in node.Edges)
            {
                if ((item.Source == node.Id && item.Target == node2Id) ||
                    (item.Source == node2Id && item.Target == node.Id)
                    )
                {
                    return item;
                }
            }
            return null;
        }

        public static void _DrawArrow(GViewer gLocalViewer, GraphItem nodeId1, GraphItem nodeId2, bool isSelected, Microsoft.Msagl.Drawing.Graph graph, IGraphPath path)
        {
            #region Set node 1 caption

            bool isNewEdge = false;
            //Microsoft.Msagl.Drawing.Graph graph = gLocalViewer.Graph;
            Node sourceNode = graph.FindNode(nodeId1.UniqueID);
            if (sourceNode == null)
            {
                isNewEdge = true;
                sourceNode = new Node(nodeId1.UniqueID);
                if (nodeId1.Name.Length > 12)
                {
                    sourceNode.Label.Text = string.Format("{0}...", nodeId1.Name.Substring(0, 6));
                }
                else
                {
                    sourceNode.Label.Text = nodeId1.Name;
                }

            }

            #endregion

            #region Set node 2 caption

            Node targetNode = graph.FindNode(nodeId2.UniqueID);
            if (targetNode == null)
            {
                isNewEdge = true;
                targetNode = new Node(nodeId2.UniqueID);
                if (nodeId2.Name.Length > 12)
                {
                    targetNode.Label.Text = string.Format("{0}...", nodeId2.Name.Substring(0, 6));
                }
                else
                {
                    targetNode.Label.Text = nodeId2.Name;
                }
            }

            #endregion

            #region Set Drawing type

            GraphItem nodeItem1 = sourceNode.UserData as GraphItem;
            GraphItem nodeItem2 = targetNode.UserData as GraphItem;

            sourceNode.UserData = nodeId1;
            targetNode.UserData = nodeId2;
            #endregion

            if (isNewEdge)
            {
                graph.AddNode(sourceNode);
                graph.AddNode(targetNode);
                if (nodeId1.UniqueID != nodeId2.UniqueID)
                {
                    Edge edge = graph.AddEdge(nodeId1.UniqueID, nodeId2.UniqueID);
                    edge.UserData = path;
                }
            }
            else
            {
                if (!IsNodesConnected(sourceNode, targetNode))
                {
                    if (nodeId1.UniqueID != nodeId2.UniqueID)
                    {
                        Edge edge = graph.AddEdge(nodeId1.UniqueID, nodeId2.UniqueID);
                        edge.UserData = path;
                    }
                }
            }

            SetNodeColor(graph, nodeId1, nodeId2, sourceNode, targetNode, isSelected);

        }

        static bool IsNodesConnected(Node sourceNode, Node targetNode)
        {
            foreach (Edge edg in sourceNode.Edges)
            {
                if (edg.SourceNode.UserData != null &&
                    edg.TargetNode.UserData != null &&
                    (edg.SourceNode.UserData as GraphItem).UniqueID != (targetNode.UserData as GraphItem).UniqueID &&
                    (edg.TargetNode.UserData as GraphItem).UniqueID != (targetNode.UserData as GraphItem).UniqueID)
                {
                    foreach (Edge edgT in targetNode.Edges)
                    {
                        if (edgT.SourceNode.UserData != null &&
                            edgT.TargetNode.UserData != null &&
                            (edgT.SourceNode.UserData as GraphItem).UniqueID != (sourceNode.UserData as GraphItem).UniqueID &&
                            (edgT.TargetNode.UserData as GraphItem).UniqueID != (sourceNode.UserData as GraphItem).UniqueID)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static void SetEdgeByNodes(Microsoft.Msagl.Drawing.Graph graph, string sourceId, string targetId)
        {
            foreach (Edge edge in graph.Edges)
            {
                if (edge.Source == sourceId && edge.Target == targetId)
                {
                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                    edge.Attr.Weight = 2;
                    edge.Attr.LineWidth = 2;
                }
                else
                {
                    if (edge.Source == targetId && edge.Target == sourceId)
                    {
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                        edge.Attr.Weight = 2;
                        edge.Attr.LineWidth = 2;
                    }
                    else
                    {
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.LightGray;
                        edge.Attr.Weight = 1;
                        edge.Attr.LineWidth = 1;
                    }
                }
            }
        }

        public static void SetNodeColor(Microsoft.Msagl.Drawing.Graph graph, GraphItem nodeId1, GraphItem nodeId2, Node sourceNode, Node targetNode, bool isSelected)
        {
            if (nodeId1 != null && nodeId2 != null && isSelected)
            {
                if (nodeId1.IsMultiReletions)
                    SetDrawDelegateByNode(sourceNode, NodeTypes.MultiRelaitionsSelected);
                else
                    SetDrawDelegateByNode(sourceNode, NodeTypes.NormalSelected);

                if (nodeId2.IsMultiReletions)
                    SetDrawDelegateByNode(targetNode, NodeTypes.MultiRelaitionsSelected);
                else
                    SetDrawDelegateByNode(targetNode, NodeTypes.NormalSelected);
            }
            else
            {
                if (nodeId1 != null && nodeId1.IsMultiReletions)
                    SetDrawDelegateByNode(sourceNode, NodeTypes.MultiRelaitions);
                else
                    SetDrawDelegateByNode(sourceNode, NodeTypes.Normal);

                if (nodeId2 != null && nodeId2.IsMultiReletions)
                    SetDrawDelegateByNode(targetNode, NodeTypes.MultiRelaitions);
                else
                    SetDrawDelegateByNode(targetNode, NodeTypes.Normal);
            }

            if (nodeId1 != null && nodeId2 != null)
                SetEdgeByNodes(graph, nodeId1.UniqueID, nodeId2.UniqueID);
        }

        #endregion

        #region Draw Nodes

        public static void DrawNode(GViewer gLocalViewer, GraphItem item, bool selected, bool maxRelations)
        {
            if (gLocalViewer.InvokeRequired)
            {
                gLocalViewer.Invoke(new MethodInvoker(delegate() { DrawNode(gLocalViewer, item, selected, maxRelations); }));
                return;
            }

            Microsoft.Msagl.Drawing.Graph graph = gLocalViewer.Graph;
            Node node = new Node(item.UniqueID);
            if (item.Name.Length > 12)
            {
                node.Label.Text = string.Format("{0}...", item.Name.Substring(0, 6));
            }
            else
            {
                node.Label.Text = item.Name;
            }
            node.UserData = item;

            if (selected || maxRelations)
            {
                if (selected && maxRelations)
                {
                    node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawMultiRelaitionsSelectedNode);
                }
                else
                {
                    if (selected)
                    {
                        node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawNormalSelectedNode);
                    }
                    else
                    {
                        if (maxRelations)
                        {
                            node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawMultiRelaitionsNode);
                        }
                        else
                        {
                            node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawNormalNode);
                        }
                    }
                }
            }
            else
            {
                node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawNormalNode);
            }
            graph.AddNode(node);
            gLocalViewer.Graph = graph;
        }

        private static bool CustomDrawNormalNode(Microsoft.Msagl.Drawing.Node node, object graphics)
        {
            return CustomDrawNode(node, graphics, NodeTypes.Normal);
        }

        private static bool CustomDrawMultiRelaitionsSelectedNode(Microsoft.Msagl.Drawing.Node node, object graphics)
        {
            return CustomDrawNode(node, graphics, NodeTypes.MultiRelaitionsSelected);
        }

        private static bool CustomDrawMultiRelaitionsNode(Microsoft.Msagl.Drawing.Node node, object graphics)
        {
            return CustomDrawNode(node, graphics, NodeTypes.MultiRelaitions);
        }

        private static bool CustomDrawNormalSelectedNode(Microsoft.Msagl.Drawing.Node node, object graphics)
        {
            return CustomDrawNode(node, graphics, NodeTypes.NormalSelected);
        }

        private static bool CustomDrawNode(Microsoft.Msagl.Drawing.Node node, object graphics, NodeTypes nodeType)
        {
            try
            {
                double width = 110;
                double height = 40;

                Microsoft.Msagl.GeometryGraph geomGraph = new Microsoft.Msagl.GeometryGraph();
                GeomNode geomCreek = new Microsoft.Msagl.Node(node.Id, Microsoft.Msagl.Splines.CurveFactory.CreateBox(width, height, 0, 0, node.Attr.GeometryNode.Center));
                node.Attr.GeometryNode.BoundaryCurve = Microsoft.Msagl.Splines.CurveFactory.CreateBox(width, height, 0, 0,
                    node.Attr.GeometryNode.Center);


                Graphics g = (Graphics)graphics;

                MemoryStream ms = new MemoryStream();

                ///<summary>
                ///Changes for plug-513 unable to see scan results
                ///</summary>
                var assemName = Assembly.GetCallingAssembly().GetName();

                switch (nodeType)
                {
                    case NodeTypes.Normal:
                        if (assemName.Name.Equals("CxViewerAction"))
                        {
                            CxViewerResources.NormalNode.Save(ms, ImageFormat.Png);
                        }
                        else
                        {
                            CxViewerResources2019.NormalNode.Save(ms, ImageFormat.Png);
                        }
                        break;
                    case NodeTypes.NormalSelected:
                        if (assemName.Name.Equals("CxViewerAction"))
                        {
                            CxViewerResources.NormalSelected.Save(ms, ImageFormat.Png);
                        }
                        else
                        {
                            CxViewerResources2019.NormalSelected.Save(ms, ImageFormat.Png);
                        }
                        break;
                    case NodeTypes.MultiRelaitions:
                        if (assemName.Name.Equals("CxViewerAction"))
                        {
                            CxViewerResources.MultiRelaitions.Save(ms, ImageFormat.Png);
                        }
                        else
                        {
                            CxViewerResources2019.MultiRelaitions.Save(ms, ImageFormat.Png);
                        }
                        break;
                    case NodeTypes.MultiRelaitionsSelected:
                        if (assemName.Name.Equals("CxViewerAction"))
                        {
                            CxViewerResources.MultiRelaitionsSelected.Save(ms, ImageFormat.Png);
                        }
                        else
                        {
                            CxViewerResources2019.MultiRelaitionsSelected.Save(ms, ImageFormat.Png);
                        }
                        break;
                }
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                //flip the image around its center
                using (System.Drawing.Drawing2D.Matrix m = g.Transform)
                {
                    using (System.Drawing.Drawing2D.Matrix saveM = m.Clone())
                    {
                        float c = (float)node.Attr.GeometryNode.Center.Y;

                        using (System.Drawing.Drawing2D.Matrix m2 = new System.Drawing.Drawing2D.Matrix(1, 0, 0, -1, 0, 2 * c))
                            m.Multiply(m2);

                        g.Transform = m;

                        g.SetClip(FillTheGraphicsPath(node.Attr.GeometryNode.BoundaryCurve));


                        g.DrawImage(image, new PointF((float)(node.Attr.GeometryNode.Center.X - node.Attr.GeometryNode.Width / 2), (float)(node.Attr.GeometryNode.Center.Y - node.Attr.GeometryNode.Height / 2)));

                        Font myFont = new System.Drawing.Font("Helvetica", 10, System.Drawing.FontStyle.Italic);
                        System.Drawing.Brush myBrush = new SolidBrush(System.Drawing.Color.Blue);

                        Rectangle rectString = new Rectangle(Convert.ToInt32(node.Attr.GeometryNode.Center.X - node.Attr.GeometryNode.Width / 2), Convert.ToInt32(node.Attr.GeometryNode.Center.Y - node.Attr.GeometryNode.Height / 2), Convert.ToInt32(width), Convert.ToInt32(height));
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        g.DrawString(node.Label.Text, myFont, myBrush,
                            rectString,
                            stringFormat
                            // new PointF((float)(node.Attr.GeometryNode.Center.X - node.Attr.GeometryNode.Width / 2), (float)(node.Attr.GeometryNode.Center.Y - 5))
                            );

                        g.Transform = saveM;

                    }
                }
            }
            catch (Exception err)
            {
                Common.Logger.Create().Error(err.ToString());
            }

            return true;//returning false would enable the default rendering
        }

        public static void SetDrawDelegateByNode(Microsoft.Msagl.Drawing.Node node, NodeTypes nodeType)
        {
            node.DrawNodeDelegate = null;
            switch (nodeType)
            {
                case NodeTypes.Normal:
                    node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawNormalNode);
                    break;
                case NodeTypes.NormalSelected:
                    node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawNormalSelectedNode);
                    break;
                case NodeTypes.MultiRelaitions:
                    node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawMultiRelaitionsNode);
                    break;
                case NodeTypes.MultiRelaitionsSelected:
                    node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(CustomDrawMultiRelaitionsSelectedNode);
                    break;
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath FillTheGraphicsPath(Microsoft.Msagl.Splines.ICurve iCurve)
        {
            Microsoft.Msagl.Splines.Curve curve = iCurve as Microsoft.Msagl.Splines.Curve;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            foreach (Microsoft.Msagl.Splines.ICurve seg in curve.Segments)
                AddSegmentToPath(seg, ref path);
            return path;
        }

        private static void AddSegmentToPath(Microsoft.Msagl.Splines.ICurve seg, ref System.Drawing.Drawing2D.GraphicsPath p)
        {
            const float radiansToDegrees = (float)(180.0 / Math.PI);
            Microsoft.Msagl.Splines.LineSegment line = seg as Microsoft.Msagl.Splines.LineSegment;
            if (line != null)
                p.AddLine(PointF(line.Start), PointF(line.End));
            else
            {
                Microsoft.Msagl.Splines.CubicBezierSegment cb = seg as Microsoft.Msagl.Splines.CubicBezierSegment;
                if (cb != null)
                    p.AddBezier(PointF(cb.B(0)), PointF(cb.B(1)), PointF(cb.B(2)), PointF(cb.B(3)));
                else
                {
                    Microsoft.Msagl.Splines.Ellipse ellipse = seg as Microsoft.Msagl.Splines.Ellipse;
                    if (ellipse != null)
                        p.AddArc((float)(ellipse.Center.X - ellipse.AxisA.Length), (float)(ellipse.Center.Y - ellipse.AxisB.Length),
                            (float)(2 * ellipse.AxisA.Length), (float)(2 * ellipse.AxisB.Length), (float)(ellipse.ParStart * radiansToDegrees),
                            (float)((ellipse.ParEnd - ellipse.ParStart) * radiansToDegrees));

                }
            }
        }

        #endregion

    }
}

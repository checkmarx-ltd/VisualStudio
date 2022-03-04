using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CxViewerAction.BaseInterfaces;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;
using CxViewerAction.Views.Shapes;
using Microsoft.Msagl.GraphViewerGdi;
using CxViewerAction.CxVSWebService;

namespace CxViewerAction.Views.DockedView
{
    /// <summary>
    /// Graph view
    /// </summary>
    public partial class PerspectiveGraphCtrl : UserControl, IPerspectiveGraphView
    {
        #region [Delegates]
        delegate void UpdateDelegate();
        #endregion

        #region [Private Members]

        /// <summary>
        /// Used for restoring scroll X position after rebinding
        /// </summary>
        private int _scroll_pos_x = 0;

        /// <summary>
        /// Used for restoring scroll Y position after rebinding
        /// </summary>

        private int _scroll_pos_y = 0;

        #endregion

        #region [Public Properties]

        public IGraphPath SelectedPath
        {
            get { return tblLayout.SelectedPath; }
            set { tblLayout.SelectedPath = value; }
        }

        public int Scroll_pos_x
        {
            get { return _scroll_pos_x; }
        }

        public int Scroll_pos_y
        {
            get { return _scroll_pos_y; }
        }

        /// <summary>
        /// Gets or sets graph object
        /// </summary>
        public IGraph Graph
        {
            get { return tblLayout.Graph; }
            set { tblLayout.Graph = value; }
        }

        public GViewer MsGalViewer;

        public EventHandler PathItemClick
        {
            get { return tblLayout.PathItemClick; }
            set { tblLayout.PathItemClick = value; }
        }

        #endregion

        #region [Constructors]

        public PerspectiveGraphCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer, true);

            this.SetStyle(ControlStyles.Selectable, false);
            gViewer1.Dock = DockStyle.Fill;
            ResumeLayout();
            gViewer1.LayoutAlgorithmSettingsButtonVisible = true;
            gViewer1.AutoScroll = true;
            gViewer1.AutoSize = false;
            gViewer1.AutoSizeMode = AutoSizeMode.GrowOnly;
            gViewer1.BackColor = Color.LightGray;
            //gViewer1.ClientSize;
            gViewer1.Dock = DockStyle.Fill;
            gViewer1.FitGraphBoundingBox();
            gViewer1.ToolBarIsVisible = true;
            gViewer1.ZoomF = 1;            
            tblLayout.SetGViewer(gViewer1);
            MsGalViewer = gViewer1;           
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            graph.Attr.NodeSeparation = 100;
            graph.Attr.Margin = 100;

            try            
            {             
                graph.GeometryGraph = new Microsoft.Msagl.GeometryGraph();
                graph.GeometryGraph.SimpleStretch = false;
                graph.GeometryGraph.AspectRatio = 1;
                graph.GeometryGraph.CalculateLayout();
            }
            catch(Exception err)
            {
                Common.Logger.Create().Error(err.ToString());
            }
            //graph.Attr.BackgroundColor = Microsoft.Msagl.Drawing.Color.LightGray;
            tblLayout.gLocalViewer.Graph = graph;
            tblLayout.gLocalViewer.Size = new Size(100, 100);
        }

        #endregion

        #region [Public Methods]

        public void ClearGraphView()
        {
            if (tblLayout != null && tblLayout.gLocalViewer != null && tblLayout.gLocalViewer.Graph != null)
            {
                tblLayout.gLocalViewer.Graph.Edges.Clear();
                tblLayout.gLocalViewer.Graph.NodeMap.Clear();
            }
        }

        public void SetZoom()
        {
            if (tblLayout != null && tblLayout.gLocalViewer != null && tblLayout.gLocalViewer.ClientSize != null && tblLayout.gLocalViewer.Graph.NodeCount == 1)
            {
                //tblLayout.gLocalViewer.ZoomF = 1;
                double zoomRatioHeight = tblLayout.gLocalViewer.GraphHeight / tblLayout.gLocalViewer.ClientSize.Height;
                double zoomRatioWidth = tblLayout.gLocalViewer.GraphWidth / tblLayout.gLocalViewer.ClientSize.Width;
                if (zoomRatioHeight > zoomRatioWidth)
                    tblLayout.gLocalViewer.ZoomF = zoomRatioHeight;
                else
                    tblLayout.gLocalViewer.ZoomF = zoomRatioWidth;
            }
        }

        /// <summary>
        /// Bind object to view representation
        /// </summary>
       
        public void BindData()
        {
            if (tblLayout.Graph == null)
                return;
           
            DrawGraph();
           
        }

        private void DrawGraph()
        {
            tblLayout.DrawGraph();
            tblLayout.DrawConnectors();
        }

        #endregion

        #region [Private Methods]


        #endregion

        public IGraphPath FindPath(CxWSResultPath queryItem)
        {
            return tblLayout.FindPath(queryItem);
        }

        public IGraphPath FindPath(Entities.WebServiceEntity.ReportQueryItemResult queryItem)
        {
            return tblLayout.FindPath(queryItem);
        }


        public void SelectEdgeGraphByPath(GraphItem itemSource, GraphItem item, IGraphPath selectedPath)
        {
            tblLayout.SelectEdgeGraphByPath(itemSource, item, selectedPath);
        }

        private void PerspectiveGraphCtrl_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                _scroll_pos_x = e.NewValue;
            else
                _scroll_pos_y = e.NewValue;
        }

        private void UpdateScrollPosition()
        {
            HorizontalScroll.Value = _scroll_pos_x;
            VerticalScroll.Value = _scroll_pos_y;

            //OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, _scroll_pos_x, ScrollOrientation.HorizontalScroll));
            //OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, _scroll_pos_y, ScrollOrientation.VerticalScroll));

        }

        private void PerspectiveGraphCtrl_Resize(object sender, EventArgs e)
        {            
            //gViewer1.Size = this.Size;
        }

        private void PerspectiveGraphCtrl_SizeChanged(object sender, EventArgs e)
        {
            //gViewer1.Size = this.Size;
        }
    }
}

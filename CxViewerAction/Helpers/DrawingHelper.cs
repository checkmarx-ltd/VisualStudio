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

namespace CxViewerAction.Helpers
{
    public class DirectLine
    {
        public NodeButton button;
        public GraphicsPath Path;
    }

    public class DrawingHelper
    {
        private static readonly Pen _regularPen = new Pen(System.Drawing.Color.Gray, 1);
        private static readonly Pen _selectedPen = new Pen(System.Drawing.Color.Black, 2);
        private static readonly Pen _maxRelationsPen = new Pen(System.Drawing.Color.Red, 2);
        private static readonly int _arrowSizeX = 3;
        private static readonly int _arrowSizeY = 7;
        public static List<DirectLine> gPathList = new List<DirectLine>();
        public static bool IsBuilding;

        public static void DrawArrow(Graphics g, Control startControl, Control endControl, Point distance, TableLayout tableLayout)
        {
            if (startControl == null || endControl == null)
                return;

            NodeButton btnStart = (NodeButton)startControl;
            NodeButton btnEnd = (NodeButton)endControl;

            Point point1 = GetMiddlePoint(startControl, true);
            Point point2 = GetMiddlePoint(endControl, false);
            Pen drawPen = btnStart.Selected && btnEnd.Selected ? _selectedPen : _regularPen;

            GraphicsPath gPath = new GraphicsPath();
            gPath.AddLine(point1, point2);
            g.FillPath(drawPen.Brush, gPath);
            g.DrawPath(drawPen, gPath);
            DirectLine dLine = new DirectLine();
            dLine.Path = gPath;
            //if (((btnEnd.Tag as IGraphItem).Parent as IGraphPath).DirectFlow[0].ID != (btnStart.Tag as GraphItem).ID)
            //{
                NodeButton btn = new NodeButton();
                btn.Text = btnStart.Text;
                btn.TableLayoutInstance = tableLayout;
                btn.StartNodeItem = btnStart.Tag as GraphItem;
                btn.EndNodeItem = btnEnd.Tag as GraphItem;
                btn.Tag = GetNodeByStartEndItems(tableLayout.Graph.Paths, btnStart.Tag as GraphItem, btnEnd.Tag as GraphItem); //btnStart.Tag;            
                btn.Click += tableLayout.ChangeSelectedFileHandler;
                btn.Click += tableLayout.PathItemClick;
                dLine.button = btn;
            //}
            //else
            //{
            //    dLine.button = btnEnd;
            //}
            gPathList.Add(dLine);

            DrawArrow(g, drawPen, point2);
        }

        static GraphItem GetNodeByStartEndItems(List<GraphPath> paths, GraphItem startItem, GraphItem endItem)
        {
            foreach(GraphPath item in paths)
            {
                if (item.DirectFlow[0].ID == startItem.ID && item.DirectFlow[1].ID == endItem.ID)
                {
                    return item.DirectFlow[1];
                }
            }
            return null;
        }

        private static void DrawPath(Graphics g, Pen drawPen, Point point1, Point point2)
        {
            Point middlePoint = new Point(point1.X, point1.Y + (int)Math.Floor((double)(point2.Y - point1.Y) / 2));

            g.DrawLines(drawPen, new Point[] { point1, middlePoint, new Point(point2.X, middlePoint.Y), point2 });
            DrawArrow(g, drawPen, point2);
        }

        private static Point GetMiddlePoint(Control ctrl, bool start)
        {
            Point loc = ctrl.Location;

            return new Point(loc.X + (int)Math.Floor((decimal)(ctrl.Width / 2)), loc.Y + (start ? ctrl.Height : 0) - (start ? 0 : _arrowSizeY + 5));
        }

        private static void DrawArrow(Graphics g, Pen pen, Point start)
        {
            Point end = new Point(start.X, start.Y + _arrowSizeY);
            g.DrawLine(pen, start, end);

            end = new Point(start.X, start.Y + _arrowSizeY + 5);
            g.FillPolygon(new SolidBrush(pen.Color), new Point[] { end, new Point(end.X - _arrowSizeX, end.Y - _arrowSizeY),
                    new Point(end.X + _arrowSizeX + 1, end.Y - _arrowSizeY)});
        }

        internal static void DrawNode(Graphics g, Control c, bool selected, bool maxRelations)
        {
            if (selected)
                g.DrawRectangle(_selectedPen, c.Bounds);

            if (maxRelations)
            {
                c.ForeColor = Color.Red;
                g.DrawRectangle(_maxRelationsPen, c.Bounds);
            }
        }
    }
}

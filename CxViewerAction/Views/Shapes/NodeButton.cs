using System;
using System.Windows.Forms;
using System.Drawing;
using CxViewerAction.Entities;

namespace CxViewerAction.Views.Shapes
{
    /// <summary>
    /// Represent custom button control to place inside graph control
    /// </summary>
    public class NodeButton : Button
    {
        #region [Private members]
        private bool _selected = false;
        private TableLayout _tableLayoutInstance;
        GraphItem _startNodeItem;
        GraphItem _endNodeItem;

        #endregion

        #region [Public Properties]

        public GraphItem StartNodeItem
        {
            get { return _startNodeItem; }
            set { _startNodeItem = value; }
        }

        public GraphItem EndNodeItem
        {
            get { return _endNodeItem; }
            set { _endNodeItem = value; }
        }

        public TableLayout TableLayoutInstance
        {
            get { return _tableLayoutInstance; }
            set { _tableLayoutInstance = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        #endregion

        #region [Constructors]

        public NodeButton()
            : base()
        {
            Initialize();
        }

        public NodeButton(string buttonText, bool selected)
            : base()
        {
            Initialize();

            _selected = selected;

            Text = buttonText;
        }

        #endregion

        #region [Public Methods]
        #endregion

        #region [Private Methods]

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        private void Initialize()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);               
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.White;

            Width = 300;
            Height = 40;
        }

        #endregion
    }
}

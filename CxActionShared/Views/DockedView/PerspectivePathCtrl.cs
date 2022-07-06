using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.Entities.WebServiceEntity;
using System.Resources;
using System.Reflection;
using CxViewerAction.Helpers.DrawingHelper;

namespace CxViewerAction.Views.DockedView
{
    /// <summary>
    /// User control to visualize problem path flow
    /// </summary>
    public partial class PerspectivePathCtrl : UserControl, IPerspectivePathView
    {
        #region [Private Members]
        /// <summary>
        /// Path click handler
        /// </summary>
        private EventHandler _pathButtonClickHandler = null;

        /// <summary>
        /// Problem flow paths
        /// </summary>
        private ReportQueryItemResult _queryItemResult = null;

        ColorButton.ColorButton selectedBtnPath = null;

        #endregion

        #region [Public Properties]
        /// <summary>
        /// Gets or sets problem flow paths
        /// </summary>
        public ReportQueryItemResult QueryItemResult
        {
            get { return _queryItemResult; }
            set { _queryItemResult = value; }
        }

        /// <summary>
        /// Sets handler for path button click event
        /// </summary>
        public EventHandler PathButtonClickHandler
        {
            set { _pathButtonClickHandler = value; }
        }
        #endregion

        #region [Constructors]
        public PerspectivePathCtrl()
        {
            InitializeComponent();
        }
        #endregion

        #region [Public Methods]

        public void SetZoom()
        {
        }

        public void ClearView()
        {
            pnlPath.Controls.Clear();
            pnlPath.Width = 200;
        }

        /// <summary>
        /// Bind object data to form controls. Generate path sequance
        /// </summary>
        public void BindData(int index)
        {
            pnlPath.Controls.Clear();
            pnlPath.Width = 200;

            if (_queryItemResult.Paths == null)
                return;

            pnlPath.RowCount = _queryItemResult.Paths.Count;

            ///<summary>
            /// Changes for bug Plug-513 unable to see scan results
            /// </summary>
            //Start
            System.IO.Stream file;
            System.IO.Stream fileEmpty;

            var data = this.GetType().Assembly.GetManifestResourceNames();
            var assemName = this.GetType().Assembly.GetName();

            if (assemName.Name.Equals("CxViewerAction2019"))
            {
                file = Assembly.GetExecutingAssembly().GetManifestResourceStream("CxViewerAction2019.Resources.down.gif");
                fileEmpty = Assembly.GetExecutingAssembly().GetManifestResourceStream("CxViewerAction2019.Resources.empty.gif");
            }
            else
            {
                file = Assembly.GetExecutingAssembly().GetManifestResourceStream("CxViewerAction.Resources.down.gif");
                fileEmpty = Assembly.GetExecutingAssembly().GetManifestResourceStream("CxViewerAction.Resources.empty.gif");
            }
            //End

            for (int i = 0; i < _queryItemResult.Paths.Count; i++)
            {
                int row = 2 * i;
                ReportQueryItemPathResult path = _queryItemResult.Paths[i];

                ColorButton.ColorButton btnPath = new ColorButton.ColorButton();
                btnPath.ButtonStyle = ColorButton.ColorButton.ButtonStyles.Rectangle;
                btnPath.SmoothingQuality = ColorButton.ColorButton.SmoothingQualities.HighQuality;
                btnPath.HoverColorA = Color.WhiteSmoke;
                btnPath.HoverColorB = Color.WhiteSmoke;
                btnPath.Text = path.Name;
                //btnPath.Width = 100;
                //btnPath.Height = 40;
                btnPath.Anchor = AnchorStyles.Top;
                btnPath.Tag = path;
                btnPath.Click += btnPath_Click;
                btnPath.Click += _pathButtonClickHandler;
                if (index == path.NodeId)
                {
                    btnPath.IsSelected = true;
                    selectedBtnPath = btnPath;
                }

                pnlPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
                pnlPath.Controls.Add(btnPath, 0, row);
                pnlPath.RowStyles[row].Height = 40F;

                PictureBox imgDown = new PictureBox();
                imgDown.Width = 16;
                imgDown.Height = 16;
                imgDown.SizeMode = PictureBoxSizeMode.StretchImage;
                // Add arrow to all buttons instead of last
                if (i != _queryItemResult.Paths.Count - 1)
                {
                    imgDown.Image = Image.FromStream(file);
                }
                else
                {
                    //imgDown.Image = Image.FromStream(fileEmpty);
                }
                imgDown.Anchor = AnchorStyles.Top;

                pnlPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
                pnlPath.Controls.Add(imgDown, 0, row + 1);
                pnlPath.RowStyles[row + 1].Height = 20F;
            }

            this.AutoScrollMinSize = new System.Drawing.Size(200, _queryItemResult.Paths.Count * 61);
            pnlPath.Refresh();
            if (selectedBtnPath != null)
            {
                selectedBtnPath.Select();
                pnlPath.ScrollControlIntoView(selectedBtnPath);
                this.ScrollControlIntoView(selectedBtnPath);
     
            }
        }
        #endregion

        private void btnPath_Click(object sender, EventArgs e)
        {
            ReportQueryItemPathResult reportQueryItemPathResult = ((ColorButton.ColorButton)sender).Tag as ReportQueryItemPathResult;
            ColorButton.ColorButton selected = ((ColorButton.ColorButton)sender);

            if (selectedBtnPath != null)
            {
                selectedBtnPath.IsSelected = false;
                selectedBtnPath.SetNormalState();
                selectedBtnPath.Invalidate();
            }

            selected.IsSelected = true;
            pnlPath.ScrollControlIntoView(selected);
            this.ScrollControlIntoView(selected);
            selectedBtnPath = selected;
        }

    }
}

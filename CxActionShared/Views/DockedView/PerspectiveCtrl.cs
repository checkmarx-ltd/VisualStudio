using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using CxViewerAction.Entities.WebServiceEntity;
using System.Collections;
using CxViewerAction.Helpers;
using CxViewerAction.Helpers.DrawingHelper;
using CxViewerAction.Entities;

namespace CxViewerAction.Views.DockedView
{
    public partial class PerspectiveCtrl : UserControl, IPerspectiveView
    {
        #region [Delegates]
        private delegate void SetActiveDelegate(bool active, string loadingMessage);
        private delegate void AddTreeViewItemDelegate(ReportResult report);
        #endregion

        #region [Private Variables]
        private ReportQueryResult _selectedReportItem = null;
        private ReportResult _report = null;

        #endregion

        #region [Constructors]
        public PerspectiveCtrl()
        {
            InitializeComponent();
            tvPerspective.ImageList = tvImages;
        }
        #endregion

        public event Action<TreeNodeData> SelectedNodeChanged;
        public event EventHandler SelectedReportItemChanged;
        public event Action<long> SelectedScanChanged;

        #region [Public Properties]

        /// <summary>
        /// Gets or sets currently selected report problem type
        /// </summary>
        public ReportQueryResult SelectedReportItem
        {
            get
            {
                return _selectedReportItem;
            }
            set
            {
                _selectedReportItem = value;
                SelectedReportItemChanged(this, null);
            }
        }

        /// <summary>
        /// Gets or sets currently selected report
        /// </summary>
        public ReportResult Report
        {
            get
            {
                return _report;
            }
            set
            {
                _report = value;
            }
        }
        #endregion

        #region [Public Methods]

        public void UpdateTreeItemInfo()
        {
            if (tvPerspective.SelectedNode != null)
            {
                ReportQueryResult reportProblem = (ReportQueryResult)tvPerspective.SelectedNode.Tag;
                int numberOfVulnerabilities = 0;
                foreach (ReportQueryItemResult item in reportProblem.Paths)
                {
                    if (!item.FalsePositive)
                        numberOfVulnerabilities++;
                }

                string nodeText = string.Format("{0} ({1} found)", reportProblem.Name, numberOfVulnerabilities);
                tvPerspective.SelectedNode.Text = nodeText;
            }
        }

        /// <summary>
        /// Bind object data to form controld
        /// </summary>
        public void BindData()
        {
            AddTreeViewItemDelegate del1 = AddTreeViewItem;
            Invoke(del1, _report);

            SetActiveDelegate del2 = SetActive;
            Invoke(del2, new object[] { true, null });
        }

        /// <summary>
        /// Set form visiblity and show specified loading message if no active
        /// </summary>
        /// <param name="active"></param>
        /// <param name="loadingMessage"></param>
        public void SetActivity(bool active, string loadingMessage)
        {
            SetActiveDelegate del2 = SetActive;
            Invoke(del2, new object[] { active, loadingMessage });
        }

        public void SetScanList(Dictionary<string, long> scanList, long selectedValue)
        {
            if (pnlScans.InvokeRequired)
            {
                pnlScans.Invoke(new MethodInvoker(delegate() { SetScanList(scanList, selectedValue); ; }));
                return;
            }

            try
            {
                BindProjectHelper.IsSelectionBlocked = true;
                cbScans.DataSource = null;
                foreach (KeyValuePair<string, long> item in scanList)
                {
                    cbScans.Items.Add(new DictionaryEntry(item.Key, item.Value));
                }
                cbScans.DisplayMember = "Key";
                cbScans.ValueMember = "Value";
                cbScans.DataSource = cbScans.Items;
                cbScans.Refresh();
                cbScans.SelectedValue = selectedValue;

                ChangePanelStatus(true);

            }
            finally
            {
                BindProjectHelper.IsSelectionBlocked = false;
            }
        }

        void ChangePanelStatus(bool isVisible)
        {
            pnlScans.Visible = isVisible;
        }

        public void RemoveScanList()
        {
            if (pnlScans.InvokeRequired)
            {
                pnlScans.Invoke(new MethodInvoker(delegate() { RemoveScanList(); ; }));
                return;
            }

            cbScans.DataSource = null;
            cbScans.Items.Clear();
            ChangePanelStatus(false);
        }

        #endregion

        #region [Private Methods]
        /// <summary>
        /// Generating tree view perspective representation
        /// </summary>
        /// <param name="report"></param>
        private void AddTreeViewItem(ReportResult report)
        {
            tvPerspective.Nodes.Clear();

            foreach (KeyValuePair<ReportQuerySeverityType, List<ReportQueryResult>> problem in report.Tree)
            {
                TreeNode node = new TreeNode(problem.Key.ToString(), (int)problem.Key +1, (int)problem.Key +1) ;
                node.Tag = new TreeNodeData(ViewerTreeNodeType.Root, (int)problem.Key, problem.Key.ToString(), problem.Value[0].ScanId, ReportQuerySeverityType.None, null); ;

                foreach (ReportQueryResult reportProblem in problem.Value)
                {
                    TreeNode problemTypeNode = new TreeNode(string.Format("{0} ({1} found)", reportProblem.Name, reportProblem.AmountOfResults), (int)reportProblem.Severity +1, (int)reportProblem.Severity +1);
                    problemTypeNode.Tag = new TreeNodeData(ViewerTreeNodeType.Query, reportProblem.Id, reportProblem.Name, reportProblem.ScanId, reportProblem.Severity, reportProblem);
                                       
                    SortNodes(problemTypeNode.Nodes);
                    node.Nodes.Add(problemTypeNode);
                }
                SortNodes(node.Nodes);

                tvPerspective.Nodes.Add(node);

            }

            //tvPerspective.Sort();
        }

        //SortNodes is a recursive method enumerating and sorting all node levels 
        private void SortNodes(TreeNodeCollection collection)
        {
            Sort(collection);
            foreach (TreeNode node in collection)
            {
                if (node.Nodes.Count > 0)
                {
                    SortNodes(node.Nodes);
                }
            }
        }

        //The Sort method is called for each node level sorting the child nodes 
        public void Sort(TreeNodeCollection collection)
        {
            TreeNode[] nodes = new TreeNode[collection.Count];
            collection.CopyTo(nodes, 0);
            Array.Sort(nodes, new TreeNodeComparer());
            collection.Clear();
            collection.AddRange(nodes);
        }

        //The TreeNodeComparer class defines the sorting criteria 
        class TreeNodeComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                TreeNode firstNode = (TreeNode)x;
                TreeNode secondNode = (TreeNode)y;

                return firstNode.Text.ToString().CompareTo(secondNode.Text);
            }

            #endregion
        }

        /// <summary>
        /// Set form activity
        /// </summary>
        /// <param name="active"></param>
        /// <param name="loadingMessage"></param>
        private void SetActive(bool active, string loadingMessage)
        {
            if (active)
            {
                lblLoading.Visible = false;
                tvPerspective.Visible = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(loadingMessage))
                    lblLoading.Text = loadingMessage;

                lblLoading.Visible = true;
                tvPerspective.Visible = false;
            }
        }

        private void tvPerspective_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        /// <summary>
        /// Handler attached to mouse up button in tree control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvPerspective_MouseUp(object sender, MouseEventArgs e)
        {
            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = tvPerspective.GetNodeAt(p);
                if (node != null)
                {
                    // Select the node the user has clicked.
                    // The node appears selected until the menu is displayed on the screen.
                    tvPerspective.SelectedNode = node;

                    if (node.Parent != null)
                    {
                        if (node.Parent.Parent == null)
                            EnableMenuItems(1);
                        else
                            EnableMenuItems(2);
                    }
                    else
                        EnableMenuItems(0);

                    contextTreeViewMenu.Show(tvPerspective, p);
                }
            }
        }

        private void EnableMenuItems(byte level)
        {
            contextTreeViewMenu.Items[0].Enabled = true;
            contextTreeViewMenu.Items[1].Enabled = true;
            contextTreeViewMenu.Items[2].Enabled = true;

            if (level == 0)
            {
                contextTreeViewMenu.Items[0].Enabled = false;
            }
            else if (level == 1)
            {

            }
            else if (level == 2)
            {
                
            }
        }

        private void menuItemShowDescription_Click(object sender, EventArgs e)
        {
            object tag = tvPerspective.SelectedNode.Tag;

            if (tag is TreeNodeData)
            {
                int queryId = ((TreeNodeData)tag).QueryResult.Id;
                string queryName = ((TreeNodeData)tag).QueryResult.Name;
                long queryVersionCode = ((TreeNodeData)tag).QueryResult.QueryVersionCode;
                QueryDescriptionEventArg eventArgs = new QueryDescriptionEventArg(queryId, queryName, queryVersionCode);
                SelectedReportItemChanged(this, eventArgs);
            }
        }
        
        private void menuItemExpand_Click(object sender, EventArgs e)
        {
            tvPerspective.ExpandAll();
        }

        private void menuItemCollapse_Click(object sender, EventArgs e)
        {
            tvPerspective.CollapseAll();
        }

        private void tvPerspective_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Node.Parent != null && e.Button == MouseButtons.Left)
            {
                TreeNode tNode = e.Node;
                TreeViewHitTestInfo hit = tvPerspective.HitTest(e.Location);
                DrawingHelper.SelectedNodeUniqueID = null;
                DrawingHelper.isEdgeSelected = false;
                SelectedNodeChanged((TreeNodeData)tNode.Tag);
                
            }
        }

        #endregion

        private void cbScans_SelectedValueChanged(object sender, EventArgs e)
        {
            if (SelectedScanChanged != null && cbScans.SelectedIndex > -1 && !BindProjectHelper.IsSelectionBlocked)
            {
                SelectedScanChanged(Convert.ToInt64(cbScans.SelectedValue));
            }
        }
    }

    public class QueryDescriptionEventArg : EventArgs
    {
        #region Properties

        public int QueryId { get; set; }

        public string QueryName { get; set; }

        public long QueryVersionCode { get; set; }

        #endregion

        #region Ctor

        public QueryDescriptionEventArg(int queryId, string queryName, long queryVersionCode)
        {
            QueryId = queryId;
            QueryName = queryName;
            QueryVersionCode = queryVersionCode;
        }

        #endregion
    }
}

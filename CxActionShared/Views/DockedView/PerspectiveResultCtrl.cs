using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Common;
using CxViewerAction.Entities.WebServiceEntity;
using System.Collections;
using CxViewerAction.Helpers;
using CxViewerAction.CxVSWebService;
using CxViewerAction.Services;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using CxViewerAction.ValueObjects;
using System.Reflection;
using CxViewerAction.Entities;

namespace CxViewerAction.Views.DockedView
{
    public partial class PerspectiveResultCtrl : UserControl, IPerspectiveResultView
    {
        #region [Delegates]
        private delegate void SetActiveDelegate(bool active, string loadingMessage);
        private delegate void AddTreeViewItemDelegate(ReportResult report);
        #endregion

        #region [Private Variables]
        private TreeNodeData nodeData = null;
        private IPerspectiveView perspectiveView = null;
        private ReportQueryResult _selectedReportItem = null;
        private ReportResult _report = null;
        private string _apiShortDescription = "sast/scans/{0}/results/{1}/shortDescription";
        private string _apiAppSecCoachLessonsRequestData = "Queries/{0}/AppSecCoachLessonsRequestData";
        private string codeBashingTooltipMessage = "Learn more about {0} using Checkmarx’s eLearning platform";
        private const string descriptionHeader = "Codebashing";
        #endregion

        #region [Constructors]
        public PerspectiveResultCtrl()
        {
            InitializeComponent();
            dgvProjects.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
            dgvProjects.RowTemplate.Height = 30;
            try
            {
                //DgvFilterManager fm = new DgvFilterManager { DataGridView = dgvProjects };
                fillSeverityCB();
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }

        }

        private void fillComboBoxes()
        {
            if (ResultStateList != null && cbState.Items.Count == 0 && !CommonData.IsWorkingOffline)
            {
                foreach (KeyValuePair<long, string> state in ResultStateList)
                {
                    cbState.Items.Add(new ComboBoxItem(state.Key, state.Value));
                }
            }

            if (ProjectAssignUsers != null && cbAssign.Items.Count == 0)
            {
                cbAssign.Items.Add(new ComboBoxItem1("None", ""));
                foreach (AssignUser user in ProjectAssignUsers)
                {
                    cbAssign.Items.Add(new ComboBoxItem1(user.UserName, user.UserName));
                }
            }

        }
        #endregion

        #region Events
        public event EventHandler SelectedReportItemChanged;
        public event Action<long> SelectedScanChanged;
        public event EventHandler SelectedRowChanged;
        public event EventHandler Refresh;

        #endregion

        #region [Public Properties]

        bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public IPerspectiveView PerspectiveView
        {
            get
            {
                return perspectiveView;
            }
            set
            {
                perspectiveView = value;
            }
        }

        /// <summary>
        /// Gets or sets currently selected Node
        /// </summary>
        public TreeNodeData SelectedNode
        {
            get
            {
                return nodeData;
            }
            set
            {
                nodeData = value;
                SelectNode(value);
            }
        }

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

        /// <summary>
        /// Bind object data to form controld
        /// </summary>
        public void BindData()
        {
        }

        /// <summary>
        /// Set form visiblity and show specified loading message if no active
        /// </summary>
        /// <param name="active"></param>
        /// <param name="loadingMessage"></param>
        public void SetActivity(bool active, string loadingMessage)
        {
            //SetActiveDelegate del2 = SetActive;
            //Invoke(del2, new object[] { active, loadingMessage });
        }

        //void ChangePanelStatus(bool isVisible)
        //{

        //}

        #endregion

        #region [Private Methods]
        /// <summary>
        /// Generating tree view perspective representation
        /// </summary>
        /// <param name="report"></param>  

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


        private TreeNodeData currentNodedata = null;

        private void SelectNode(TreeNodeData nodeData)
        {
            fillComboBoxes();

            currentNodedata = nodeData;
            dgvProjects.DataSource = null;
            DataTable dt = new DataTable();

            DataColumn col = new DataColumn(Constants.COL_NAME_NUMBER, typeof(int));
            col.ReadOnly = true;
            dt.Columns.Add(col);
            DataColumn status = new DataColumn(Constants.COL_NAME_STATUS);
            col.ReadOnly = true;
            dt.Columns.Add(status);
            col = new DataColumn(Constants.COL_NAME_SOURCE_FOLDER);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_SOURCE_FILE_NAME);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_SOURCE_LINE, typeof(int));
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_SOURCE_OBJECT);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_DEST_FOLDER);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_DEST_FILE_NAME);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_DEST_LINE, typeof(int));
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_DEST_OBJECT);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_SHOW_PATH);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_SEVERITY);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_ASSIGN);
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn(Constants.COL_NAME_REMARK);
            col.ReadOnly = false;
            dt.Columns.Add(col);
            col = new DataColumn("ResultEntity", typeof(CxWSSingleResultData));
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn("ScanId", typeof(int));
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn("PathId", typeof(long));
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn("State", typeof(int));
            col.ReadOnly = true;
            dt.Columns.Add(col);

            CxWSSingleResultData[] results = PerspectiveHelper.GetScanResultsForQuery(nodeData.ScanId, nodeData.Id);

            int index = 1;

            foreach (CxWSSingleResultData reportQueryItemResult in results)
            {
                string resultComment = reportQueryItemResult.Comment;
                if (!string.IsNullOrEmpty(resultComment))
                {
                    string[] commentsArr = resultComment.Split(new char[] { Convert.ToChar(255) }, StringSplitOptions.RemoveEmptyEntries);
                    if (commentsArr.Length > 0)
                    {
                        resultComment = commentsArr[commentsArr.Length - 1];
                        int endMetadataIndex = resultComment.LastIndexOf(SavedResultsManager.RESULT_COMMENT_DETAILS_SEPARATOR);
                        if (endMetadataIndex > 0)
                        {
                            resultComment = resultComment.Substring(endMetadataIndex + SavedResultsManager.RESULT_COMMENT_DETAILS_SEPARATOR.Length).Trim();
                        }
                    }
                }
                else
                {
                    resultComment = string.Empty;
                }

                dt.Rows.Add(new object[] {
                                           index,
                                           SavedResultsManager.ConvertResultStatusToString(reportQueryItemResult.ResultStatus),
                                           reportQueryItemResult.SourceFolder,
                                           reportQueryItemResult.SourceFile,
                                           (int)reportQueryItemResult.SourceLine,
                                           reportQueryItemResult.SourceObject,
                                           reportQueryItemResult.DestFolder,
                                           reportQueryItemResult.DestFile,
                                           (int)reportQueryItemResult.DestLine,
                                           reportQueryItemResult.DestObject,
                                           ConvertResultState(reportQueryItemResult.State),
                                           getSeverityDescription(reportQueryItemResult.Severity),
                                           reportQueryItemResult.AssignedUser,
                                           resultComment,
                                           reportQueryItemResult,
                                           nodeData.ScanId,
                                           reportQueryItemResult.PathId,
                                           reportQueryItemResult.State
                                       });

                index++;
            }
            dgvProjects.DataSource = dt;
            dgvProjects.DataMember = dt.TableName;

            for (int i = 0; i < 11; i++)
            {
                dgvProjects.Columns[i].HeaderCell.Style.Alignment = GetCellAlignment();
                dgvProjects.Columns[i].DefaultCellStyle.Font = GetColumnFont();
            }

            foreach (CxWSSingleResultData reportQueryItemResult in results)
            {
                GetShortDescription(nodeData.ScanId, reportQueryItemResult.PathId);
                break;
            }

            if (currentNodedata != null)
            {
                label2.Text = currentNodedata.Name;
                toolTip1.SetToolTip(this.linkLabel1, string.Format(codeBashingTooltipMessage, currentNodedata.Name));
            }

            dgvProjects.Columns["checkBoxesColumn"].Frozen = true;
            dgvProjects.Columns["checkBoxesColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProjects.Columns["checkBoxesColumn"].Resizable = DataGridViewTriState.False;
            dgvProjects.Columns[Constants.COL_NAME_NUMBER].Width = 30;
            dgvProjects.Columns[Constants.COL_NAME_STATUS].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjects.Columns[Constants.COL_NAME_SOURCE_FOLDER].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjects.Columns[Constants.COL_NAME_SOURCE_FILE_NAME].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjects.Columns[Constants.COL_NAME_SOURCE_LINE].Width = 60;
            dgvProjects.Columns[Constants.COL_NAME_SOURCE_OBJECT].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjects.Columns[Constants.COL_NAME_DEST_FOLDER].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjects.Columns[Constants.COL_NAME_DEST_FILE_NAME].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjects.Columns[Constants.COL_NAME_DEST_LINE].Width = 60;
            dgvProjects.Columns[Constants.COL_NAME_DEST_OBJECT].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProjects.Columns[Constants.COL_NAME_SHOW_PATH].Width = 60;
            dgvProjects.Columns[Constants.COL_NAME_SEVERITY].Width = 60;
            dgvProjects.Columns[Constants.COL_NAME_ASSIGN].Width = 60;
            dgvProjects.Columns[Constants.COL_NAME_REMARK].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            dgvProjects.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;

            dgvProjects.Columns["ResultEntity"].Visible = false;
            dgvProjects.Columns["ScanId"].Visible = false;
            dgvProjects.Columns["PathId"].Visible = false;
            dgvProjects.Columns["State"].Visible = false;

            show_chkBox();

            UpdateGridShowPath();

        }

        private string ConvertResultState(int resultId)
        {
            if (!ResultStateList.ContainsKey(resultId))
            {
                return resultId.ToString();
            }
            return ResultStateList[resultId];
        }

        private string getSeverityDescription(int severity)
        {
            string severityDesc = "Info";

            switch (severity)
            {
                case 3:
                    severityDesc = "High";
                    break;
                case 2:
                    severityDesc = "Medium";
                    break;
                case 1:
                    severityDesc = "Low";
                    break;
                default:
                    severityDesc = "Info";
                    break;

            }

            return severityDesc;
        }

        private void fillSeverityCB()
        {
            ComboBoxItem high = new ComboBoxItem(3, "High");
            ComboBoxItem medium = new ComboBoxItem(2, "Medium");
            ComboBoxItem low = new ComboBoxItem(1, "Low");
            ComboBoxItem info = new ComboBoxItem(0, "Info");
            cbSeverity.Items.Add(high);
            cbSeverity.Items.Add(medium);
            cbSeverity.Items.Add(low);
            cbSeverity.Items.Add(info);

        }

        public void MarkRowAsSelected(long pathId)
        {
            foreach (DataGridViewRow row in dgvProjects.Rows)
            {
                if (Convert.ToInt64(row.Cells["PathId"].Value) == pathId)
                {
                    row.Selected = true;
                    dgvProjects.CurrentCell = row.Cells[0];
                }
            }
        }

        public void SelectRow()
        {
            if (dgvProjects.SelectedRows.Count > 0)
            {
                IsActive = true;
                CxWSSingleResultData reportQueryItemPathResult = dgvProjects.SelectedRows[0].Cells["ResultEntity"].Value as CxWSSingleResultData;
                int scanId = -1;
                Int32.TryParse(dgvProjects.SelectedRows[0].Cells["ScanId"].Value.ToString(), out scanId);

                if (reportQueryItemPathResult == null)
                    return;

                this.SelectedRowChanged(this, new ResultData(reportQueryItemPathResult, scanId, this.SelectedNode));
                UpdateGridShowPath();
            }
        }

        private DataGridViewContentAlignment GetCellAlignment()
        {
            return DataGridViewContentAlignment.MiddleCenter;
        }

        private Font GetColumnFont()
        {
            return new Font("Arial", 8, FontStyle.Regular);
        }

        void UpdateGridShowPath()
        {
            Font font = GetColumnFont();
            foreach (DataGridViewRow row in dgvProjects.Rows)
            {
                try
                {
                    bool isFalsePositive = false;
                    ReportQueryItemResult reportQueryItemPathResult = row.Cells["ResultEntity"].Value as ReportQueryItemResult;
                    if (reportQueryItemPathResult != null)
                    {
                        if (!reportQueryItemPathResult.FalsePositive)
                        {
                            row.DefaultCellStyle.Font = new Font(font, FontStyle.Regular);
                            row.DefaultCellStyle.ForeColor = Color.Black;
                            isFalsePositive = true;
                        }
                        else
                        {
                            row.DefaultCellStyle.Font = new Font(font, FontStyle.Strikeout);
                            row.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                    }


                    if (row.Cells["State"].Value != null && Convert.ToInt32(row.Cells["State"].Value) == 1)
                    {
                        row.DefaultCellStyle.Font = new Font(font, FontStyle.Strikeout);
                        isFalsePositive = true;
                    }

                    if (row.Cells[Constants.COL_NAME_STATUS].Value != null)
                    {
                        string rowStatus = row.Cells[Constants.COL_NAME_STATUS].Value.ToString();
                        if (rowStatus == SavedResultsManager.RESULT_STATUS_NEW_TEXT)
                        {
                            row.Cells[Constants.COL_NAME_STATUS].Style.Font = new Font(font, FontStyle.Bold);
                            row.Cells[Constants.COL_NAME_STATUS].Style.ForeColor = Color.Red;
                        }
                        else if (rowStatus == SavedResultsManager.RESULT_STATUS_RECURRENT_TEXT)
                        {
                            row.Cells[Constants.COL_NAME_STATUS].Style.Font = new Font(font, FontStyle.Bold);
                            row.Cells[Constants.COL_NAME_STATUS].Style.ForeColor = ColorTranslator.FromHtml("#FBB917");//an orange flavour
                        }
                        if (isFalsePositive)
                        {
                            row.Cells[Constants.COL_NAME_STATUS].Style.Font = new Font(font, FontStyle.Bold | FontStyle.Strikeout);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }

            }
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            try
            {
                if (dgvProjects.SelectedRows.Count > 0)
                {
                    IsActive = true;
                    CxWSSingleResultData reportQueryItemPathResult = dgvProjects.SelectedRows[0].Cells["ResultEntity"].Value as CxWSSingleResultData;
                    int scanId = -1;
                    Int32.TryParse(dgvProjects.SelectedRows[0].Cells["ScanId"].Value.ToString(), out scanId);

                    if (reportQueryItemPathResult == null)
                        return;

                    this.SelectedRowChanged(this, new ResultData(reportQueryItemPathResult, scanId, this.SelectedNode));

                    GetShortDescription(scanId, reportQueryItemPathResult.PathId);
                    UpdateGridShowPath();
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnIndex = e.ColumnIndex;
                int rowIndex = e.RowIndex;

                if (columnIndex >= 0 && rowIndex >= 0)
                {
                    if (columnIndex == 0) // check box
                    {
                        if (dgvProjects.Rows[rowIndex].Cells["checkBoxesColumn"].Value != null)
                        {
                            dgvProjects.Rows[rowIndex].Cells["checkBoxesColumn"].Value = !(bool)dgvProjects.Rows[rowIndex].Cells["checkBoxesColumn"].Value;
                        }
                        else
                        {
                            dgvProjects.Rows[rowIndex].Cells["checkBoxesColumn"].Value = true;
                        }

                        return;
                    }

                    if (dgvProjects.Columns[columnIndex].HeaderText == Constants.COL_NAME_REMARK)
                    {
                        EditRemark(columnIndex, rowIndex);

                        return;
                    }
                    if (dgvProjects.SelectedRows.Count > 0)
                    {
                        IsActive = true;
                        CxWSSingleResultData reportQueryItemPathResult = dgvProjects.SelectedRows[0].Cells["ResultEntity"].Value as CxWSSingleResultData;
                        int scanId = -1;
                        Int32.TryParse(dgvProjects.SelectedRows[0].Cells["ScanId"].Value.ToString(), out scanId);

                        if (reportQueryItemPathResult == null)
                            return;

                        this.SelectedRowChanged(this, new ResultData(reportQueryItemPathResult, scanId, this.SelectedNode));

                        GetShortDescription(scanId, reportQueryItemPathResult.PathId);
                        UpdateGridShowPath();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
        }

        private void linkLabel1_Sorted(object sender, EventArgs e)
        {
            try
            {
                if (this.SelectedNode != null)
                {
                    string responseText = string.Empty;
                    CxAppSecCodbashing codbashingDetails = new CxAppSecCodbashing();

                    CxRESTApiCommon rESTApiPortalConfiguration = new CxRESTApiCommon(string.Format(_apiAppSecCoachLessonsRequestData, this.SelectedNode.Id));
                    HttpWebResponse response = rESTApiPortalConfiguration.InitPortalBaseUrl();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                        {
                            responseText = reader.ReadToEnd();
                        }
                    }

                    if (!string.IsNullOrEmpty(responseText))
                    {
                        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                        codbashingDetails = (CxAppSecCodbashing)javaScriptSerializer.Deserialize(responseText, typeof(CxAppSecCodbashing));
                    }

                    if (codbashingDetails != null)
                    {
                        NavigateToCodebashing(codbashingDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
        }

        private void NavigateToCodebashing(CxAppSecCodbashing codbashingDetails)
        {
            string urlToDescription = codbashingDetails.url + "?serviceProviderId=" + codbashingDetails.paramteres.serviceProviderId
            + "&utm_source=" + codbashingDetails.paramteres.utm_source
            + "&utm_campaign=" + codbashingDetails.paramteres.utm_campaign;

            QueryDescriptionForm codebashingDesc = new QueryDescriptionForm(urlToDescription, OidcLoginData.GetOidcLoginDataInstance().AccessToken, descriptionHeader);
            codebashingDesc.Show();

        }

        private void GetShortDescription(long scanId, long pathId)
        {
            string responseText = string.Empty;
            CxQueryShortDescription queryShortDescription = new CxQueryShortDescription();

            CxRESTApiCommon rESTApiPortalConfiguration = new CxRESTApiCommon(string.Format(_apiShortDescription, scanId, pathId));
            HttpWebResponse response = rESTApiPortalConfiguration.InitPortalBaseUrl();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd();
                }
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                queryShortDescription = (CxQueryShortDescription)javaScriptSerializer.Deserialize(responseText, typeof(CxQueryShortDescription));
            }
            this.label1.Text = queryShortDescription.shortDescription;
        }

        private void EditRemark(int columnIndex, int rowIndex)
        {
            if (CommonData.IsWorkingOffline)
            {
                MessageBox.Show("You are working offline. \rCannot update data", "Error", MessageBoxButtons.OK);
                return;
            }
            int currentRowIndex = dgvProjects.CurrentCell.RowIndex;
            int currentColumnIndex = dgvProjects.CurrentCell.ColumnIndex;

            CxWSSingleResultData reportQueryItemPathResult = dgvProjects.Rows[rowIndex].Cells["ResultEntity"].Value as CxWSSingleResultData;

            long pathId = reportQueryItemPathResult.PathId;
            long resultId = Convert.ToInt64(dgvProjects.Rows[rowIndex].Cells["ScanId"].Value);
            CxViewerAction.CxVSWebService.CxWSResultPath resultPath = PerspectiveHelper.GetPathCommentsHistory(resultId, pathId);
            string commentHistory = string.Empty;
            if (resultPath != null && !string.IsNullOrEmpty(resultPath.Comment))
            {
                commentHistory = resultPath.Comment;
            }

            string[] commentsArr = commentHistory.Split(Convert.ToChar(255));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < commentsArr.Length; i++)
            {
                if (!string.IsNullOrEmpty(commentsArr[i]))
                {
                    sb.Append(commentsArr[i].ToString());
                    if (i != commentsArr.Length)
                    {
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            EditRemarkPopUp remarkPopUp = new EditRemarkPopUp("", sb.ToString(),true);

            DialogResult result = remarkPopUp.ShowDialog();

            if (result != DialogResult.OK)
                return;

            string remark = remarkPopUp.Remark;
            if (String.IsNullOrWhiteSpace(remark))
            {
                return;
            }
            if (ChangeResultHelper.EditRemark(resultId, pathId, remark) == Entities.Enum.ProjectScanStatuses.Success)
            {
                reportQueryItemPathResult.Comment = remark;
                dgvProjects.Rows[rowIndex].Cells[columnIndex].Value = remark;
                dgvProjects.Rows[rowIndex].Selected = true;
                dgvProjects.CurrentCell = dgvProjects.Rows[rowIndex].Cells[columnIndex];
            }

        }

        #endregion

        private Dictionary<long, string> stateList = null;

        public Dictionary<long, string> ResultStateList
        {
            get
            {
                if (stateList != null)
                {
                    return stateList;
                }
                else
                {
                    try
                    {

                        Dictionary<long, string> list = new Dictionary<long, string>();
                        ResultState[] res = PerspectiveHelper.GetResultStateList();

                        foreach (ResultState state in res)
                        {
                            list.Add(state.ResultID, state.ResultName);
                        }

                        stateList = list;
                    }
                    catch (Exception ex)
                    {
                        Logger.Create().Error(ex.ToString());
                    }

                }

                return stateList;
            }
        }

        private AssignUser[] projectAssignUsers = null;

        public AssignUser[] ProjectAssignUsers
        {
            get
            {

                try
                {
                    projectAssignUsers = PerspectiveHelper.GetProjectAssignUsers();
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }
                return projectAssignUsers;


            }
        }

        private void cbState_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!IsMandatoryCommentOnChangeResultState())
            {
                updateResultStateDetails(sender,"");
            }
            else
            {
                EditRemarkPopUp remarkPopUp = new EditRemarkPopUp("", "",false);

                DialogResult result = remarkPopUp.ShowDialog();

                if (result == DialogResult.Cancel)
                {
                    this.cbState.SelectedIndex = -1;
                    return;
                }
                string remark = remarkPopUp.Remark;
                if (!String.IsNullOrWhiteSpace(remark))
                    updateResultStateDetails(sender, remark);
            }
            this.cbState.SelectedIndex = -1;
        }


        private CheckBox checkboxHeader = new CheckBox();

        private void updateResultStateDetails(object sender,string remark)
        {
            bool needRefresh = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ComboBox senderComboBox = (ComboBox)sender;
                ComboBoxItem item = (ComboBoxItem)senderComboBox.SelectedItem;
                List<ResultStateData> list = new List<ResultStateData>();
                foreach (DataGridViewRow row in dgvProjects.Rows)
                {
                    if (row.Cells["checkBoxesColumn"].Value != null && (bool)row.Cells["checkBoxesColumn"].Value)
                    {
                        CxWSSingleResultData reportQueryItemPathResult = row.Cells["ResultEntity"].Value as CxWSSingleResultData;
                        long pathId = reportQueryItemPathResult.PathId;
                        long resultId = Convert.ToInt64(row.Cells["ScanId"].Value);
                        list.Add(new ResultStateData()
                        {
                            data = item.Id.ToString(),
                            PathId = pathId,
                            Remarks = remark,
                            ResultLabelType = (int)CxViewerAction.Helpers.ResultLabelTypeEnum.State,
                            scanId = resultId
                        });

                    }
                }

                if (list.Count > 0)
                {
                    needRefresh = PerspectiveHelper.UpdateResultState(list.ToArray());
                }

                if (needRefresh)
                {
                    this.Refresh(this, currentNodedata);
                }


            }
            catch (Exception ex)
            {

                Logger.Create().Error(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool IsMandatoryCommentOnChangeResultState()
        {
            CxRESTApiPortalConfiguration rESTApiPortalConfiguration = new CxRESTApiPortalConfiguration();
            return rESTApiPortalConfiguration.InitPortalConfigurationDetails().MandatoryCommentOnChangeResultState;
        }

        private void show_chkBox()
        {
            checkboxHeader.Checked = false;
            Rectangle rect = dgvProjects.GetCellDisplayRectangle(0, -1, true);
            rect.Y = 10;
            rect.X = rect.Location.X + (rect.Width / 4) + 1;
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged -= new EventHandler(checkboxHeader_CheckedChanged);
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
            dgvProjects.Controls.Add(checkboxHeader);
        }

        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvProjects.Rows)
            {
                row.Cells["checkBoxesColumn"].Value = checkboxHeader.Checked;
            }
            dgvProjects.EndEdit();
        }

        private void cbSeverity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            bool needRefresh = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ComboBox senderComboBox = (ComboBox)sender;
                ComboBoxItem item = (ComboBoxItem)senderComboBox.SelectedItem;
                List<ResultStateData> list = new List<ResultStateData>();
                foreach (DataGridViewRow row in dgvProjects.Rows)
                {
                    if (row.Cells["checkBoxesColumn"].Value != null && (bool)row.Cells["checkBoxesColumn"].Value)
                    {
                        CxWSSingleResultData reportQueryItemPathResult = row.Cells["ResultEntity"].Value as CxWSSingleResultData;
                        long pathId = reportQueryItemPathResult.PathId;
                        long resultId = Convert.ToInt64(row.Cells["ScanId"].Value);
                        list.Add(new ResultStateData()
                        {
                            data = item.Id.ToString(),
                            PathId = pathId,
                            Remarks = string.Empty,
                            ResultLabelType = (int)CxViewerAction.Helpers.ResultLabelTypeEnum.Severity,
                            scanId = resultId
                        });
                    }
                }

                if (list.Count > 0)
                {
                    needRefresh = PerspectiveHelper.UpdateResultState(list.ToArray());
                }

                if (needRefresh)
                {
                    this.Refresh(this, currentNodedata);
                }

            }
            catch (Exception ex)
            {

                Logger.Create().Error(ex.ToString());

            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.cbSeverity.SelectedIndex = -1;
            }
        }

        private void cbAssign_SelectionChangeCommitted(object sender, EventArgs e)
        {
            bool needRefresh = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ComboBox senderComboBox = (ComboBox)sender;
                ComboBoxItem1 cbItem = (ComboBoxItem1)senderComboBox.SelectedItem;
                string item = cbItem.Value;
                List<ResultStateData> list = new List<ResultStateData>();
                foreach (DataGridViewRow row in dgvProjects.Rows)
                {
                    if (row.Cells["checkBoxesColumn"].Value != null && (bool)row.Cells["checkBoxesColumn"].Value)
                    {
                        CxWSSingleResultData reportQueryItemPathResult = row.Cells["ResultEntity"].Value as CxWSSingleResultData;
                        long pathId = reportQueryItemPathResult.PathId;
                        long resultId = Convert.ToInt64(row.Cells["ScanId"].Value);
                        list.Add(new ResultStateData()
                        {
                            data = item,
                            PathId = pathId,
                            Remarks = string.Empty,
                            ResultLabelType = (int)CxViewerAction.Helpers.ResultLabelTypeEnum.Assign,
                            scanId = resultId
                        });
                    }
                }

                if (list.Count > 0)
                {
                    needRefresh = PerspectiveHelper.UpdateResultState(list.ToArray());
                }

                if (needRefresh)
                {
                    this.Refresh(this, currentNodedata);
                }

            }
            catch (Exception ex)
            {

                Logger.Create().Error(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.cbAssign.SelectedIndex = -1;
            }
        }

    }

    public class ResultData : EventArgs
    {
        public ResultData(CxWSSingleResultData result, int scanId, TreeNodeData nodeData)
        {
            Result = result;
            ScanId = scanId;
            NodeData = nodeData;
        }
        public CxWSSingleResultData Result
        {
            set;
            get;
        }

        public int ScanId
        {
            set;
            get;
        }

        public TreeNodeData NodeData
        {
            set;
            get;
        }
    }

    public class ComboBoxItem
    {
        public ComboBoxItem(long id, string value)
        {
            Id = id;
            Value = value;
        }

        public long Id
        {
            set;
            get;
        }

        public string Value
        {
            set;
            get;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class ComboBoxItem1
    {
        public ComboBoxItem1(string title, string value)
        {
            Title = title;
            Value = value;
        }

        public string Title
        {
            set;
            get;
        }

        public string Value
        {
            set;
            get;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}

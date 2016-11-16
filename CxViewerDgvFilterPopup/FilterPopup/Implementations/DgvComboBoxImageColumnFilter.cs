using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DgvFilterPopup.FilterPopup;

namespace DgvFilterPopup {


    /// <summary>
    /// A standard <i>column filter</i> implementation for ComboBox columns.
    /// </summary>
    /// <remarks>
    /// If the <b>DataGridView</b> column to which this <i>column filter</i> is applied
    /// is not a ComboBox column, it automatically creates a distinct list of values from the bound <b>DataView</b> column.
    /// If the DataView changes, you should do an explicit call to <see cref="DgvComboBoxColumnFilter.RefreshValues"/> method.
    /// </remarks>
    public partial class DgvComboBoxImageColumnFilter : DgvBaseColumnFilter {

        /// <summary>
        /// Initializes a new instance of the <see cref="DgvComboBoxColumnFilter"/> class.
        /// </summary>
        public DgvComboBoxImageColumnFilter()
        {
            InitializeComponent();        
            comboBoxValue.SelectedValueChanged += new EventHandler(onFilterChanged);
        }
        
        /// <summary>
        /// Gets the ComboBox control containing the available values.
        /// </summary>
        public ComboBox ComboBoxValue { get { return comboBoxValue; }}



        /// <summary>
        /// Perform filter initialitazion and raises the FilterInitializing event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// When this <i>column filter</i> control is added to the <i>column filters</i> array of the <i>filter manager</i>,
        /// the latter calls the <see cref="DgvBaseColumnFilter.Init"/> method which, in turn, calls this method.
        /// You can ovverride this method to provide initialization code or you can create an event handler and 
        /// set the <i>Cancel</i> property of event argument to true, to skip standard initialization.
        /// </remarks>
        protected override void OnFilterInitializing(object sender, CancelEventArgs e) {
            base.OnFilterInitializing(sender, e);
            if (e.Cancel) return;

            if (this.DataGridViewColumn is DataGridViewComboBoxColumn) {
                comboBoxValue.ValueMember = ((DataGridViewComboBoxColumn)DataGridViewColumn).ValueMember;
                comboBoxValue.DisplayMember = ((DataGridViewComboBoxColumn)DataGridViewColumn).DisplayMember;
                comboBoxValue.DataSource = ((DataGridViewComboBoxColumn)DataGridViewColumn).DataSource;
            }
            else {
                comboBoxValue.ValueMember = this.DataGridViewColumn.DataPropertyName;
                comboBoxValue.DisplayMember = this.DataGridViewColumn.DataPropertyName;
                RefreshValues();
            }
            this.FilterHost.RegisterComboBox(comboBoxValue);
        }

        /// <summary>
        /// For non-combobox columns, refreshes the list of the <b>DataView</b> values in the column.
        /// </summary>
        public void RefreshValues() {
            if (!(this.DataGridViewColumn is DataGridViewComboBoxColumn)) {
                //DataTable DistinctDataTable = this.BoundDataView.ToTable(true, new string[] { this.DataGridViewColumn.DataPropertyName });               
                List<string> list = new List<string>();
                list.Add("All");
                list.Add("Enabled");
                list.Add("Disabled");
                //DistinctDataTable.Rows.Add(new object[] { legendItemEnable, legendItemDisable });
                //DistinctDataTable.DefaultView.Sort = this.DataGridViewColumn.DataPropertyName;
                comboBoxValue.DataSource = list; // DistinctDataTable;
            }
        }


        /// <summary>
        /// Builds the filter expression and raises the FilterExpressionBuilding event
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Override <b>OnFilterExpressionBuilding</b> to provide a filter expression construction
        /// logic and to set the values of the <see cref="DgvBaseColumnFilter.FilterExpression"/> and <see cref="DgvBaseColumnFilter.FilterCaption"/> properties.
        /// The <see cref="DgvFilterManager"/> will use these properties in constructing the whole filter expression and to change the header text of the filtered column.
        /// Otherwise, you can create an event handler and set the <i>Cancel</i> property of event argument to true, to skip standard filter expression building logic.
        /// </remarks>
        protected override void OnFilterExpressionBuilding(object sender, CancelEventArgs e){
         	base.OnFilterExpressionBuilding(sender, e);
            if (e.Cancel) {
                FilterManager.RebuildFilter();
                return;
            } 

            string ResultFilterExpression = "";
            string ResultFilterCaption = OriginalDataGridViewColumnHeaderText ;

            // Managing the NULL and NOT NULL cases which are type-independent

            if (ResultFilterExpression != "") {
                FilterExpression = ResultFilterExpression;
                FilterCaption = ResultFilterCaption + "\n ";
                FilterManager.RebuildFilter();
                return;
            }

            object FilterValue = comboBoxValue.SelectedValue;
            string FormattedValue = "";
            
            if (ColumnDataType == typeof(Bitmap)) {
                // Managing the string-column case
                string EscapedFilterValue = StringEscape(FilterValue.ToString());
                if (!string.IsNullOrEmpty(EscapedFilterValue) && EscapedFilterValue.ToLower() != "all")
                {
                    string value = "0";
                    if (EscapedFilterValue == "Enabled")
                    {
                        value = "1";
                    }

                    ResultFilterExpression = "IsEnableInt=" + "'" + value + "'";
                    ResultFilterCaption += "\n" + " " + comboBoxValue.Text;
                }
                else
                {
                    ResultFilterExpression = "1=1";
                    ResultFilterCaption += "\n" + " " + comboBoxValue.Text;
                }
            }
            else {
                // Managing the other cases
                FormattedValue = FormatValue(FilterValue, this.ColumnDataType);
                if (FormattedValue != "") {
                    ResultFilterExpression = this.DataGridViewColumn.DataPropertyName + " " + FormattedValue ;
                    ResultFilterCaption += "\n" + " " + comboBoxValue.Text ;
                }

            }
            if (ResultFilterExpression != "")
            {
                FilterExpression = ResultFilterExpression;
                FilterCaption = ResultFilterCaption;
                FilterManager.RebuildFilter();
            }            
        }

        private void onFilterChanged(object sender, EventArgs e){
            if (!FilterApplySoon || !this.Visible) return;
            Active = true;
            FilterExpressionBuild();

        }
    }
}

using System;
using System.Windows.Forms;
using Common;

namespace CxViewerAction.Views
{
    public partial class EditRemarkPopUp : Form
    {
        private string remark = string.Empty;
        private bool isCommentHistoryVisible = true;
        public string Remark
        {
            get { return remark; }
        }

        public EditRemarkPopUp(string currentRemark, string historyComments, bool _isCommentHistoryVisible)
        {
            InitializeComponent();
            textBox.Text = currentRemark;
            txtCommentHistory.Text = historyComments;
            isCommentHistoryVisible = _isCommentHistoryVisible;
            if (!_isCommentHistoryVisible)
            {
                label2.Visible = false;
                txtCommentHistory.Visible = false;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 7F);
                this.label1.Text = "Enter mandatory comment for result state change :";
            }
            else
            {
                this.label1.Text = "Enter your comment:";
            }
        }


        private void EditRemarkPopUp_Load(object sender, EventArgs e)
        {
            Logger.Create().Info("Editing remark pop up.");
            MinimizeBox = false;
            MaximizeBox = false;
            MinimumSize = MaximumSize = Size;
            AcceptButton = okButton;
            CancelButton = cancelButton;

            okButton.Click += (sender1, e1) =>
                                  {
                                      if (!String.IsNullOrWhiteSpace(textBox.Text))
                                          DialogResult = DialogResult.OK;
                                      else
                                          MessageBox.Show("Please enter comment", "Error", MessageBoxButtons.OK);
                                  };

            cancelButton.Click += (sender1, e1) =>
                                  {
                                      DialogResult = DialogResult.Cancel;
                                      Close();
                                  };
            FormClosing += (sender1, e1) => { remark = textBox.Text; };
        }
        

    }
}

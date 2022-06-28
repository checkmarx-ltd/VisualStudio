using System;
using System.Windows.Forms;
using Common;

namespace CxViewerAction.Views
{
    public partial class EditRemarkPopUp : Form
    {
        private string remark = string.Empty;
        public string Remark
        {
            get { return remark; }
        }

        public EditRemarkPopUp(string currentRemark, string historyComments)
        {
            InitializeComponent();
            textBox.Text = currentRemark;
            txtCommentHistory.Text = historyComments;
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
                                      DialogResult = DialogResult.OK;
                                      Close();
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

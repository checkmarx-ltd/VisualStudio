using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CxViewerAction.Views
{
    public partial class WaitFrm : Form, IWaitView
    {
        #region [Delegates]

        /// <summary>
        /// Delegate mrthod. Used for async call Cancel handler function
        /// </summary>
        private delegate void CancelingDelegate();
        
        #endregion

        #region [Private Members]
        #endregion

        #region [Public Properties]

        /// <summary>
        /// Gets or sets dialog process name
        /// </summary>
        public string ProgressDialogMessage
        {
            get { return lblProgressMessage.Text; }
            set { lblProgressMessage.Text = value; }
        }

        /// <summary>
        /// Sets cancel button click handler
        /// </summary>
        public EventHandler CancelHandler
        {
            set { btnCancel.Click += value; }
        }
        #endregion

        #region [Constructors]

        public WaitFrm(string progressMessage, EventHandler cancelEvent)
        {
            InitializeComponent();
            ProgressDialogMessage = progressMessage;
            CancelHandler = cancelEvent;
        }

        #endregion

        #region [Public methods]

        /// <summary>
        /// Close view
        /// </summary>
        public void CloseView()
        {
            this.Close();
        }

        /// <summary>
        /// Show modal dialog view
        /// </summary>
        /// <returns></returns>
        public virtual DialogResult ShowModalView()
        {
            return this.ShowDialog();
        }

        /// <summary>
        /// Show modal dialog view
        /// </summary>
        /// <param name="parent">Parent container view</param>
        /// <returns></returns>
        public virtual DialogResult ShowModalView(IView parent)
        {
            return this.ShowDialog((IWin32Window)parent);
        }

        /// <summary>
        /// Show non-modal dialog view
        /// </summary>
        public void ShowView()
        {
            this.Show();
        }

        /// <summary>
        /// Show non-modal dialog view
        /// </summary>
        /// <param name="parent">Parent container view</param>
        public void ShowView(IView parent)
        {
            this.Show((IWin32Window)parent);
        }
        
        #endregion

        #region [Private methods]

        /// <summary>
        /// Cancel button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelingDelegate del = new CancelingDelegate(Canceling);
            Invoke(del);
        }

        /// <summary>
        /// Change cancel button state after button pressed
        /// </summary>
        private void Canceling()
        {
            btnCancel.Text = "Canceling...";
            btnCancel.Enabled = false;
        }
        
        #endregion
    }
}

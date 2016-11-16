using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace CxViewerAction.Views
{
    public partial class OpenPercspectiveDialog : Form
    {
        public OpenPercspectiveDialog()
        {
            InitializeComponent();
        }

        public bool RememberDecision
        {
            get { return chRemember.Checked; }
        }
    }
}

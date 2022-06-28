using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using CxViewerAction.Helpers;
using Common;

namespace CxViewerAction.Views
{
    public partial class ErrorFrm : Form
    {
        public ErrorFrm(string errorDescription, EventHandler onReconnect, EventHandler onRelogin)
        {
            Logger.Create().Info("Error form loading.");
            InitializeComponent();
            
            txtErrorDescription.Text = errorDescription;
            Logger.Create().Info("Reconnect on button click event calling.");
            btnReconnect.Click += new EventHandler(delegate(object o, EventArgs e) { Visible = false; });
            btnReconnect.Click += onReconnect;
            btnReconnect.Click += new EventHandler(delegate(object o, EventArgs e) { Visible = true; });
            Logger.Create().Info("Reconnect on button click event called.");
            //btnRelogin.Click += new EventHandler(delegate(object o, EventArgs e) { this.DialogResult = DialogResult.Cancel; });
            //if (onRelogin != null)
            //    btnRelogin.Click += onRelogin;
        }
    }
}

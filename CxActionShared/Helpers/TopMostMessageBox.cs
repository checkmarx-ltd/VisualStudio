using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace CxViewerAction2022.Helpers
{
    /// <summary>
    /// Show Message on the top of all windows
    /// </summary>
    static public class TopMostMessageBox
    {
        static public DialogResult Show(string message)
        {
            return Show(message, string.Empty, MessageBoxButtons.OK);
        }

        static public DialogResult Show(string message, string title)
        {
            return Show(message, title, MessageBoxButtons.OK);
        }

        static public DialogResult Show(string message, string title,
            MessageBoxButtons buttons)
        {
            Form topmostForm = new Form();
            // We do not want anyone to see this window so position it off the 
            // visible screen and make it as small as possible
            topmostForm.Size = new System.Drawing.Size(1, 1);
            topmostForm.StartPosition = FormStartPosition.Manual;
            System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
            topmostForm.Location = new System.Drawing.Point(rect.Bottom + 10,
                rect.Right + 10);
            topmostForm.Show();
            // Make this form the active form and make it TopMost
            topmostForm.Focus();
            topmostForm.BringToFront();
            topmostForm.TopMost = true;
            // Finally show the MessageBox with the form just created as its owner
            DialogResult result = MessageBox.Show(topmostForm, message, title,
                buttons);
            // clean it up all the way
            topmostForm.Dispose();

            return result;
        }
    }
}

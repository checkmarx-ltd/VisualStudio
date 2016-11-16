using CxViewerAction.Views.DockedView;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CxViewerVSIX.OptionsPages
{
    [Guid("CF7539CD-011B-4D31-9DEA-1297078C05A0")]
    public class AuthenticationOptionPage : DialogPage
    {
        protected OptionsAuthCtrl _page;

        protected override void OnApply(PageApplyEventArgs e)
        {
            Page.OnOK();
        }

        protected override IWin32Window Window
        {
            get
            {
                return Page;
            }
        }

        public OptionsAuthCtrl Page {
            get {
                if (_page == null)
                    _page = new OptionsAuthCtrl();
                return _page;
            }
        }
    }

}

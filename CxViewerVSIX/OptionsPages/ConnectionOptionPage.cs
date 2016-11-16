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
    [Guid("DE81EF74-9A1A-4175-A780-6E6F7BA5B8C7")]
    public class ConnectionOptionPage : DialogPage
    {
        protected ConnectionCtrl _page;

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

        public ConnectionCtrl Page {
            get {
                if (_page == null)
                    _page = new ConnectionCtrl();
                return _page;
            }
        }
    }

}

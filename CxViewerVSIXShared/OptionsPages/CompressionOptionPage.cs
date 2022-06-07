using CxViewerAction2022.Views.DockedView;
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
    [Guid("ED15360C-D6F5-42D9-A5D2-A86FDBD32A1D")]
    public class CompressionOptionPage : DialogPage
    {
        protected OptionsZipCtrl _page;

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

        public OptionsZipCtrl Page {
            get {
                if (_page == null)
                    _page = new OptionsZipCtrl();
                return _page;
            }
        }
    }

}

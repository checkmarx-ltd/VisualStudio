using System;
using System.Windows.Forms;

namespace CxViewerAction
{
    /// <summary>
    /// Represent View class structure
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// View load handler
        /// </summary>
        event EventHandler Load;

        /// <summary>
        /// Show modal dialog
        /// </summary>
        /// <returns></returns>
        DialogResult ShowModalView();

        /// <summary>
        /// Show modal dialog
        /// </summary>
        /// <param name="parent">parent view</param>
        /// <returns></returns>
        DialogResult ShowModalView(IView parent);

        /// <summary>
        /// Show non-modal view
        /// </summary>
        void ShowView();

        /// <summary>
        /// Show non-modal view
        /// </summary>
        /// <param name="parent">parent view</param>
        void ShowView(IView parent);

        /// <summary>
        /// Close view
        /// </summary>
        void CloseView();
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Windows.Forms;
using CxViewerAction.Views;
using Common;

namespace CxViewerAction.Helpers
{
    /// <summary>
    /// Perform background work by parallel threads to improve UI responsibility
    /// </summary>
    public class BackgroundWorkerHelper
    {
        #region [Private Constants]

        /// <summary>
        /// Set the numbers of attempts to execute function in exeception occurs
        /// </summary>
        private int _repeatCountOnException = 3;

        /// <summary>
        /// Set the delay interval beetween repeat
        /// </summary>
        private int _repeatIntervalOnException = 15000;

        #endregion

        #region [Private Members]

        /// <summary>
        /// Main background worker delegate
        /// </summary>
        private delegate object _doWorkDelegate(object state);

        /// <summary>
        /// Main background worker funck
        /// </summary>
        private Action<object> _doWorkFunc = null;

        /// <summary>
        /// Relogin handler
        /// </summary>
        private EventHandler _doReloginFunc = null;

        #endregion

        #region [Public Properties]

        static bool _isReloginInvoked;
        public static bool IsReloginInvoked { get { return _isReloginInvoked; } set { _isReloginInvoked = value; } }

        public Action<object> DoWorkFunc
        {
            get { return _doWorkFunc; }
            set { _doWorkFunc = value; }
        }

        public EventHandler DoReloginFunc
        {
            get { return _doReloginFunc; }
            set { _doReloginFunc = value; }
        }

        #endregion

        #region [Constructors]

        public BackgroundWorkerHelper(int reconnectInterval, int reconnectCount)
        {
            _repeatIntervalOnException = reconnectInterval;
            _repeatCountOnException = reconnectCount;
        }

        public BackgroundWorkerHelper(Action<object> func, int reconnectInterval, int reconnectCount)
        {
            _doWorkFunc = func;
            _repeatIntervalOnException = reconnectInterval;
            _repeatCountOnException = reconnectCount;
        }

        public BackgroundWorkerHelper(Action<object> func)
        {
            _doWorkFunc = func;
        }

        #endregion

        #region [Public Methods]

        /// <summary>
        /// Perform background job and show view while process executes
        /// </summary>
        /// <param name="message">View title message</param>
        /// <returns>If false - cancel buton in supported view was pressed</returns>
        public bool DoWork(string message)
        {
            if (_doWorkFunc == null)
                return false;

            // Show perform operation dialog
            bool canceling = false;
            IWaitView waitView = new WaitFrm(message, delegate(object sender, EventArgs e) { canceling = true; });

            int repeatNum = 1;
            string errorMessage = null;
            bool success = false;

            do
            {
                try
                {
                    Logger.Create().Debug("DoWork" + message);

                    if (Invoke(message, waitView) || canceling)
                    {
                        success = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                    LoginHelper.DoLogout();
                    errorMessage = ex.Message;
                    message = string.Format("Reconnection attempt #{0}...", repeatNum.ToString());

                    DateTime waitTo = DateTime.Now.AddMilliseconds(_repeatIntervalOnException);

                    while (!(canceling || DateTime.Now > waitTo))
                    {
                        Application.DoEvents();
                        Thread.Sleep(10);
                    }

                    if (canceling)
                        break;
                }
            }
            while (repeatNum++ <= _repeatCountOnException);

            if (success || canceling)
            {
                if (!string.IsNullOrEmpty(message))
                    waitView.CloseView();

                return !canceling ? success : false;
            }

            bool executionResult = false;

            while (true)
            {
                try
                {
                    Logger.Create().Debug("DoWork - Reconnection " + message);
                    ErrorFrm frmError = new ErrorFrm(errorMessage, delegate (object o, EventArgs e) { executionResult = Invoke("Reconnection...", waitView); }, _doReloginFunc);
                    DialogResult res = frmError.ShowDialog();

                    if (res == DialogResult.Cancel)
                        break;

                    if (res != DialogResult.OK)
                    {
                        if (_doReloginFunc != null)
                        {
                            IsReloginInvoked = true;
                            _doReloginFunc.BeginInvoke(null, null, null, null);
                            
                        }
                        break;
                    }

                    if (executionResult)
                        break;
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }
            }

            if (!string.IsNullOrEmpty(message))
                waitView.CloseView();

            return executionResult;
        }

        public void DoWork()
        {
            if (_doWorkFunc == null)
                return;


            try
            {
                _doWorkFunc.BeginInvoke(null, null, null);
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
            
        }

        private bool Invoke(string message, IWaitView waitView)
        {
            if (!string.IsNullOrEmpty(message))
                waitView.ShowView();

            waitView.ProgressDialogMessage = message;

            // Perform work
            IAsyncResult res = _doWorkFunc.BeginInvoke(null, null, null);

            // Whait wail operation complete and this time execute waiting app threads
            while (!res.IsCompleted)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            _doWorkFunc.EndInvoke(res);
            // Close perform operation dialog
            return true;
        }

        #endregion
    }
}

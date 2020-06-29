using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Common;
using CxViewerAction.CxVSWebService;
using CxViewerAction.Dispatchers;
using CxViewerAction.Entities;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Services;
using CxViewerAction.ValueObjects;
using CxViewerAction.WebPortal;

namespace CxViewerAction.Helpers
{
    public class LoginHelper
    {
        static readonly object lockObj = new object();
        private static readonly OIDCLoginHelper _oidcLoginHelper = new OIDCLoginHelper();

        #region [ Constants ]

        /// <summary>
        /// File name to store user settings
        /// </summary>
        private const string FileName = "CxVSPlugin.conf";

        private static string studioVersion = "";
        private static bool isScanner = true;
		private static CxRESTApi cxRestApi = null;


		public static bool IsScanner
        {
            get { return LoginHelper.isScanner; }
        }

        public static string StudioVersion
        {
            get { return LoginHelper.studioVersion; }
            set { LoginHelper.studioVersion = value; }
        }

        public static string FullConfigPath
        {
            get
            {
                if (String.IsNullOrEmpty(studioVersion))
                {
                    string addinTarget = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Visual Studio 2010\Settings"); //\CxExtention
                    return Path.Combine(addinTarget, FileName);
                }
                else
                {
                    string studioYear = "2010";
                    switch (studioVersion)
                    {
                        case "8.0": studioYear = "2005"; break;
                        case "9.0": studioYear = "2008"; break;
                        case "10.0": studioYear = "2010"; break;
                        case "11.0": studioYear = "2012"; break;
                        case "12.0": studioYear = "2013"; break;
                        case "14.0": studioYear = "2015"; break;
                        case "15.0": studioYear = "2017"; break;
                        case "16.0": studioYear = "2019"; break;
                    }
                    string addinTargetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), string.Format(@"Visual Studio {0}\Settings", studioYear)); //\CxExtention
                    return Path.Combine(addinTargetPath, FileName);
                }
            }
        }

        public static string FolderPath
        {
            get { return Path.GetDirectoryName(FullConfigPath); }
        }

        /// <summary>
        /// Wait dialog progress text
        /// </summary>
        private const string WAIT_DIALOG_PROGRESS_TEXT = "Authorizing...";

        #endregion [ Constants ]

        #region [ Private Members ]

        private static IDispatcher _dispatcher;
        private static bool _isLogged = false;
        private static bool useCurrentSession = false;
        private static LoginResult _loginResult = null;

        #endregion [ Private Members ]

        #region [Public Properties]

        public static LoginResult LoginResult
        {
            get { return _loginResult; }
        }

        public static bool IsLogged
        {
            get { return _isLogged; }
            set { _isLogged = value; }
        }
        
        private static string serverBaseUrl;

        public static string ServerBaseUrl
        {
            get { return serverBaseUrl; }
        }

        private static string server;

        public static string Server
        {
            get { return server; }
        }

        public static CxPortalConfiguration PortalConfiguration { get; set; }


        #endregion

        /// <summary>
        /// Load login data
        /// </summary>
        /// <param name="tryNum">How many times load method completed with problems</param>
        /// <returns></returns>
        internal static LoginData Load(int tryNum)
        {
            Logger.Create().Debug("Load preferences");
            LoginData login = LoginData.GetLoginDataInstance;

            if (tryNum > 2)
                return login;

            string fileName = FullConfigPath; // Directory.GetCurrentDirectory() + "\\" + FileName;

            if (!File.Exists(fileName))
                return login;

            XmlSerializer reader = new XmlSerializer(typeof(LoginData));

            try
            {
                if (!File.Exists(@fileName))
                {
                    MessageBox.Show(string.Format("CHECKMARCX Viewer Configuration file is missing. {0}{1}", Environment.NewLine, fileName));
                }
                else
                {
                    // load user personal settings
                    lock (lockObj)
                    {
                        using (StreamReader file = new StreamReader(@fileName))
                        {
                            login = (LoginData)reader.Deserialize(file);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Logger.Create().Error(ex.ToString());
                if (ex.Message.StartsWith("The process cannot access the file"))
                {
                    Thread.Sleep(1000);
                    Load(++tryNum);
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex);
            }

            return login;
        }

        /// <summary>
        /// Save login info
        /// </summary>
        /// <param name="login">Login class</param>
        internal static void Save(LoginData login)
        {
            loginCache = null;
            Logger.Create().Debug("Save preferences");
            server = null; // reset server
            string fileName = FullConfigPath; // Directory.GetCurrentDirectory() + "\\" + FileName;
            Logger.Create().Debug("Save To File : " + fileName);

            try
            {
                lock (lockObj)
                {
                    if (File.Exists(fileName))
                    {
                        // Create a new FileInfo object.
                        FileInfo fInfo = new FileInfo(fileName);

                        // Set the IsReadOnly property.
                        if (fInfo.IsReadOnly == true)
                        {
                            fInfo.IsReadOnly = false;
                        }
                    }

                    XmlSerializer writer = new XmlSerializer(typeof(LoginData));
                    using (StreamWriter file = new StreamWriter(@fileName))
                    {
                        writer.Serialize(file, login);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error("Failed to save preferences");
                Logger.Create().Error(ex);
            }
        }

        /// <summary>
        /// Execute login
        /// </summary>
        /// <returns></returns>
        internal static LoginResult DoLogin(out bool cancelPressed)
        {
            return TryDoLogin(out cancelPressed, true, true);
        }

        internal static LoginResult DoLoginWithoutForm(out bool cancelPressed, bool relogin)
        {
			//loads the preferences
            Entities.LoginData login = LoadSaved();
            if (login == null)
            {
                cancelPressed = false;
                return new LoginResult();
            }
            if (string.IsNullOrEmpty(login.Server))
            {
                cancelPressed = false;
                return new LoginResult();
            }
            
            return TryDoLogin(out cancelPressed, relogin, false);
        }

        static LoginResult TryDoLogin(out bool cancelPressed, bool relogin, bool showForm)
        {
            cancelPressed = false;

			//TODO check what to do with this
            if (_isLogged && !relogin)
            {
                _loginResult.AuthenticationData = LoadSaved();
                return _loginResult;
            }

            LoginData login = null;

            if (_dispatcher == null)
                _dispatcher = ServiceLocators.ServiceLocator.GetDispatcher();

            if (_dispatcher != null)
            {
                login = LoadSaved();

                // verify that user hasn't active session and dialog data was validated sucessfull
				//TODO check if isLogged neccessary
                if (!_isLogged || !login.CanLog() || relogin)
                {
                    if (showForm)
                    {
                        try
                        {
                            _dispatcher.Dispatch(login);
                        }
                        catch (Exception err)
                        {
                            Logger.Create().Error(err);
                        }
                    }
                }
            }
            login.IsLogging = true;
            LoginResult loginResult = ExecuteLogin(login, out cancelPressed, relogin);

            if (loginResult != null && loginResult.IsSuccesfull)
            {
                isScanner = loginResult.AuthenticationData.SaveSastScan;
            }

            _isLogged = !cancelPressed ? loginResult.IsSuccesfull : false;
            
            Helpers.LoginHelper.Save(login);

            _loginResult = loginResult;
            _loginResult.IsSuccesfull = _isLogged;

            return loginResult;
        }

        public static LoginResult ExecuteLogin(LoginData login, out bool cancelPressed, bool relogin)
        {
            LoginResult loginResult = new LoginResult();
            cancelPressed = !login.IsLogging;

            if (login != null && login.IsLogging && (!_isLogged || relogin))
            {
                CxWebServiceClient client = null;

                BackgroundWorkerHelper bg = new BackgroundWorkerHelper(delegate
                {
                    try
                    {
                        client = new CxWebServiceClient(login);
                    }
                    catch (Exception e)
                    {
                        Logger.Create().Error(e.ToString());
                        System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK);
                        return;
                    }

                    if (client == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot connect to server", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                        return;
                    }

                    // perform login
                    try
                    {
                        serverBaseUrl = login.ServerBaseUri;

						bool loginSucceeded = DolLogin(login, client);
                        if (loginSucceeded) {
                            loginResult.IsSuccesfull = true;
                            loginResult.AuthenticationData = login;
                        }
                        _loginResult = loginResult;
                    }
                    catch (WebException ex)
                    {
                        Logger.Create().Error(ex.Message, ex);
                        loginResult.LoginResultType = LoginResultType.UnknownServerName;
                        loginResult.LoginResultMessage = ex.Message;
                        loginResult.IsSuccesfull = false;
                    }
                    catch (Exception ex)
                    {
                        Logger.Create().Error(ex.Message, ex);
                        loginResult.LoginResultType = LoginResultType.UnknownError;
                        loginResult.LoginResultMessage = ex.Message;
                    }
                    finally
                    {
                        if (client != null)
                            client.Close();
                    }

                },
                login.ReconnectInterval * 1000, login.ReconnectCount);

                cancelPressed = !bg.DoWork(WAIT_DIALOG_PROGRESS_TEXT);
            }

            return loginResult;
        }


        public static bool DolLogin(LoginData login, CxWebServiceClient client)
        {
            bool loginSucceeded = false;
			_oidcLoginHelper.resetLatestResult();
			OidcLoginResult oidcLoginResult = _oidcLoginHelper.ConnectToIdentidyProvider(login.ServerBaseUri);

			if (oidcLoginResult.IsSuccessful)
			{
				cxRestApi = new CxRESTApi(login);
				string accessToken = cxRestApi.Login(oidcLoginResult.Code);
				cxRestApi.GetPermissions(accessToken);
                loginSucceeded = true;

            }
			else
			{
				_oidcLoginHelper.CloseLoginWindow();
			}
            return loginSucceeded;

        }

        /// <summary>
        /// Logout from service
        /// </summary>
        internal static void DoLogout()
        {
            Logger.Create().Debug("Logging out, clear authentication data");
            OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            oidcLoginData.AccessToken = null;
            oidcLoginData.RefreshToken = null;
            oidcLoginData.AccessTokenExpiration = -1;
            _isLogged = false;
        }

        /// <summary>
        /// Load stored user data
        /// </summary>
        /// <returns></returns>
        internal static LoginData LoadSaved()
        {
            if (loginCache != null)
            {
                return loginCache;
            }
            LoginData login = Helpers.LoginHelper.Load(0);
            server = login.Server;
            loginCache = login;
            return login;
        }

        static LoginData loginCache = null;

        internal static void ShowLoginErrorMessage(LoginResult loginResult)
        {
            if (loginResult.LoginResultType == LoginResultType.UnknownServerName)
            {
                TopMostMessageBox.Show(Constants.ERR_UNKNOWN_SERVER, "Verify authority", MessageBoxButtons.OK);
            }
            else
            {
                TopMostMessageBox.Show(Constants.ERR_UNKNOWN_USER_PASSWORD, "Verify authority", MessageBoxButtons.OK);
            }
        }
    }
}

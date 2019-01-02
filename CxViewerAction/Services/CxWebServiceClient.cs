using System;
using System.Net;
using CxViewerAction.Helpers;
using CxViewerAction.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using CxViewerAction.CxVSWebService;
using CxViewerAction.Entities.WebServiceEntity;

namespace CxViewerAction.Services
{
    /// <summary>
    /// Wrapper for service client
    /// </summary>
    public class CxWebServiceClient
    {
        #region [Private Members]
        private readonly CxViewerAction.Services.CxVSWebServiceWrapper _client;
        private const int INTERFACE_VERSION = 1;
        #endregion

        #region [Public Properties]

        /// <summary>
        /// Servive client object
        /// </summary>
        public CxViewerAction.Services.CxVSWebServiceWrapper ServiceClient { get { return _client; } }

        #endregion

        #region [Private Memebers]

        /// <summary>
        /// Constract service endpoint binding configuration. Used to control service on transort layer
        /// </summary>
        /// <returns></returns>
        //private BasicHttpBinding GetBindingConfiguration(bool ssl)
        //{
        //    BasicHttpBinding binding;

        //    if (ssl)
        //        binding = new BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.Transport);
        //    else
        //        binding = new BasicHttpBinding();

        //    binding.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
        //    binding.ReaderQuotas.MaxArrayLength = 16384;
        //    binding.ReaderQuotas.MaxStringContentLength = 655360;
        //    binding.SendTimeout = TimeSpan.FromHours(1);
        //    binding.MaxReceivedMessageSize = 655360;

        //    return binding;
        //}

        #endregion

        #region [Constructors]
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">server url</param>
        public CxWebServiceClient(LoginData pLogin)
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            CxViewerAction.Services.CxWSResolverWrapper resolver = new CxViewerAction.Services.CxWSResolverWrapper { Url = pLogin.Server };
            resolver.DisableConnectionOptimizations = pLogin.DisableConnectionOptimizations;
            resolver.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            resolver.UseDefaultCredentials = true;
            CxWsResolver.CxWSResponseDiscovery discoveryResponse = resolver.GetWebServiceUrl(CxViewerAction.CxWsResolver.CxClientType.VS, INTERFACE_VERSION);

            if (!discoveryResponse.IsSuccesfull)
            {
                string errorMsg = "Internal Server Error - Resolver Returned: \r\n" + discoveryResponse.ErrorMessage;
                throw new Exception(errorMsg);
            }
            
            _client = new CxVSWebServiceWrapper { Url = discoveryResponse.ServiceURL };
            _client.DisableConnectionOptimizations = pLogin.DisableConnectionOptimizations;
            _client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            _client.UseDefaultCredentials = true;
            
        }

        #endregion

        #region [Public methods]
        /// <summary>
        /// Close client and clear object data
        /// </summary>
        public void Close()
        {
            _client.Dispose();
        }

        /// <summary>
        /// Return selected problem description
        /// </summary>
        /// <param name="queryId">Problem identifier</param>
        /// <returns></returns>
        public static CxWSResponseQueryDescription GetQueryDesription(int queryId)
        {
            LoginData login = LoginHelper.Load(0);
            CxWebServiceClient client = null;
            if (login != null)
            {
                client = new CxWebServiceClient(login);
            }
            if (client == null || client.ServiceClient == null)
                return null;

            if (LoginHelper.LoginResult == null)
            {
                //Execute login
                bool cancelPressed;
                LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
                if (!loginResult.IsSuccesfull)
                    loginResult = LoginHelper.DoLogin(out cancelPressed);
            }
            CxWSResponseQueryDescription cxWSResponseQueryDescription = client.ServiceClient.GetQueryDescriptionByQueryId(LoginHelper.LoginResult.SessionId, queryId);
            //output = cxWSResponseQueryDescription.QueryDescription;

            client.Close();
            return cxWSResponseQueryDescription;
        }

        #endregion
    }
}

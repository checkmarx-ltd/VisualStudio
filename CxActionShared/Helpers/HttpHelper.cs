using System;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace CxViewerAction.Helpers
{
    /// <summary>
    /// Helper class for downloading content via HTTP
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Download page content
        /// </summary>
        /// <param name="url">page url</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            string output;
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                response = (HttpWebResponse)request.GetResponse();

                using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                {
                    output = responseStream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Common.Logger.Create().Error(ex.ToString());
                output = null;
            }
            finally
            {
                if(response != null)
                    response.Close();
            }

            return output;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CxViewerAction.Services
{
    public class CxVSWebServiceWrapper : CxVSWebService.CxVSWebService
    {
        public bool DisableConnectionOptimizations = false;
        
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest request = base.GetWebRequest(uri);
            if (DisableConnectionOptimizations)
            {
                ((HttpWebRequest)request).ServicePoint.UseNagleAlgorithm = false;
                ((HttpWebRequest)request).ServicePoint.Expect100Continue = false;
                ((HttpWebRequest)request).KeepAlive = false;
                ((HttpWebRequest)request).ServicePoint.ConnectionLimit = 10;
            }
            return request;
        }
    }
}

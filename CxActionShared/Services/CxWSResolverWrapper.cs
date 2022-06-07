using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CxViewerAction2022.Services
{
    public class CxWSResolverWrapper : CxViewerAction2022.CxWsResolver.CxWSResolver
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

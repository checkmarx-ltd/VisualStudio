using CefSharp;
using CefSharp.Handler;
using Common;
using System;
using System.Web;
using System.Windows.Forms;

namespace CxActionShared
{
    public class CommonCustomResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }

        public class NewCustomRequestHandler : RequestHandler
        {
            private string _token;
            public NewCustomRequestHandler(string token)
            {
                _token = token;
            }

            public NewCustomRequestHandler() { }

            protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
                IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
            {
                if (!string.IsNullOrEmpty(_token))
                    return new CustomResourceRequestHandler(_token);
                else
                    return new CustomResourceRequestHandler();
            }
        }

        public class CustomResourceRequestHandler : ResourceRequestHandler
        {
            private string _token;

            public CustomResourceRequestHandler(string token)
            {
                _token = token;
            }

            public CustomResourceRequestHandler() { }

            protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request,
                IRequestCallback callback)
            {
                Logger.Create().Info("In on before resource load event of chrome browser page.");
                var Url = request.Url.ToString();
                Uri myUri = new Uri(request.Url);

                if (!string.IsNullOrEmpty(_token))
                {
                    var headers = request.Headers;
                    headers["Authorization"] = $"Bearer {_token}";
                    request.Headers = headers;
                }

                if (Url.ToLower().Contains("error="))
                {
                    string error = HttpUtility.ParseQueryString(myUri.Query).Get("error");
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                    Logger.Create().Error(error);
                    browser.CloseBrowser(false);
                }
                else
                {
                    Logger.Create().Debug("New url " + Url + ".");
                }
                return CefReturnValue.Continue;
            }
        }
    }
}
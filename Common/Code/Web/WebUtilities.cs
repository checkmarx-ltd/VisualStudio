using System;
using System.Web;

namespace Common.Code.Web
{
    public static class WebUtilities
    {
        #region API

        public static string GetUrlEncodedString(String str)
        {
            return HttpUtility.UrlEncode(str);
        }
    }

    #endregion
}
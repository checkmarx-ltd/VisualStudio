using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Common.Web
{
    public class Cookies
    {
        #region Fields

        /// <summary>
        ///     Creates a cookie associated with the specified URL.
        /// </summary>
        /// <param name="lpszUrlName"></param>
        /// <param name="lpszCookieName"></param>
        /// <param name="lpszCookieData"></param>
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

        #endregion

        #region API

        /// <summary>
        ///     Reads cookies from the given <see cref="HttpWebResponse"/> Set-Cookies header, and add it to the <see cref="CookieCollection"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public static void FillWebReponseCookies(HttpWebRequest request, HttpWebResponse response)
        {
            for (int i = 0; i < response.Headers.Count; i++)
            {
                string name = response.Headers.GetKey(i);

                if (name != "Set-Cookie")
                {
                    continue;
                }

                string value = response.Headers.Get(i);

                foreach (var singleCookie in value.Split(','))
                {
                    Match match = Regex.Match(singleCookie, "(.+?)=(.+?);");

                    if (match.Captures.Count == 0)
                    {
                        continue;
                    }

                    response.Cookies.Add(
                        new Cookie(
                            match.Groups[1].ToString(),
                            match.Groups[2].ToString(),
                            "/",
                            request.Host.Split(':')[0]));
                }
            }
        }


        /// <summary>
        ///     Sets the cookies of the given <see cref="HttpWebResponse"/> in the internal browser.
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="urlName">The url which the cookie will be valid under.</param>
        public static void SetCookiesInTheInternalBrowser(HttpWebResponse webResponse, string urlName, string cookieName = null)
        {
            foreach (Cookie cookie in webResponse.Cookies)
            {
                InternetSetCookie(string.Format("{0}", urlName), cookieName, cookie.ToString());
            }
        }

        #endregion
    }
}

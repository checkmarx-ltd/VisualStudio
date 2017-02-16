using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Web
{
    public class Cookies
    {
        #region Fields

        private const Int32 InternetCookieHttponly = 0x2000;

        /// <summary>
        ///     Creates a cookie associated with the specified URL.
        /// </summary>
        /// <param name="lpszUrlName"></param>
        /// <param name="lpszCookieName"></param>
        /// <param name="lpszCookieData"></param>
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(string url, string cookieName, StringBuilder cookieData, ref int size, Int32 dwFlags, IntPtr lpReserved);

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

        /// <summary>
        ///     Sets the cookies of the given <see cref="CookieCollection"/> in the internal browser.
        /// </summary>
        public static void SetCookiesInTheInternalBrowser(CookieCollection cookies, string urlName, string cookieName = null)
        {
            foreach (Cookie cookie in cookies)
            {
                InternetSetCookie(string.Format("{0}", urlName), cookieName, cookie.ToString());
            }
        }

        /// <summary>
        ///     Gets the URI cookie container.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }

            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }

            return cookies;
        }

        #endregion
    }
}

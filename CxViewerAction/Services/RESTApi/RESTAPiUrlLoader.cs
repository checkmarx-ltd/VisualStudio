using CxViewerAction.Entities;

namespace CxViewerAction.Services.RESTApi
{
    public class RESTAPiUrlLoader
    {
        #region API

        public string Load(LoginData loginData)
        {
            string url = "";

            if (loginData.SSO)
            {
                url = "/cxrestapi/auth/ssologin";
            }
            else if (!loginData.isSaml)
            {
                url = "/cxrestapi/auth/login";
            }


            return url;
        }

        #endregion
    }
}

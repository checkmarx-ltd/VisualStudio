using System;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Services;
using CxViewerAction.CxVSWebService;
using System.Threading;
using CxViewerAction.Entities;

namespace CxViewerAction.Helpers
{
    public class ChangeResultHelper
    {
        
        public static ProjectScanStatuses EditRemark(long resultId, long pathId, string remark)
        {

            LoginData loginData = LoginHelper.LoadSaved();
          //  OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            LoginResult loginResult = new LoginResult();
            bool cancelPressed = false;
            //if (oidcLoginData.AccessToken == null) {
              
            if (loginResult.SessionId == "") { 
                //Execute login
                loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
                if (!loginResult.IsSuccesfull)
                    loginResult = LoginHelper.DoLogin(out cancelPressed);

                if (loginResult.IsSuccesfull)
                {
                    return EditRemark(loginResult, resultId, pathId, remark);
                }
                else if (!cancelPressed)
                {
                    TopMostMessageBox.Show("Unable to connect to server or user creadentials are invalid. Please verify data", "Log in problem");
                    return ProjectScanStatuses.Error;
                }
            }
            else {
                loginResult.AuthenticationData = loginData;
                loginResult.IsSuccesfull = true;
            }

            return ProjectScanStatuses.CanceledByUser;
        }


        static ProjectScanStatuses EditRemark(LoginResult loginResult, long resultId, long pathId, string remark)
        {
            bool cxWSBasicResponse = false;
            // show bind project form
            CxWebServiceClient client = null;
            bool isThrewError = false;
            BackgroundWorkerHelper bg = new BackgroundWorkerHelper(delegate(object obj)
            {
                try
                {
                    client = new CxWebServiceClient(loginResult.AuthenticationData);
                }
                catch (Exception e)
                {
                    Common.Logger.Create().Error(e.ToString());
                    System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK);
                    isThrewError = true;
                    return;
                }

                if (client == null)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot connect to server", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                    isThrewError = true;
                    return;
                }
                ResultStateData[] dataArr = new ResultStateData[1];
                dataArr[0] = new ResultStateData()
                {
                    data = string.Empty,
                    PathId = pathId,
                    Remarks = remark,
                    ResultLabelType = (int)ResultLabelTypeEnum.Remark,
                    scanId = resultId
                };
                cxWSBasicResponse = PerspectiveHelper.UpdateResultState(dataArr);

            }, loginResult.AuthenticationData.ReconnectInterval * 1000, loginResult.AuthenticationData.ReconnectCount);

            //Show wait dialog and perform server request in different thread to safe UI responsibility
            if (!bg.DoWork("Changing remark"))
                return ProjectScanStatuses.CanceledByUser;

            if (!cxWSBasicResponse|| isThrewError)
            {
                return ProjectScanStatuses.Error;
            }

            return ProjectScanStatuses.Success;
        }


    }

    public enum ResultLabelTypeEnum
    {
        IgnorePath = 0,
        Remark = 1,
        Severity = 2,
        State = 3,
        Assign = 4
    }
}

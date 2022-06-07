using System;
using CxViewerAction2022.Dispatchers;
using CxViewerAction2022.Commands;
using CxViewerAction2022.Entities.WebServiceEntity;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Services;
using CxViewerAction2022.CxVSWebService;
using System.Collections.Generic;

namespace CxViewerAction2022.Helpers
{
	/// <summary>
	/// Settings upload project params
	/// </summary>
	public class UploadHelper
	{
		// Wait dialog caption message
		const string PRESETS_LOADING_TEXT = "Project settings form initializing...";
        
        static CxWebServiceClient _client;
		static IDispatcher _dispatcher;

		/// <summary>
		/// Set upload params
		/// </summary>
		/// <param name="loginResult">Auth user data for uploading</param>
		/// <param name="project">Selected solution project data</param>
		/// <returns></returns>
		internal static Upload SetUploadSettings(LoginResult loginResult, Project project, bool cancelStatus)
		{
			cancelStatus = false;
			PresetResult presetResult = null;
            TeamResult teamResult = null;

			BackgroundWorkerHelper bg = new BackgroundWorkerHelper(delegate(object obj)
			{
                _client = InitCxClient(loginResult);
                if (_client == null)
                    return;

                presetResult = GetPresets(loginResult, presetResult);

                CxWSResponseGroupList teamXmlList = _client.ServiceClient.GetAssociatedGroupsList(loginResult.SessionId);

                teamResult = new TeamResult();
                teamResult.IsSuccesfull = teamXmlList.IsSuccesfull;
				teamResult.Teams = new Dictionary<string, string>();
				if (teamXmlList.GroupList != null && teamXmlList.GroupList.Length > 0)
                    {
                        for (int i = 0; i < teamXmlList.GroupList.Length; i++)
                        {
                            teamResult.Teams.Add(teamXmlList.GroupList[i].ID, teamXmlList.GroupList[i].GroupName);
                        }
                    }

				if (_client != null)
					_client.Close();
			}, loginResult.AuthenticationData.ReconnectInterval * 1000, loginResult.AuthenticationData.ReconnectCount);

			//Show wait dialog and perform server request in different thread to safe UI responsibility
			if (!bg.DoWork(PRESETS_LOADING_TEXT))
			{
				cancelStatus = true;
				return null;
			}

			if (!presetResult.IsSuccesfull)
				return null;

			var uploadData = new Upload(new EntityId(loginResult), project.ProjectName,
                                           string.Format("{0} Description", project.ProjectName), presetResult.Presets, 0,
                                           teamResult.Teams,Guid.Empty.ToString(),true);

			if (_dispatcher == null)
				_dispatcher = ServiceLocators.ServiceLocator.GetDispatcher();

			if (_dispatcher != null)
			{
				_dispatcher.Dispatch(uploadData);
			}

			return uploadData;
		}

        static PresetResult GetPresets(LoginResult loginResult, PresetResult presetResult)
        {
            CxWSResponsePresetList cxWSResponsePresetList = _client.ServiceClient.GetPresetList(loginResult.SessionId);

            presetResult = new PresetResult();

            presetResult.IsSuccesfull = cxWSResponsePresetList.IsSuccesfull;
            presetResult.Presets = new Dictionary<int, string>();
            if (cxWSResponsePresetList.PresetList != null)
            {
                foreach (Preset item in cxWSResponsePresetList.PresetList)
                {
                    presetResult.Presets.Add(Convert.ToInt32(item.ID), item.PresetName);
                }
            }
            return presetResult;
        }

        static CxWebServiceClient InitCxClient(LoginResult loginResult)
        {
            CxWebServiceClient client;

            try
            {
                client =  new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Common.Logger.Create().Error(e.ToString());
                LoginHelper.DoLogout();
                System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK);
                return null;
            }

            if (client == null)
            {
                LoginHelper.DoLogout();
                System.Windows.Forms.MessageBox.Show("Cannot connect to server", "Error", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return client;
        }
	}
}

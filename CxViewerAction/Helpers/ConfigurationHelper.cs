using CxViewerAction.CxVSWebService;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction.Helpers
{
    internal class ConfigurationHelper : IConfigurationHelper
    {
        public ConfigurationResult GetConfigurationList(string sessionId, BackgroundWorkerHelper bg, CxWebServiceClient client)
        {
            ConfigurationResult configuration = null;
            bg.DoWorkFunc = delegate(object obj)
            {
                configuration = new ConfigurationResult();
                CxWSResponseConfigSetList cxWSResponseConfigSetList = client.ServiceClient.GetConfigurationSetList(sessionId);

                configuration.IsSuccesfull = cxWSResponseConfigSetList.IsSuccesfull;
                configuration.Configurations = new Dictionary<long, string>();
                if (cxWSResponseConfigSetList != null && cxWSResponseConfigSetList.ConfigSetList.Length > 0)
                {
                    for (int i = 0; i < cxWSResponseConfigSetList.ConfigSetList.Length; i++)
                    {
                        configuration.Configurations.Add(cxWSResponseConfigSetList.ConfigSetList[i].ID, cxWSResponseConfigSetList.ConfigSetList[i].ConfigSetName);
                    }
                }
            };

            if (!bg.DoWork("Receive Configuration list..."))
                return null;

            return configuration;
        }
    }
}

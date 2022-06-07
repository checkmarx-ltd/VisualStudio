using Common;
using CxViewerAction2022.CxVSWebService;
using CxViewerAction2022.Entities.WebServiceEntity;
using CxViewerAction2022.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction2022.Helpers
{
    internal class ConfigurationHelper : IConfigurationHelper
    {
        public ConfigurationResult GetConfigurationList(string sessionId, BackgroundWorkerHelper bg, CxWebServiceClient client)
        {
            Logger.Create().Debug("Getting configuration list.");
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

            Logger.Create().Debug("Configuration list received. " + configuration.ToString());
            return configuration;
        }
    }
}

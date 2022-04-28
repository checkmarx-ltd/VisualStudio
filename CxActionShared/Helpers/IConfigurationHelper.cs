using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction.Helpers
{
    interface IConfigurationHelper
    {
        ConfigurationResult GetConfigurationList(string sessionId, BackgroundWorkerHelper bg, CxWebServiceClient client);
    }
}

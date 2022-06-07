using CxViewerAction2022.Entities.WebServiceEntity;
using CxViewerAction2022.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction2022.Helpers
{
    interface IConfigurationHelper
    {
        ConfigurationResult GetConfigurationList(string sessionId, BackgroundWorkerHelper bg, CxWebServiceClient client);
    }
}

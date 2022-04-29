using CxViewerAction.CxVSWebService;
using CxViewerAction.Entities;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction.Helpers
{
    interface IScanHelper
    {
        ProjectScanStatuses DoScan(Project project, bool isIncremental, ref CxWSQueryVulnerabilityData[] scanData, ref long scanId);
    }
}

using CxViewerAction2022.CxVSWebService;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Entities.Enum;
using CxViewerAction2022.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction2022.Helpers
{
    interface IScanHelper
    {
        ProjectScanStatuses DoScan(Project project, bool isIncremental, ref CxWSQueryVulnerabilityData[] scanData, ref long scanId);
    }
}

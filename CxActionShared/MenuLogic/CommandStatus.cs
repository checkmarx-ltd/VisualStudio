using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CxViewerAction.MenuLogic
{
    public enum CommandStatus
    {
        CommandStatusUnsupported = 0,
        CommandStatusSupported = 1,
        CommandStatusEnabled = 2,
        CommandStatusNull = 4,
    }
}

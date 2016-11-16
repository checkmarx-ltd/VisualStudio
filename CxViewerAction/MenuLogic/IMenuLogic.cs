using EnvDTE;

namespace CxViewerAction.MenuLogic
{
    public interface IMenuLogic
    {
        ActionStatus Act();
        CommandStatus GetStatus();
    }
}

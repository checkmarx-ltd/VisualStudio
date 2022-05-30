using EnvDTE;

namespace CxViewerAction2022.MenuLogic
{
    public interface IMenuLogic
    {
        ActionStatus Act();
        CommandStatus GetStatus();
    }
}

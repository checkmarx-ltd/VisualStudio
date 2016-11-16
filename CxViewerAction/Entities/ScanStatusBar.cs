namespace CxViewerAction.Entities
{
    public class ScanStatusBar
    {
        public ScanStatusBar(bool inProgress, string label, int completed, int total, bool clearBeforeUpdateProgress = false)
        {
            ClearBeforeUpdateProgress = clearBeforeUpdateProgress;
            InProgress = inProgress;
            Label = label;
            Completed = completed;
            Total = total;
        }

        public bool ClearBeforeUpdateProgress { get; private set; }
        public bool InProgress { get; private set; }
        public string Label { get; private set; }
        public int Completed { get; private set; }
        public int Total { get; private set; }
    }
}

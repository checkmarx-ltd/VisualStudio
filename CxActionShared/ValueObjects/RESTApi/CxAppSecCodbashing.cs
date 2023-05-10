namespace CxViewerAction.ValueObjects
{
    public class CxAppSecCodbashing
    {
        public string url { get; set; }
        public Paramteres paramteres { get; set; }
    }

    public class Paramteres
    {
        public string serviceProviderId { get; set; }
        public string utm_source { get; set; }
        public string utm_campaign { get; set; }
        public string queryId { get; set; }
    }
}

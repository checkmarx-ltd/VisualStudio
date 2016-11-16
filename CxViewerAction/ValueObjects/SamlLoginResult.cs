namespace CxViewerAction.ValueObjects
{
    public class SamlLoginResult : Results
    {
        #region Ctor

        public SamlLoginResult(string ott)
            : this(true, string.Empty, ott) { }

        public SamlLoginResult(bool isSuccessful, string resultMessage, string ott)
        {
            IsSuccessful = isSuccessful;
            ResultMessage = resultMessage;
            Ott = ott;
        }

        #endregion

        #region Properties

        public string Ott { get; private set; } 
        
        #endregion
    }
}

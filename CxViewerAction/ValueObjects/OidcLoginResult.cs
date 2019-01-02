namespace CxViewerAction.ValueObjects
{
    public class OidcLoginResult : Results
    {
        #region Ctor

        public OidcLoginResult(string code)
            : this(true, string.Empty, code) { }

        public OidcLoginResult(bool isSuccessful, string resultMessage, string code)
        {
            IsSuccessful = isSuccessful;
            ResultMessage = resultMessage;
            Code = code;
        }

        #endregion

        #region Properties

        public string Code { get; private set; } 
        
        #endregion
    }
}

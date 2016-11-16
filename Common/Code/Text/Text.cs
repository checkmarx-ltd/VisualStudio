namespace Common.Text
{
    public class Text
    {
        #region API

        public static string RemoveTrailingSlash(string str)
        {
            return str.TrimEnd('/');
        }

        #endregion
    }
}

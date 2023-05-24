namespace Core.Utilities.Security.JWT
{
    public class UserToken
    {
        public string UserAccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}

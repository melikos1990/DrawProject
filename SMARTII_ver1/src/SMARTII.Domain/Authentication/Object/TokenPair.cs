namespace SMARTII.Domain.Authentication.Object
{
    public class TokenPair
    {
        public TokenPair()
        {
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
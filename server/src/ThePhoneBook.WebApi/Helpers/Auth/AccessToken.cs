namespace ThePhoneBook.WebApi.Helpers.Auth
{
    /// <summary>
    /// The access token
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// The Jwt token
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// The number of seconds the token is valid for
        /// </summary>
        public int ExpiresIn { get; }

        public AccessToken(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}
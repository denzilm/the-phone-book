using ThePhoneBook.WebApi.Helpers.Auth;

namespace ThePhoneBook.WebApi.Api.Users.Dtos
{
    /// <summary>
    /// Describes the response after logging in
    /// </summary>
    public class LoginResponse
    {
        public LoginResponse(AccessToken accessToken)
        {
            AccessToken = accessToken;
        }

        /// <summary>
        /// The access token
        /// </summary>
        public AccessToken AccessToken { get; }
    }
}
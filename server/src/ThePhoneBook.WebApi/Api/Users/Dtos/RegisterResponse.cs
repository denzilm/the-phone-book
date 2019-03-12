namespace ThePhoneBook.WebApi.Api.Users.Dtos
{
    /// <summary>
    /// Describes the response after registering
    /// </summary>
    public class RegisterResponse
    {
        /// <summary>
        /// The unique identifier of the registered user
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The first name of the registered user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the registered user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email address of the registered user
        /// </summary>
        public string Email { get; set; }
    }
}
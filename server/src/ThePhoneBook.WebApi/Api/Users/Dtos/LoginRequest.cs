using System.ComponentModel.DataAnnotations;

namespace ThePhoneBook.WebApi.Api.Users.Dtos
{
    /// <summary>
    /// The login Model
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The email address of the user who wants to log in
        /// </summary>
        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "The email is invalid")]
        public string Email { get; set; }

        /// <summary>
        /// The password of the user who wants to log in
        /// </summary>
        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; }
    }
}
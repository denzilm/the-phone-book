using System.ComponentModel.DataAnnotations;

namespace ThePhoneBook.WebApi.Api.Users.Dtos
{
    /// <summary>
    /// The register model
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The first name of the registering user
        /// </summary>
        [Required(ErrorMessage = "The first name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "The first name must be between 2 and 50 characters long")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the registering user
        /// </summary>
        [Required(ErrorMessage = "The last name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "The last name must be between 2 and 50 characters long")]
        public string LastName { get; set; }

        /// <summary>
        /// The email address of the registering user
        /// </summary>
        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "The email is invalid")]
        public string Email { get; set; }

        /// <summary>
        /// The password of the registering user
        /// </summary>
        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; }

        /// <summary>
        /// The confirmation password
        /// </summary>
        [Required(ErrorMessage = "The confirmation password is required")]
        [Compare(nameof(Password), ErrorMessage = "The passwords does not match")]
        public string ConfirmPassword { get; set; }
    }
}
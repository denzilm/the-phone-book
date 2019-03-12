using System.ComponentModel.DataAnnotations;

namespace ThePhoneBook.WebApi.Api.PhoneBooks.Dtos
{
    /// <summary>
    /// The phone book update request model
    /// </summary>
    public class PhoneBookUpdateRequest
    {
        /// <summary>
        /// The title to update the phone book with
        /// </summary>
        [Required(ErrorMessage = "The title is required")]
        [StringLength(50, MinimumLength = 4,
            ErrorMessage = "The title must be between 4 and 50 characters long")]
        public string Title { get; set; }

        /// <summary>
        /// The description to update the phone book with
        /// </summary>
        [Required(ErrorMessage = "The description is required")]
        [StringLength(100, MinimumLength = 10,
            ErrorMessage = "The description must be between 10 and 100 characters long")]
        public string Description { get; set; }
    }
}
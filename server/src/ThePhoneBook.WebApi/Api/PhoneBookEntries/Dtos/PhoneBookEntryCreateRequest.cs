using System.ComponentModel.DataAnnotations;

namespace ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos
{
    /// <summary>
    /// The phone book entry create request model
    /// </summary>
    public class PhoneBookEntryCreateRequest
    {
        /// <summary>
        /// The first name of the contact
        /// </summary>
        [Required(ErrorMessage = "The first name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "First Name must be between 2 and 50 characters long")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the contact
        /// </summary>
        [Required(ErrorMessage = "The last name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Last Name must be between 2 and 50 characters long")]
        public string LastName { get; set; }

        /// <summary>
        /// The phone number of the contact
        /// </summary>
        [Required(ErrorMessage = "The phone number is required")]
        [MaxLength(15, ErrorMessage = "The phone number cannot be longer than 15 characters")]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "The phone number is in an incorrect format")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The email address of the contact
        /// </summary>
        [EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The address of he contact
        /// </summary>
        public string Address { get; set; }
    }
}
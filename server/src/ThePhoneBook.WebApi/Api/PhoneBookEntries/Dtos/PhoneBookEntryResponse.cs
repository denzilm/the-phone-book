namespace ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos
{
    /// <summary>
    /// Describes the phone book entry response
    /// </summary>
    public class PhoneBookEntryResponse
    {
        /// <summary>
        /// The unique identifier of the phone book entry
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The first name of the contact
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the contact
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The phone number of the contact
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The email address of the contact
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// The address of the contact
        /// </summary>
        public string Address { get; set; }
    }
}
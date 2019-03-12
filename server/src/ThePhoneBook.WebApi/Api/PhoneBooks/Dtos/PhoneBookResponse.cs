namespace ThePhoneBook.WebApi.Api.PhoneBooks.Dtos
{
    /// <summary>
    /// Describes the phone book response
    /// </summary>
    public class PhoneBookResponse
    {
        /// <summary>
        /// The unique identifier of the phone book
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title of the phone book
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description for the phone book
        /// </summary>
        public string Description { get; set; }
    }
}
using System.Collections.Generic;
using ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos;

namespace ThePhoneBook.WebApi.Api.PhoneBooks.Dtos
{
    /// <summary>
    /// Describes the response for a phone book with entries
    /// </summary>
    public class PhoneBookWithEntriesResponse : PhoneBookResponse
    {
        /// <summary>
        /// The entries in the phone book
        /// </summary>
        public List<PhoneBookEntryResponse> PhoneBookEntries { get; set; }
    }
}
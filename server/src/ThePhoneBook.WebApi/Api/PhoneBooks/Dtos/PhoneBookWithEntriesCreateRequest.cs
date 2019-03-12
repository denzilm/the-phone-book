using System.Collections.Generic;
using ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos;

namespace ThePhoneBook.WebApi.Api.PhoneBooks.Dtos
{
    /// <summary>
    /// Describes the request body for a phone book with entries
    /// </summary>
    public class PhoneBookWithEntriesCreateRequest : PhoneBookCreateRequest
    {
        /// <summary>
        /// The entries in the phone book
        /// </summary>
        public List<PhoneBookEntryResponse> PhoneBookEntries { get; set; }
    }
}
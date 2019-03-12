using ThePhoneBook.WebApi.Api.PhoneBooks.Dtos;

namespace ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos
{
    public class PhoneBookEntryWithPhoneBookResponse : PhoneBookEntryResponse
    {
        public PhoneBookResponse PhoneBook { get; set; }
    }
}
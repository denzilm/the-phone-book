using Microsoft.AspNetCore.Identity;

namespace ThePhoneBook.Persistence.Identity
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
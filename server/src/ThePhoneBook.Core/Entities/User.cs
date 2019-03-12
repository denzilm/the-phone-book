using System.Collections.Generic;

namespace ThePhoneBook.Core.Entities
{
    /// <summary>
    /// Models a registered user
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// The identity id from the membership system
        /// </summary>
        public string IdentityId { get; set; }

        /// <summary>
        /// The email address of the registered user
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// The hash of the registered user's password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The first name of the registered user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the registered user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Navigation property to the registered user's phone books
        /// </summary>
        public ICollection<PhoneBook> PhoneBooks { get; set; }
    }
}
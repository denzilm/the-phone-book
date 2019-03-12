using System;

namespace ThePhoneBook.Core.Entities
{
    /// <summary>
    /// Models a single phone book entry
    /// </summary>
    public class PhoneBookEntry : BaseEntity
    {
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

        /// <summary>
        /// Navigation property to the phone book who owns this entry
        /// </summary>
        public PhoneBook PhoneBook { get; set; }

        /// <summary>
        /// Foreign Key to phone book who owns this entry
        /// </summary>
        public Guid PhoneBookId { get; set; }
    }
}
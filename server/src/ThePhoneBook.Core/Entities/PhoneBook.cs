using System;
using System.Collections.Generic;

namespace ThePhoneBook.Core.Entities
{
    /// <summary>
    /// Models a single phone book
    /// </summary>
    public class PhoneBook : BaseEntity
    {
        /// <summary>
        /// The title of the phone
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description describing the purpose of the phone book
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Foreign Key to user who owns the phone book
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Navigation property to user who owns the phone book
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The entries of this phone book
        /// </summary>
        public ICollection<PhoneBookEntry> PhoneBookEntries { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;

namespace ThePhoneBook.Core.Repositories
{
    /// <summary>
    /// The phone book repository
    /// </summary>
    public interface IPhoneBookEntryRepository : IRepository<PhoneBookEntry>
    {
        /// <summary>
        /// Get the phone book entries for the specified book
        /// </summary>
        /// <param name="phoneBookId">The unique identifier for the phone book</param>
        /// <param name="page">The current page we are looking at</param>
        /// <param name="pageSize">The number of phone books per page</param>
        /// <returns>The phone book entries for the phone book</returns>
        Task<IReadOnlyList<PhoneBookEntry>> GetPhoneBookEntriesForBook(Guid phoneBookId, int page, int pageSize);

        /// <summary>
        /// Get the phone book entry with associated phone book
        /// </summary>
        /// <param name="id">The unique identifier for the phone book entry</param>
        /// <returns>The phone book entry with its associated phone book</returns>
        Task<PhoneBookEntry> GetPhoneBookEntryWithPhoneBook(Guid id);

        /// <summary>
        /// Gets the number of phone book entries owned by this phone book
        /// </summary>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <returns>The number of phone book entries owned by this phone book</returns>
        Task<int> CountForBookAsync(Guid phoneBookId);

        /// <summary>
        /// Creates a new phone book entry for the current book
        /// </summary>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <param name="phoneBookEntry">The phone book entry to create</param>
        /// <returns>The newly created phone book entry</returns>
        Task<PhoneBookEntry> CreatePhoneBookEntryForBook(Guid phoneBookId, PhoneBookEntry phoneBookEntry);

        /// <summary>
        /// Retrieves phone book entries by contact full name
        /// </summary>
        /// <param name="userId">The id of the authenticated user</param>
        /// <param name="contactFullName">The full name of the contact on the phone book</param>
        /// <returns>The newly created phone book entry</returns>
        Task<IReadOnlyList<PhoneBookEntry>> SearchPhoneBookEntries(Guid userId, string contactFullName);
    }
}
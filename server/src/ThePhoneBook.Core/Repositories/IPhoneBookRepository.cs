using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;

namespace ThePhoneBook.Core.Repositories
{
    /// <summary>
    /// The phone book repository
    /// </summary>
    public interface IPhoneBookRepository : IRepository<PhoneBook>
    {
        /// <summary>
        /// Gets the number of phone books owned by this user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The number of phone books owned by this user</returns>
        Task<int> CountForUserAsync(Guid userId);

        /// <summary>
        /// Gets a paged amount of phone books owned by this user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="page">The current page we are looking at</param>
        /// <param name="pageSize">The number of phone books per page</param>
        /// <returns>A paged list of phone books owned by this user</returns>
        Task<IReadOnlyList<PhoneBook>> GetPhoneBooksForUser(Guid userId, int page, int pageSize);

        /// <summary>
        /// Gets a paged amount of phone books owned by this user with the related phone book entries
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="page">The current page we are looking at</param>
        /// <param name="pageSize">The number of phone books per page</param>
        /// <returns>A paged list of phone books owned by this user</returns>
        Task<IReadOnlyList<PhoneBook>> GetPhoneBooksForUserWithEntries(Guid userId, int page, int pageSize);

        /// <summary>
        /// Gets a phone book with its related phone book entries
        /// </summary>
        /// <param name="id">The id of the phone book</param>
        /// <returns>The phone book with its related phone book entries</returns>
        Task<PhoneBook> GetPhoneBookWithEntries(Guid id);

        /// <summary>
        /// Creates a new phone book for the current user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="phoneBook">The phone book to create</param>
        /// <returns>The newly created phone book</returns>
        Task<PhoneBook> CreatePhoneBookForUser(Guid userId, PhoneBook phoneBook);
    }
}
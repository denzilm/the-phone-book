using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Core.Repositories;
using ThePhoneBook.Persistence.Application;

namespace ThePhoneBook.Persistence.Repositories
{
    /// <summary>
    /// The phone book repository
    /// </summary>
    public class PhoneBookRepository : GenericRepository<PhoneBook>, IPhoneBookRepository
    {
        public PhoneBookRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the number of phone books owned by this user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The number of phone books owned by this user</returns>
        public async Task<int> CountForUserAsync(Guid userId)
        {
            return await _entities.CountAsync(e => e.UserId == userId).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a paged amount of phone books owned by this user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="page">The current page we are looking at</param>
        /// <param name="pageSize">The number of phone books per page</param>
        /// <returns>A paged list of phone books owned by this user</returns>
        public async Task<IReadOnlyList<PhoneBook>> GetPhoneBooksForUser(Guid userId, int page, int pageSize)
        {
            return (await _entities.Where(e => e.UserId == userId)
                .OrderBy(e => e.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false))
                .AsReadOnly();
        }

        /// <summary>
        /// Gets a paged amount of phone books owned by this user with the related phone book entries
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="page">The current page we are looking at</param>
        /// <param name="pageSize">The number of phone books per page</param>
        /// <returns>A paged list of phone books owned by this user</returns>
        public async Task<IReadOnlyList<PhoneBook>> GetPhoneBooksForUserWithEntries(Guid userId, int page, int pageSize)
        {
            return (await _entities.Include(e => e.PhoneBookEntries)
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false))
                .AsReadOnly();
        }

        /// <summary>
        /// Gets a phone book with its related phone book entries
        /// </summary>
        /// <param name="id">The id of the phone book</param>
        /// <returns>The phone book with its related phone book entries</returns>
        public async Task<PhoneBook> GetPhoneBookWithEntries(Guid id)
        {
            return await _entities.Include(e => e.PhoneBookEntries)
                .FirstOrDefaultAsync(e => e.Id == id)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new phone book for the current user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="phoneBook">The phone book to create</param>
        /// <returns>The newly created phone book</returns>
        public async Task<PhoneBook> CreatePhoneBookForUser(Guid userId, PhoneBook phoneBook)
        {
            phoneBook.Id = Guid.NewGuid();
            phoneBook.UserId = userId;
            return (await _context.AddAsync(phoneBook).ConfigureAwait(false)).Entity;
        }
    }
}
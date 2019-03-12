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
    public class PhoneBookEntryRepository : GenericRepository<PhoneBookEntry>, IPhoneBookEntryRepository
    {
        public PhoneBookEntryRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get the phone book entries for the specified book
        /// </summary>
        /// <param name="phoneBookId">The unique identifier for the phone book</param>
        /// <param name="page">The current page we are looking at</param>
        /// <param name="pageSize">The number of phone books per page</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<PhoneBookEntry>> GetPhoneBookEntriesForBook(Guid phoneBookId, int page, int pageSize)
        {
            return (await _entities.Include(e => e.PhoneBook)
                .Where(e => e.PhoneBookId == phoneBookId)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false))
                .AsReadOnly();
        }

        /// <summary>
        /// Gets the number of phone book entries owned by this phone book
        /// </summary>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <returns>The number of phone book entries owned by this phone book</returns>
        public async Task<int> CountForBookAsync(Guid phoneBookId)
        {
            return await _entities.CountAsync(e => e.PhoneBookId == phoneBookId).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the phone book entry with associated phone book
        /// </summary>
        /// <param name="id">The unique identifier for the phone book entry</param>
        /// <returns>The phone book entry with its associated phone book</returns>
        public async Task<PhoneBookEntry> GetPhoneBookEntryWithPhoneBook(Guid id)
        {
            return await _entities.Include(e => e.PhoneBook)
                .FirstOrDefaultAsync(e => e.Id == id)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new phone book entry for the current user and book
        /// </summary>
        /// <param name="phoneBookId">The id of the phone book</param>
        /// <param name="phoneBookEntry">The phone book entry to create</param>
        /// <returns>The newly created phone book</returns>
        public async Task<PhoneBookEntry> CreatePhoneBookEntryForBook(Guid phoneBookId,
            PhoneBookEntry phoneBookEntry)
        {
            phoneBookEntry.Id = Guid.NewGuid();
            phoneBookEntry.PhoneBookId = phoneBookId;
            return (await _context.AddAsync(phoneBookEntry).ConfigureAwait(false)).Entity;
        }

        /// <summary>
        /// Retrieves phone book entries by contact full name
        /// </summary>
        /// <param name="userId">The id of the authenticated user</param>
        /// <param name="contactFullName">The full name of the contact on the phone book</param>
        /// <returns>The newly created phone book entry</returns>
        public async Task<IReadOnlyList<PhoneBookEntry>> SearchPhoneBookEntries(Guid userId, string contactFullName)
        {
            return (await _entities.Include(e => e.PhoneBook)
                .Where(e => e.PhoneBook.UserId == userId && (e.FirstName + " " + e.LastName)
                    .IndexOf(contactFullName, StringComparison.OrdinalIgnoreCase) > -1)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync()
                .ConfigureAwait(false))
                .AsReadOnly();
        }
    }
}
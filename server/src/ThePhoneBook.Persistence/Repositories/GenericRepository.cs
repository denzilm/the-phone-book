using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Core.Repositories;
using ThePhoneBook.Persistence.Application;

namespace ThePhoneBook.Persistence.Repositories
{
    /// <summary>
    /// A generic repository implementation
    /// </summary>
    /// <typeparam name="T">The entity for the repository</typeparam>
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _entities;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _entities = _context.Set<T>();
        }

        /// <summary>
        /// Adds a new entity to the repository
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The newly added entity</returns>
        public async Task<T> AddAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            return (await _context.AddAsync(entity).ConfigureAwait(false)).Entity;
        }

        /// <summary>
        /// Gets the amount of entities in the repository
        /// </summary>
        /// <returns>The amount of entities in the repository</returns>
        public async Task<int> CountAsync()
        {
            return await _entities.CountAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Removes an entity from the repository
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// Checks whether entity exists
        /// </summary>
        /// <param name="id">The unique identifier for the entity</param>
        /// <returns>A boolean that indicate whether the entity exists</returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _entities.AnyAsync(e => e.Id == id);
        }

        /// <summary>
        /// Gets a read-only list of entities from the repository
        /// </summary>
        /// <returns>A read-only list of entities from the repository</returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return (await _entities.OrderBy(e => e.Id).ToListAsync().ConfigureAwait(false)).AsReadOnly();
        }

        /// <summary>
        /// Gets all entities that matches a specific predicate
        /// </summary>
        /// <param name="predicate">The predicate to match against</param>
        /// <returns>The entities that matches the predicate</returns>
        public async Task<IReadOnlyList<T>> GetAllByAsync(Expression<Func<T, bool>> predicate)
        {
            return (await _entities.Where(predicate).ToListAsync().ConfigureAwait(false)).AsReadOnly();
        }

        /// <summary>
        /// Gets an entity from the repository by id
        /// </summary>
        /// <param name="id">The id for the entity</param>
        /// <returns>The entity</returns>
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves changes to data source
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
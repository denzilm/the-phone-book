using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;

namespace ThePhoneBook.Core.Repositories
{
    /// <summary>
    /// Base contract for all repositories
    /// </summary>
    /// <typeparam name="T">The entity for the repository</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets the amount of entities in the repository
        /// </summary>
        /// <returns>The amount of entities in the repository</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Checks whether entity exists
        /// </summary>
        /// <param name="id">The unique identifier for the entity</param>
        /// <returns>A boolean that indicate whether the entity exists</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Gets a read-only list of entities from the repository
        /// </summary>
        /// <returns>A read-only list of entities from the repository</returns>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity from the repository by id
        /// </summary>
        /// <param name="id">The id for the entity</param>
        /// <returns>The entity</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all entities that matches a specific predicate
        /// </summary>
        /// <param name="predicate">The predicate to match against</param>
        /// <returns>The entities that matches the predicate</returns>
        Task<IReadOnlyList<T>> GetAllByAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity to the repository
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The newly added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Removes an entity from the repository
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        void Delete(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Saves changes to data source
        /// </summary>
        Task SaveChangesAsync();
    }
}
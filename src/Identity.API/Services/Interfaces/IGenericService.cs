using System.Collections.Generic;

namespace Identity.API.Services.Interfaces
{
    /// <summary>
    /// Generic service interface for CRUD operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IGenericService<T> where T : class
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A collection of entities.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity with the specified identifier.</returns>
        T GetById(object id);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        void Delete(object id);
    }
}
using Accelerate.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Data repository for queryable collections.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Type of data entity managed by repository.
    /// </typeparam>
    public interface IQueryableRepository<TEntity> : IDisposable where TEntity : class, IEntity, new()
    {
        /// <summary>
        /// Commit repository changes.
        /// </summary>
        void Commit();
        /// <summary>
        /// Remove data entity.
        /// </summary>
        /// <param name="entity">
        /// Data entity information.
        /// </param>
        void Delete(TEntity entity);
        /// <summary>
        /// Remove a list of data entities.
        /// </summary>
        /// <param name="entities">
        /// Data entities information.
        /// </param>
        void Delete(IEnumerable<TEntity> entities);
        /// <summary>
        /// Retrieve a list of entities and remove them.
        /// </summary>
        /// <param name="expression">
        /// Expression to determine the entities to delete.
        /// </param>
        void Delete(Expression<Func<TEntity, Boolean>> expression);
        /// <summary>
        /// Insert a data entity.
        /// </summary>
        /// <param name="entity">
        /// Data entity information.
        /// </param>
        void Insert(TEntity entity);
        /// <summary>
        /// Insert a list of entities.
        /// </summary>
        /// <param name="entities">
        /// Data entities information.
        /// </param>
        void Insert(IEnumerable<TEntity> entities);
        /// <summary>
        /// Undo repository changes.
        /// </summary>
        void Rollback();
        /// <summary>
        /// Select all entities in repository.
        /// </summary>
        IEnumerable<TEntity> Select();
        /// <summary>
        /// Select a list of entities by expression.
        /// </summary>
        /// <param name="expression">
        /// Expression to determine the entities to select.
        /// </param>
        IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression);
        /// <summary>
        /// Select a list of entities by expression.
        /// </summary>
        /// <param name="expression">
        /// Expression to determine the entities to select.
        /// </param>
        /// <param name="skip">
        /// Number of entities to skip.
        /// </param>
        IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip);
        /// <summary>
        /// Select a list of entities by expression.
        /// </summary>
        /// <param name="expression">
        /// Expression to determine the entities to select.
        /// </param>
        /// <param name="skip">
        /// Number of entities to skip.
        /// </param>
        /// <param name="take">
        /// Number of entities to take.
        /// </param>
        IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip, Int32 take);
        /// <summary>
        /// Update a data entity.
        /// </summary>
        /// <param name="entity">
        /// Data entity information.
        /// </param>
        void Update(TEntity entity);
        /// <summary>
        /// Update a list of data entities.
        /// </summary>
        /// <param name="entities">
        /// Data entities information.
        /// </param>
        void Update(IEnumerable<TEntity> entities);
    }
}

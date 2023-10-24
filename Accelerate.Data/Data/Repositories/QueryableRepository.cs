using Accelerate.Data.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Data repository for queriable collections.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Type of data entity managed by repository.
    /// </typeparam>
    /// <typeparam name="TOptions">
    /// Type of repository options managed in the repository.
    /// </typeparam>
    public abstract class QueryableRepository<TEntity, TOptions> : IQueryableRepository<TEntity> where TEntity : class, IEntity, new()
                                                                                                 where TOptions : QueryableRepositoryOptions
    {
        private Boolean _disposed;
        private TOptions _options;

        /// <summary>
        /// Initialize a new instance of <seealso cref="QueryableRepository{TEntity, TOptions}" /> class.
        /// </summary>
        /// <param name="options">
        /// Configuration options of data repository.
        /// </param>
        protected QueryableRepository(IOptions<TOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentException($"Argument '{nameof(options)}' cannot be null or empty", nameof(options));;
        }

        /// <summary>
        /// Configuration options of data repository.
        /// </summary>
        protected TOptions Options => _options;

        /// <inheritdoc />
        public abstract void Commit();
        /// <inheritdoc />
        public abstract void Delete(TEntity entity);
        /// <inheritdoc />
        public abstract void Delete(IEnumerable<TEntity> entities);
        /// <inheritdoc />
        public abstract void Delete(Expression<Func<TEntity, Boolean>> expression);
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// Indicate if object is currently freeing, releasing, or resetting unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (disposing)
            {
                _options = null;
            }

            _disposed = true;
        }
        /// <inheritdoc />
        public abstract void Insert(TEntity entity);
        /// <inheritdoc />
        public abstract void Insert(IEnumerable<TEntity> entities);
        /// <inheritdoc />
        public abstract void Rollback();
        /// <inheritdoc />
        public abstract IEnumerable<TEntity> Select();
        /// <inheritdoc />
        public abstract IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression);
        /// <inheritdoc />
        public abstract IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip);
        /// <inheritdoc />
        public abstract IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip, Int32 take);
        /// <inheritdoc />
        public abstract void Update(TEntity entity);
        /// <inheritdoc />
        public abstract void Update(IEnumerable<TEntity> entities);
    }
}

using Accelerate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Data depository based on entity framework.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Type of data entity managed by repository.
    /// </typeparam>
    /// <typeparam name="TOptions">
    /// Type of configuration options.
    /// </typeparam>
    public abstract class EntityFrameworkRepository<TEntity, TOptions> : QueryableRepository<TEntity, TOptions> where TEntity : Entity, new()
                                                                                                                where TOptions : EntityFrameworkRepositoryOptions
    {
        private EntityFrameworkRepositoryContext<TEntity, TOptions> _context;
        private Boolean _disposed;
        private readonly Object _syncLock = new Object();

        /// <summary>
        /// Initialize a new instance of class <seealso cref="EntityFrameworkRepository{TEntity, TOptions}" />.
        /// </summary>
        /// <param name="options">
        /// Configuration options of data repository.
        /// </param>
        protected EntityFrameworkRepository(IOptions<TOptions> options) : base(options)
        {
            _context = new EntityFrameworkRepositoryContext<TEntity, TOptions>(this, Options);
        }

        /// <inheritdoc/>
        public override void Commit()
        {
            lock (_syncLock)
            {
                PerformCommit();
            }
        }
        /// <inheritdoc/>
        public override void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException($"Argument '{nameof(entity)}' cannot be null or empty", nameof(entity));
            }

            lock (_syncLock)
            {
                try
                {
                    _context.Remove(entity);

                    if (Options.AutoCommit)
                    {
                        PerformCommit();
                    }
                }
                catch (Exception ex)
                {
                    if (Options.AutoCommit)
                    {
                        PerformRollback();
                    }

                    throw new DataException($"Error deleting entity of type '{typeof(TEntity).Name}': {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentException($"Argument '{nameof(entities)}' cannot be null or empty", nameof(entities));
            }

            lock (_syncLock)
            {
                try
                {
                    _context.RemoveRange(entities);

                    if (Options.AutoCommit)
                    {
                        PerformCommit();
                    }
                }
                catch (Exception ex)
                {
                    if (Options.AutoCommit)
                    {
                        PerformRollback();
                    }

                    throw new DataException($"Error deleting entities of type '{typeof(TEntity).Name}': {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override void Delete(Expression<Func<TEntity, Boolean>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException($"Argument '{nameof(expression)}' cannot be null or empty", nameof(expression));
            }

            lock (_syncLock)
            {
                try
                {
                    var entities = _context.Set<TEntity>()
                                           .Where(expression);

                    _context.RemoveRange(entities);

                    if (Options.AutoCommit)
                    {
                        PerformCommit();
                    }
                }
                catch (Exception ex)
                {
                    if (Options.AutoCommit)
                    {
                        PerformRollback();
                    }

                    throw new DataException($"Error deleting entities of type '{typeof(TEntity).Name}' by expression: {ex.Message}", ex);
                }
            }
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// Indicate if object is currently freeing, releasing, or resetting unmanaged resources.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            if (disposing)
            {
                _context = null;
            }

            _disposed = true;
        }
        /// <inheritdoc/>
        public override void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException($"Argument '{nameof(entity)}' cannot be null or empty", nameof(entity));
            }

            lock (_syncLock)
            {
                try
                {
                    _context.Add(entity);

                    if (Options.AutoCommit)
                    {
                        PerformCommit();
                    }
                }
                catch (Exception ex)
                {
                    if (Options.AutoCommit)
                    {
                        PerformRollback();
                    }

                    throw new DataException($"Error inserting entity of type '{typeof(TEntity).Name}': {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentException($"Argument '{nameof(entities)}' cannot be null or empty", nameof(entities));
            }

            lock (_syncLock)
            {
                try
                {
                    _context.AddRange(entities);

                    if (Options.AutoCommit)
                    {
                        PerformCommit();
                    }
                }
                catch (Exception ex)
                {
                    if (Options.AutoCommit)
                    {
                        PerformRollback();
                    }

                    throw new DataException($"Error inserting entities of type '{typeof(TEntity).Name}': {ex.Message}", ex);
                }
            }
        }
        /// <summary>
        /// Configure the database (and other options) to be used for the context.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        protected internal abstract void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
        /// <summary>
        /// Configure the model that was discovered by convention from the entity types exposed in Microsoft.EntityFrameworkCore.DbSet`1 properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected internal abstract void OnModelCreating(ModelBuilder modelBuilder);
        /// <summary>
        /// Perform commit operations.
        /// </summary>
        private void PerformCommit()
        {
            try
            {
                _context.SaveChanges(true);
            }
            catch (Exception ex)
            {
                throw new DataException($"Error commiting changes in repository with entities of type '{typeof(TEntity).Name}': {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Perform rollback operations.
        /// </summary>
        private void PerformRollback()
        {
            _context?.Dispose();
            _context = new EntityFrameworkRepositoryContext<TEntity, TOptions>(this, Options);
        }
        /// <inheritdoc/>
        public override void Rollback()
        {
            lock (_syncLock)
            {
                PerformRollback();
            }
        }
        /// <inheritdoc/>
        public override IEnumerable<TEntity> Select()
        {
            lock (_syncLock)
            {
                try
                {
                    return _context.Set<TEntity>()
                                   .ToList();
                }
                catch (Exception ex)
                {
                    throw new DataException($"Error selecting all entities of type '{typeof(TEntity).Name}': {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException($"Argument '{nameof(expression)}' cannot be null or empty", nameof(expression));
            }

            lock (_syncLock)
            {
                try
                {
                    return _context.Set<TEntity>()
                                   .Where(expression)
                                   .ToList();
                }
                catch (Exception ex)
                {
                    throw new DataException($"Error selecting entities of type '{typeof(TEntity).Name}' by expression: {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip)
        {
            if (expression == null)
            {
                throw new ArgumentException($"Argument '{nameof(expression)}' cannot be null or empty", nameof(expression));
            }

            if (skip < 0)
            {
                throw new ArgumentException($"Argument '{nameof(skip)}' must be major or equals than 0", nameof(skip));
            }

            lock (_syncLock)
            {
                try
                {
                    return _context.Set<TEntity>()
                                   .Where(expression)
                                   .Skip<TEntity>(skip)
                                   .ToList();
                }
                catch (Exception ex)
                {
                    throw new DataException($"Error selecting entities of type '{typeof(TEntity).Name}' by expression: {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip, Int32 take)
        {
            if (expression == null)
            {
                throw new ArgumentException($"Argument '{nameof(expression)}' cannot be null or empty", nameof(expression));
            }

            if (skip < 0)
            {
                throw new ArgumentException($"Argument '{nameof(skip)}' must be major or equals than 0", nameof(skip));
            }

            if (take <= 0)
            {
                throw new ArgumentException($"Argument '{nameof(take)}' must be major than 0", nameof(take));
            }

            lock (_syncLock)
            {
                try
                {
                    return _context.Set<TEntity>()
                                   .Where(expression)
                                   .Skip<TEntity>(skip)
                                   .Take<TEntity>(take)
                                   .ToList();
                }
                catch (Exception ex)
                {
                    throw new DataException($"Error selecting entities of type '{typeof(TEntity).Name}' by expression: {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException($"Argument '{nameof(entity)}' cannot be null or empty", nameof(entity));
            }

            lock (_syncLock)
            {
                try
                {
                    _context.Update(entity);

                    if (Options.AutoCommit)
                    {
                        PerformCommit();
                    }
                }
                catch (Exception ex)
                {
                    if (Options.AutoCommit)
                    {
                        PerformRollback();
                    }

                    throw new DataException($"Error updating entity of type '{typeof(TEntity).Name}': {ex.Message}", ex);
                }
            }
        }
        /// <inheritdoc/>
        public override void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentException($"Argument '{nameof(entities)}' cannot be null or empty", nameof(entities));
            }

            lock (_syncLock)
            {
                try
                {
                    _context.UpdateRange(entities);

                    if (Options.AutoCommit)
                    {
                        PerformCommit();
                    }
                }
                catch (Exception ex)
                {
                    if (Options.AutoCommit)
                    {
                        PerformRollback();
                    }

                    throw new DataException($"Error updating entities of type '{typeof(TEntity).Name}': {ex.Message}", ex);
                }
            }
        }
    }
}

using Accelerate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Represents a session with the database and can be used to query and save instances of your entities.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Type of data entity managed by the context.
    /// </typeparam>
    /// <typeparam name="TOptions">
    /// Type of configuration options.
    /// </typeparam>
    internal class EntityFrameworkRepositoryContext<TEntity, TOptions> : DbContext where TEntity : Entity, new()
                                                                                          where TOptions : EntityFrameworkRepositoryOptions
    {
        private Boolean _disposed;
        private readonly TOptions _options;
        private readonly EntityFrameworkRepository<TEntity, TOptions> _repository;

        /// <summary>
        /// Initialize a new instance of class <seealso cref="EntityFrameworkRepositoryContext{TEntity, TOptions}" />.
        /// </summary>
        /// <param name="repository">
        /// Repository managed in the context.
        /// </param>
        /// <param name="options">
        /// Configuration options of repository.
        /// </param>
        public EntityFrameworkRepositoryContext(EntityFrameworkRepository<TEntity, TOptions> repository, TOptions options)
        {
            _options = options ?? throw new ArgumentException($"Argument '{nameof(options)}' cannot be null or empty", nameof(options));
            _repository = repository ?? throw new ArgumentException($"Argument '{nameof(repository)}' cannot be null or empty", nameof(repository));

            ChangeTracker.AutoDetectChangesEnabled = _options.DetectChanges;
            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
            ChangeTracker.LazyLoadingEnabled = _options.LazyLoading;
            ChangeTracker.QueryTrackingBehavior = _options.TrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking;

#if NET7_0
            Database.AutoTransactionBehavior = _options.AutoTransactions ? AutoTransactionBehavior.Always : AutoTransactionBehavior.Never;
#else
            Database.AutoTransactionsEnabled = _options.AutoTransactions;
#endif
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
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
                throw new ObjectDisposedException(this.GetType().Name);
            }

            _disposed = true;
        }
        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(x => { x.Default(WarningBehavior.Log); })
                          .EnableDetailedErrors(_options.DetailedErrors)
                          .EnableSensitiveDataLogging(_options.SensitiveDataLogging);

            _repository.OnConfiguring(optionsBuilder);
        }
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _repository.OnModelCreating(modelBuilder);
        }
    }
}

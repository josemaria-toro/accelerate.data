using Accelerate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Data repository based on MySQL.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Type of data entity managed by repository.
    /// </typeparam>
    /// <typeparam name="TOptions">
    /// Type of configuration options.
    /// </typeparam>
    public abstract class MySQLRepository<TEntity, TOptions> : EntityFrameworkRepository<TEntity, TOptions> where TEntity : Entity, new()
                                                                                                            where TOptions : MySQLRepositoryOptions
    {
        /// <summary>
        /// Initialize a new instance of class <seealso cref="MySQLRepository{TEntity, TOptions}" />.
        /// </summary>
        /// <param name="options">
        /// Configuration options of data repository.
        /// </param>
        protected MySQLRepository(IOptions<TOptions> options) : base(options)
        {
        }

        /// <inheritdoc/>
        protected internal override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Options.ConnectionString, options =>
            {
                options.CommandTimeout(Options.Timeout);
            });
        }
    }
}

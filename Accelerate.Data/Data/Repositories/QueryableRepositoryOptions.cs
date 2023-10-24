using System;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Configuration options for queryable repository.
    /// </summary>
    public class QueryableRepositoryOptions
    {
        /// <summary>
        /// Database connection string.
        /// </summary>
        public virtual String ConnectionString { get; set; }
    }
}

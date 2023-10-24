using System;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Configuration options for data repository.
    /// </summary>
    public class MongoRepositoryOptions : QueryableRepositoryOptions
    {
        /// <summary>
        /// Collection name.
        /// </summary>
        public virtual String Collection { get; set; }
        /// <summary>
        /// Database name.
        /// </summary>
        public virtual String Database { get; set; }
    }
}

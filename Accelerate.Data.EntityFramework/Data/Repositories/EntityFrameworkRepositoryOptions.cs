using System;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Configuration options for data repository based on entity framework.
    /// </summary>
    public class EntityFrameworkRepositoryOptions : QueryableRepositoryOptions
    {
        /// <summary>
        /// Flag for call commit operation automatically.
        /// </summary>
        public Boolean AutoCommit { get; set; }
        /// <summary>
        /// Flag for auto start transactions.
        /// </summary>
        public Boolean AutoTransactions { get; set; }
        /// <summary>
        /// Flag for enabling log detailed errors.
        /// </summary>
        public Boolean DetailedErrors { get; set; }
        /// <summary>
        /// Flag for detect data changes.
        /// </summary>
        public Boolean DetectChanges { get; set; }
        /// <summary>
        /// Flag for enabling lazy loading of data.
        /// </summary>
        public Boolean LazyLoading { get; set; }
        /// <summary>
        /// Flag for enabling sensitive data logging.
        /// </summary>
        public Boolean SensitiveDataLogging { get; set; }
        /// <summary>
        /// Timeout, in seconds, for query execution.
        /// </summary>
        public Int32 Timeout { get; set; }
        /// <summary>
        /// Flag for enabling the changes tracking.
        /// </summary>
        public Boolean TrackChanges { get; set; }
    }
}

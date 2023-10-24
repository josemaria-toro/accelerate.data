using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Accelerate.Data.Entities
{
    /// <summary>
    /// Base class for data entities based on Mongo DB.
    /// </summary>
    public abstract class MongoEntity : Entity
    {
        /// <summary>
        /// Initialize a new instance of <see cref="MongoEntity" /> class.
        /// </summary>
        protected MongoEntity() : base()
        {

        }

        /// <summary>
        /// Initialize a new instance of <seealso cref="MongoEntity" /> class.
        /// </summary>
        /// <param name="info">
        /// The <seealso cref="SerializationInfo" /> to populate with data.
        /// </param>
        /// <param name="context">
        /// The destination (see <seealso cref="StreamingContext" />) for this serialization.
        /// </param>
        [ExcludeFromCodeCoverage]
        [Obsolete("This constructor is obsolete for security reasons")]
        protected MongoEntity(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Unique id of entity.
        /// </summary>
        [JsonPropertyName("_id")]
        public Guid Id { get; set; }
    }
}

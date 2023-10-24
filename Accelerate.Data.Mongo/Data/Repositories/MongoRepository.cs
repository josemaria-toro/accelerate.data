using Accelerate.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Accelerate.Data.Repositories
{
    /// <summary>
    /// Data repository based on Mongo DB.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Type of data entity managed by repository.
    /// </typeparam>
    /// <typeparam name="TOptions">
    /// Type of configuration options.
    /// </typeparam>
    public abstract class MongoRepository<TEntity, TOptions> : QueryableRepository<TEntity, TOptions> where TEntity : MongoEntity, new()
                                                                                                      where TOptions : MongoRepositoryOptions
    {
        private IMongoClient _client;
        private IMongoCollection<TEntity> _collection;
        private IMongoDatabase _database;
        private Boolean _disposed;

        /// <summary>
        /// Initialize a new instance of <seealso cref="MongoRepository{TEntity,TOptions}" /> class.
        /// </summary>
        /// <param name="options">
        /// Configuration options of data repository.
        /// </param>
        protected MongoRepository(IOptions<TOptions> options) : base(options)
        {
            var camelCaseConvention = new ConventionPack
            {
                new CamelCaseElementNameConvention()
            };

            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _client = new MongoClient(options.Value.ConnectionString);
            _database = _client.GetDatabase(options.Value.Database);
            _collection = _database.GetCollection<TEntity>(options.Value.Collection);
        }

        /// <inheritdoc />
        public override void Commit()
        {
        }
        /// <inheritdoc />
        public override void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException($"Argument '{nameof(entity)}' cannot be null or empty", nameof(entity));
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ValidationException("Property 'Id' cannot be null or empty", "Id");
            }

            try
            {
                var result = _collection.DeleteOne(x => x.Id == entity.Id);

                if (result.DeletedCount == 0)
                {
                    throw new NotFoundException($"Entity with id '{entity.Id}' was not found");
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataException($"Unhandled exception was thrown while deleting entity with id '{entity.Id}'", ex);
            }
        }
        /// <inheritdoc />
        public override void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentException($"Argument '{nameof(entities)}' cannot be null or empty", nameof(entities));
            }

            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
        /// <inheritdoc />
        public override void Delete(Expression<Func<TEntity, Boolean>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException($"Argument '{nameof(expression)}' cannot be null or empty", nameof(expression));
            }

            try
            {
                var entities = _collection.AsQueryable()
                                          .Where(expression)
                                          .ToList();

                foreach (var entity in entities)
                {
                    Delete(entity);
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (DataException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataException($"Unhandled exception was thrown while deleting entities by expression", ex);
            }
        }
        /// <inheritdoc />
        protected override void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            base.Dispose(disposing);

            if (disposing)
            {
                _client = null;
                _collection = null;
                _database = null;
            }

            _disposed = true;
        }
        /// <inheritdoc />
        public override void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException($"Argument '{nameof(entity)}' cannot be null or empty", nameof(entity));
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ValidationException("Property 'Id' cannot be null or empty", "Id");
            }

            try
            {
                _collection.InsertOne(entity);
            }
            catch (MongoWriteException ex)
            {
                throw new ConflictException($"Entity with id '{entity.Id}' already exists", ex);
            }
            catch (Exception ex)
            {
                throw new DataException("Unhandled exception was thrown while inserting entity", ex);
            }
        }
        /// <inheritdoc />
        public override void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentException($"Argument '{nameof(entities)}' cannot be null or empty", nameof(entities));
            }

            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }
        /// <inheritdoc />
        public override void Rollback()
        {

        }
        /// <inheritdoc />
        public override IEnumerable<TEntity> Select()
        {
            try
            {
                var entities = _collection.AsQueryable()
                                          .ToList();

                return entities;
            }
            catch (Exception ex)
            {
                throw new DataException("Error selecting all entities in repository", ex);
            }
        }
        /// <inheritdoc />
        public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression)
        {
            try
            {
                var entities = _collection.AsQueryable()
                                          .Where(expression)
                                          .ToList();

                return entities;
            }
            catch (Exception ex)
            {
                throw new DataException("Error selecting entities by expression", ex);
            }
        }
        /// <inheritdoc />
        public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip)
        {
            try
            {
                var entities = _collection.AsQueryable()
                                          .Where(expression)
                                          .Skip(skip)
                                          .ToList();

                return entities;
            }
            catch (Exception ex)
            {
                throw new DataException("Error selecting entities by expression", ex);
            }
        }
        /// <inheritdoc />
        public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip, Int32 take)
        {
            try
            {
                var entities = _collection.AsQueryable()
                                          .Where(expression)
                                          .Skip(skip)
                                          .Take(take)
                                          .ToList();

                return entities;
            }
            catch (Exception ex)
            {
                throw new DataException("Error selecting entities by expression", ex);
            }
        }
        /// <inheritdoc />
        public override void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException($"Argument '{nameof(entity)}' cannot be null or empty", nameof(entity));
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ValidationException("Property 'Id' cannot be null or empty", "Id");
            }

            try
            {
                var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
                var response = _collection.ReplaceOne(filter, entity);

                if (!response.IsAcknowledged)
                {
                    throw new DataException($"Something was wrong updating the entity with id '{entity.Id}'");
                }

                if (response.IsModifiedCountAvailable)
                {
                    if (response.ModifiedCount == 0)
                    {
                        throw new NotFoundException($"Entity with id '{entity.Id}' was not found");
                    }
                }
                else if (response.MatchedCount == 0)
                {
                    throw new NotFoundException($"Entity with id '{entity.Id}' was not found");
                }
            }
            catch (DataException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataException("Unhandled exception was thrown while inserting entity", ex);
            }
        }
        /// <inheritdoc />
        public override void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentException($"Argument '{nameof(entities)}' cannot be null or empty", nameof(entities));
            }

            foreach (var entity in entities)
            {
                Update(entity);
            }
        }
    }
}

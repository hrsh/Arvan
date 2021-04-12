using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;

namespace Arvan.Api
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity>
        where TEntity : class
    {
        private readonly MongoOptions _options;
        private readonly IMongoCollection<TEntity> _collections;
        private readonly ILogger<MongoRepository<TEntity>> _logger;
        private readonly FilterDefinitionBuilder<TEntity> _filterBuilder =
            Builders<TEntity>.Filter;

        public MongoRepository(
            IMongoClient client,
            IOptions<MongoOptions> options,
            ILogger<MongoRepository<TEntity>> logger)
        {
            _options = options.Value;
            _logger = logger;

            _logger.LogInformation(options.Value.ConnectionString);

            var database = client.GetDatabase(options.Value.Database);
            _collections = database.GetCollection<TEntity>(options.Value.Collection);
        }

        public void Create(TEntity model)
        {
            _collections.InsertOne(model);
        }

        public Task CreateAsync(TEntity model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate
            , CancellationToken ct = default)
        {
            await _collections.DeleteOneAsync(predicate, ct);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            try
            {
                var t = await _collections.Find(predicate).FirstOrDefaultAsync();
                return t;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task<TEntity> FindAsync<TField>(
            Expression<Func<TEntity, TField>> predicate,
            TField fieldValue,
            CancellationToken ct = default)
        {
            var filter = _filterBuilder.Eq(predicate, fieldValue);
            var t = await _collections.FindAsync(filter, cancellationToken: ct);
            return await t.FirstOrDefaultAsync(cancellationToken: ct);
        }

        public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _collections.AsQueryable().Where(predicate).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task UpdateAsync(
            Expression<Func<TEntity, bool>> predicate,
            TEntity model,
            CancellationToken ct = default)
        {
            await _collections.ReplaceOneAsync(
                predicate,
                model,
                cancellationToken: ct);
        }

        public async Task UpdateAsync<TField>(
            TField fieldId,
            Expression<Func<TEntity, TField>> search,
            TEntity model,
            CancellationToken ct = default)
        {
            var filter = _filterBuilder.Eq(search, fieldId);
            await _collections.ReplaceOneAsync(
                filter,
                model,
                cancellationToken: ct);
        }
    }
}
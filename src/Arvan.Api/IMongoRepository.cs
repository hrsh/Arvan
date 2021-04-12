using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Arvan.Api
{
    public interface IMongoRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

        Task<TEntity> FindAsync<TField>(Expression<Func<TEntity, TField>> predicate,
            TField fieldValue,
            CancellationToken ct = default);

        IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate);

        void Create(TEntity model);

        Task CreateAsync(TEntity model, CancellationToken ct = default);

        Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task UpdateAsync(
            Expression<Func<TEntity, bool>> predicate,
            TEntity model,
            CancellationToken ct = default);

        Task UpdateAsync<TField>(
            TField fieldId,
            Expression<Func<TEntity, TField>> search,
            TEntity model,
            CancellationToken ct = default);
    }
}
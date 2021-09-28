using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Domain.Abstractions
{
    public interface IRepository<TAggregateRoot, in TKey>
        where TKey : IEquatable<TKey>
        where TAggregateRoot : Entity<TKey>, IAggregateRoot
    {
        Task<TAggregateRoot> FindOrDefaultAsync(
            TKey id,
            CancellationToken cancellationToken);

        Task<TAggregateRoot> AddAsync(
            TAggregateRoot entity,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            TAggregateRoot entity,
            CancellationToken cancellationToken);
    }
}
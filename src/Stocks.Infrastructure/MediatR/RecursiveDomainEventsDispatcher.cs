using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Stocks.Domain.Abstractions;
using Stocks.Infrastructure.EntityFrameworkCore;

namespace Stocks.Infrastructure.MediatR
{
    /// <summary>
    /// Рекурсивно отправляет доменные события обработчикам
    /// </summary>
    public class RecursiveDomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly StockDbContext _dbContext;
        private readonly IMediator _mediator;

        public RecursiveDomainEventsDispatcher(StockDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task DispatchAllAsync(CancellationToken cancellationToken)
        {
            List<EntityEntry<IHaveEvents>> entries = _dbContext.ChangeTracker
                .Entries<IHaveEvents>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            if (!entries.Any())
            {
                return;
            }

            foreach (EntityEntry<IHaveEvents> entry in entries)
            {
                INotification notification = entry.Entity.DomainEvents.Dequeue();

                await _mediator.Publish(notification, cancellationToken);
            }

            await DispatchAllAsync(cancellationToken);
        }
    }
}
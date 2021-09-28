using System;
using System.Collections.Generic;
using MediatR;
using Stocks.Domain.Abstractions;
using Stocks.Domain.Events;

namespace Stocks.Domain.StockItems
{
    public class StockItem : Entity<Guid>, IAggregateRoot, IHaveEvents
    {
        /// <inheritdoc />
        private StockItem()
            : base(default)
        {
            State = new StockItemState();
            DomainEvents = new Queue<INotification>();
        }

        /// <inheritdoc />
        public StockItem(Guid stockId, Guid productId, StockItemPosition position)
            : this()
        {
            StockId = stockId;
            ProductId = productId;
            Position = position;
        }

        public Guid StockId { get; private set; }

        public Guid ProductId { get; private set; }

        public StockItemPosition Position { get; private set; }

        public StockItemState State { get; private set; }

        public void Move(StockItemPosition position)
        {
            Position = position;
        }

        public void Reserve()
        {
            var @event = new StockItemReserved(this.Id);
            DomainEvents.Enqueue(@event);
        }

        /// <inheritdoc />
        public Queue<INotification> DomainEvents { get; }
    }
}
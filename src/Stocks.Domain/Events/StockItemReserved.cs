using System;
using MediatR;

namespace Stocks.Domain.Events
{
    public class StockItemReserved : INotification
    {
        public StockItemReserved(Guid stockItemId)
        {
            StockItemId = stockItemId;
        }

        public Guid StockItemId { get; }
    }
}
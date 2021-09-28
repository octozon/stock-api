using System;
using Stocks.Domain.Abstractions;

namespace Stocks.Domain.StockItems
{
    public interface IStockItemRepository : IRepository<StockItem, Guid>
    {
        
    }
}
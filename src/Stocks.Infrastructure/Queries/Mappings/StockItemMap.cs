using System;
using System.Linq.Expressions;
using Stocks.Application.Queries.StockItems;
using Stocks.Application.Queries.Stocks;
using Stocks.Domain.StockItems;
using Stocks.Domain.Stocks;

namespace Stocks.Infrastructure.Queries.Mappings
{
    public class StockItemMap
    {
        public static Expression<Func<StockItem, StockItemView>> ToStockItemView
            => stock => new StockItemView
            {
                Id = stock.Id,
                ProductId = stock.ProductId,
                Position = stock.Position.Number,
                State = stock.State.CurrentState
            };

        public static Expression<Func<StockItem, StockItemDetailedView>> ToStockItemDetailedView
            => stock => new StockItemDetailedView
            {
                Id = stock.Id,
                ProductId = stock.ProductId,
                Position = stock.Position.Number,
                State = stock.State.CurrentState
            };
    }
}
using System;
using System.Linq.Expressions;
using Stocks.Application.Queries.Stocks;
using Stocks.Domain.Stocks;

namespace Stocks.Infrastructure.Queries.Mappings
{
    internal static class StockMap
    {
        public static Expression<Func<Stock, StockView>> ToStockView
            => stock => new StockView
            {
                Id = stock.Id,
                Name = stock.Name
            };

        public static Expression<Func<Stock, StockDetailedView>> ToStockDetailedView
            => stock => new StockDetailedView
            {
                Id = stock.Id,
                Name = stock.Name
            };
    }
}
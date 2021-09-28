using System;

namespace Stocks.Application.Commands.CreateStock
{
    public class CreateStockResult
    {
        public CreateStockResult(Guid stockId)
        {
            StockId = stockId;
        }

        public Guid StockId { get; }
    }
}
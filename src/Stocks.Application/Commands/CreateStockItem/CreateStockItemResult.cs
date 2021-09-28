using System;

namespace Stocks.Application.Commands.CreateStockItem
{
    public class CreateStockItemResult
    {
        public CreateStockItemResult(Guid stockItemId)
        {
            StockItemId = stockItemId;
        }

        public Guid StockItemId { get; }
    }
}
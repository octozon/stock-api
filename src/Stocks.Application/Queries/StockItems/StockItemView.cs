using System;

namespace Stocks.Application.Queries.StockItems
{
    public class StockItemView
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Position { get; set; }

        public string State { get; set; }
    }
}
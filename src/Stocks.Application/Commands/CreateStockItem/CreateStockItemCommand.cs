using System;
using MediatR;

namespace Stocks.Application.Commands.CreateStockItem
{
    public class CreateStockItemCommand : IRequest<CreateStockItemResult>
    {
        public Guid StockId { get; set; }

        public Guid ProductId { get; set; }

        public string Position { get; set; }
    }
}
using System;
using MediatR;

namespace Stocks.Application.Commands.MoveStockItem
{
    public class MoveStockItemCommand : IRequest
    {
        public Guid StockItemId { get; set; }

        public string PositionNumber { get; set; }
    }
}
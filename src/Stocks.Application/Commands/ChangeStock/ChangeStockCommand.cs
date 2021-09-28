using System;
using MediatR;

namespace Stocks.Application.Commands.ChangeStock
{
    public class ChangeStockCommand : IRequest
    {
        public Guid StockId { get; set; }

        public string Name { get; set; }
    }
}
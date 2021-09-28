using MediatR;

namespace Stocks.Application.Commands.CreateStock
{
    public class CreateStockCommand : IRequest<CreateStockResult>
    {
        public string Name { get; init; }
    }
}
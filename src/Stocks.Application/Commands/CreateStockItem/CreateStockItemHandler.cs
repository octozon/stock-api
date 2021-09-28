using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Stocks.Domain.StockItems;

namespace Stocks.Application.Commands.CreateStockItem
{
    [UsedImplicitly]
    public class CreateStockItemHandler : IRequestHandler<CreateStockItemCommand, CreateStockItemResult>
    {
        private readonly IStockItemRepository _stockItemRepository;

        public CreateStockItemHandler(IStockItemRepository stockItemRepository)
        {
            _stockItemRepository = stockItemRepository;
        }

        /// <inheritdoc />
        public async Task<CreateStockItemResult> Handle(CreateStockItemCommand request, CancellationToken cancellationToken)
        {
            var stockItemPosition = new StockItemPosition(request.Position);
            var stockItem = new StockItem(request.StockId, request.ProductId, stockItemPosition);

            await _stockItemRepository.AddAsync(stockItem, cancellationToken);

            return new CreateStockItemResult(stockItem.Id);
        }
    }
}
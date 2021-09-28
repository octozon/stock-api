using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Stocks.Domain.StockItems;

namespace Stocks.Application.Commands.MoveStockItem
{
    [UsedImplicitly]
    public class MoveStockItemHandler : IRequestHandler<MoveStockItemCommand>
    {
        private readonly IStockItemRepository _stockItemRepository;

        public MoveStockItemHandler(IStockItemRepository stockItemRepository)
        {
            _stockItemRepository = stockItemRepository;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(MoveStockItemCommand request, CancellationToken cancellationToken)
        {
            StockItem item = await _stockItemRepository.FindOrDefaultAsync(request.StockItemId, cancellationToken);
            
            var newPosition = new StockItemPosition(request.PositionNumber);
            
            item.Move(newPosition);
            
            return Unit.Value;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Stocks.Domain.Stocks;

namespace Stocks.Application.Commands.ChangeStock
{
    [UsedImplicitly]
    public class ChangeStockHandler : IRequestHandler<ChangeStockCommand>
    {
        private readonly IStockRepository _stockRepository;

        public ChangeStockHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(ChangeStockCommand request, CancellationToken cancellationToken)
        {
            Stock stock = await _stockRepository.FindOrDefaultAsync(request.StockId, cancellationToken);
            
            stock.ChangeName(request.Name);
            
            return Unit.Value;
        }
    }
}
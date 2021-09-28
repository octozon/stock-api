using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Stocks.Domain.Stocks;

namespace Stocks.Application.Commands.CreateStock
{
    [UsedImplicitly]
    public class CreateStockHandler : IRequestHandler<CreateStockCommand, CreateStockResult>
    {
        private readonly IStockRepository _stockRepository;

        public CreateStockHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        /// <inheritdoc />
        public async Task<CreateStockResult> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = new Stock(request.Name);

            await _stockRepository.AddAsync(stock, cancellationToken);

            return new CreateStockResult(stock.Id);
        }
    }
}
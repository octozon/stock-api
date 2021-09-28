using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Application.Queries.Stocks
{
    public interface IStockQueries
    {
        Task<PagedCollection<StockView>> GetAllAsync(PageSize pageSize, CancellationToken cancellationToken);

        Task<StockDetailedView> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
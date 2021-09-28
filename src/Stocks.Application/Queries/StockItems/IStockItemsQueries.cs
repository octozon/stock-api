using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Application.Queries.StockItems
{
    public interface IStockItemsQueries
    {
        Task<PagedCollection<StockItemView>> GetAllAsync(PageSize pageSize, CancellationToken cancellationToken);

        Task<StockItemDetailedView> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
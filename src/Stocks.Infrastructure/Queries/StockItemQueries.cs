using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stocks.Application.Queries;
using Stocks.Application.Queries.StockItems;
using Stocks.Infrastructure.EntityFrameworkCore;
using Stocks.Infrastructure.Queries.Mappings;

namespace Stocks.Infrastructure.Queries
{
    internal sealed class StockItemQueries : IStockItemsQueries
    {
        private readonly StockDbContext _dbContext;

        public StockItemQueries(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<PagedCollection<StockItemView>> GetAllAsync(PageSize pageSize, CancellationToken cancellationToken)
        {
            StockItemView[] stockItems = await _dbContext
                .StockItems
                .Skip((pageSize.Page - 1) * pageSize.Size)
                .Take(pageSize.Size)
                .Select(StockItemMap.ToStockItemView)
                .ToArrayAsync(cancellationToken);

            int total = await _dbContext.StockItems.CountAsync(cancellationToken);

            return new PagedCollection<StockItemView>(pageSize.Size, total, stockItems);
        }

        /// <inheritdoc />
        public async Task<StockItemDetailedView> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            StockItemDetailedView stockItem = await _dbContext
                .StockItems
                .Select(StockItemMap.ToStockItemDetailedView)
                .SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

            return stockItem;
        }
    }
}
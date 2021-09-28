using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stocks.Application.Queries;
using Stocks.Application.Queries.Stocks;
using Stocks.Infrastructure.EntityFrameworkCore;
using Stocks.Infrastructure.Queries.Mappings;

namespace Stocks.Infrastructure.Queries
{
    internal sealed class StockQueries : IStockQueries
    {
        private readonly StockDbContext _dbContext;

        public StockQueries(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<PagedCollection<StockView>> GetAllAsync(PageSize pageSize, CancellationToken cancellationToken)
        {
            StockView[] stocks = await _dbContext
                .Stocks
                .Skip((pageSize.Page - 1) * pageSize.Size)
                .Take(pageSize.Size)
                .Select(StockMap.ToStockView)
                .ToArrayAsync(cancellationToken);

            int total = await _dbContext
                .Stocks
                .CountAsync(cancellationToken);

            return new PagedCollection<StockView>(stocks.Length, total, stocks);
        }

        /// <inheritdoc />
        public async Task<StockDetailedView> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            StockDetailedView stock = await _dbContext
                .Stocks
                .Select(StockMap.ToStockDetailedView)
                .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);

            return stock;
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Stocks.Domain.StockItems;
using Stocks.Infrastructure.EntityFrameworkCore;

namespace Stocks.Infrastructure.Repositories
{
    internal sealed class StockItemsRepository : IStockItemRepository
    {
        private readonly StockDbContext _dbContext;

        public StockItemsRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<StockItem> FindOrDefaultAsync(Guid id, CancellationToken cancellationToken)
        {
            StockItem stockItem = await _dbContext
                .StockItems
                .FindAsync(new object[] { id }, cancellationToken: cancellationToken);

            return stockItem;
        }

        /// <inheritdoc />
        public async Task<StockItem> AddAsync(StockItem entity, CancellationToken cancellationToken)
        {
            await _dbContext
                .StockItems
                .AddAsync(entity, cancellationToken);

            return entity;
        }

        /// <inheritdoc />
        public Task DeleteAsync(StockItem entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _dbContext
                .StockItems
                .Remove(entity);
            
            return Task.CompletedTask;
        }
    }
}
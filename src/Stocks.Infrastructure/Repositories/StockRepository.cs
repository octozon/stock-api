using System;
using System.Threading;
using System.Threading.Tasks;
using Stocks.Domain.Stocks;
using Stocks.Infrastructure.EntityFrameworkCore;

namespace Stocks.Infrastructure.Repositories
{
    internal sealed class StockRepository : IStockRepository
    {
        private readonly StockDbContext _dbContext;

        public StockRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<Stock> FindOrDefaultAsync(Guid id, CancellationToken cancellationToken)
        {
            Stock stock = await _dbContext
                .Stocks
                .FindAsync(new object[] { id }, cancellationToken: cancellationToken);

            return stock;
        }

        /// <inheritdoc />
        public async Task<Stock> AddAsync(Stock entity, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(entity, cancellationToken);

            return entity;
        }

        /// <inheritdoc />
        public Task DeleteAsync(Stock entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _dbContext.Remove(entity);
            
            return Task.CompletedTask;
        }
    }
}
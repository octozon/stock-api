using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Stocks.Domain.StockItems;
using Stocks.Domain.Stocks;
using Stocks.Infrastructure.EntityFrameworkCore.Mappings;

namespace Stocks.Infrastructure.EntityFrameworkCore
{
    public class StockDbContext : DbContext
    {
        /// <inheritdoc />
        public StockDbContext([NotNull] DbContextOptions<StockDbContext> options)
            : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; private set; }

        public DbSet<StockItem> StockItems { get; private set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StockMap());
            modelBuilder.ApplyConfiguration(new StockItemMap());
        }
    }
}
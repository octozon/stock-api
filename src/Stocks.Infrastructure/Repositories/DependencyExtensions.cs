using Microsoft.Extensions.DependencyInjection;
using Stocks.Domain.StockItems;
using Stocks.Domain.Stocks;

namespace Stocks.Infrastructure.Repositories
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockItemRepository, StockItemsRepository>();

            return services;
        }
    }
}
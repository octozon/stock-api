using Microsoft.Extensions.DependencyInjection;
using Stocks.Application.Queries.StockItems;
using Stocks.Application.Queries.Stocks;

namespace Stocks.Infrastructure.Queries
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddScoped<IStockQueries, StockQueries>();
            services.AddScoped<IStockItemsQueries, StockItemQueries>();

            return services;
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Stocks.Infrastructure.EntityFrameworkCore
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddStockDbContext(this IServiceCollection services, StockDbContextSettings settings)
        {
            services.AddDbContext<StockDbContext>(options =>
            {
                options.UseNpgsql(settings.ConnectionString, builder =>
                {
                    builder.MigrationsHistoryTable("VersionInfo", "dbo");
                    builder.EnableRetryOnFailure(
                        maxRetryCount: settings.MaxRetryCount,
                        maxRetryDelay: settings.MaxRetryDelay,
                        errorCodesToAdd: null);
                });

                options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }

    public class StockDbContextSettings
    {
        public string ConnectionString { get; set; }

        public int MaxRetryCount { get; set; }

        public TimeSpan MaxRetryDelay { get; set; }
    }
}
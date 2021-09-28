using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Stocks.Infrastructure.EntityFrameworkCore;
using Stocks.Infrastructure.MediatR;
using Stocks.Infrastructure.MediatR.Behaviors;
using Stocks.Infrastructure.Queries;
using Stocks.Infrastructure.Repositories;

namespace Stocks.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });
            
            var retryPublishSettings = _configuration
                .GetSection("RetryPublishMediatorSettings")
                .Get<RetryPublishMediatorSettings>();

            var retryBehaviorSettings = _configuration
                .GetSection("RetryBehaviorSettings")
                .Get<RetryBehaviorSettings>();

            services.AddMediator()
                .AddRetryPublish(retryPublishSettings)
                .AddLoggingBehavior()
                .AddValidationBehavior()
                .AddTransactionBehavior()
                .AddRetryBehavior(retryBehaviorSettings);
            
            var stockDbContextSettings = _configuration
                .GetSection("StockDbContextSettings")
                .Get<StockDbContextSettings>();

            services.AddStockDbContext(stockDbContextSettings)
                .AddRepositories()
                .AddQueries();
            
            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Octozon Stocks",
                    Version = "v1"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                configuration.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(configuration =>
            {
                configuration.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "Octozon Stocks");
                configuration.RoutePrefix = "docs";
            });
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
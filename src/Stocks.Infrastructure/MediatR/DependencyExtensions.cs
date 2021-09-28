using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Application.Commands.CreateStock;
using Stocks.Domain.Events;
using Stocks.Infrastructure.MediatR.Behaviors;

namespace Stocks.Infrastructure.MediatR
{
    public interface IMediatorDependencyBuilder
    {
        IServiceCollection Services { get; }
    }

    internal class MediatorDependencyBuilder : IMediatorDependencyBuilder
    {
        public MediatorDependencyBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
    
    public static class DependencyExtensions
    {
        public static IMediatorDependencyBuilder AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateStockCommand), typeof(StockItemReserved));

            return new MediatorDependencyBuilder(services);
        }

        public static IMediatorDependencyBuilder AddTransactionBehavior(this IMediatorDependencyBuilder builder)
        {
            builder.Services.AddScoped<IDomainEventsDispatcher, RecursiveDomainEventsDispatcher>();
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return builder;
        }
        
        public static IMediatorDependencyBuilder AddRetryPublish(this IMediatorDependencyBuilder builder, RetryPublishMediatorSettings settings)
        {
            builder.Services.AddSingleton(settings);
            builder.Services.AddTransient<IMediator, RetryPublishMediator>();

            return builder;
        }
        
        public static IMediatorDependencyBuilder AddValidationBehavior(this IMediatorDependencyBuilder builder)
        {
            AssemblyScanner
                .FindValidatorsInAssembly(typeof(CreateStockCommand).Assembly)
                .ForEach(assembly => builder.Services.AddScoped(typeof(IValidator), assembly.ValidatorType));

            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            return builder;
        }
        
        public static IMediatorDependencyBuilder AddLoggingBehavior(this IMediatorDependencyBuilder builder)
        {
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return builder;
        }
        
        public static IMediatorDependencyBuilder AddRetryBehavior(this IMediatorDependencyBuilder builder, RetryBehaviorSettings settings)
        {
            builder.Services.AddSingleton(settings);
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RetryBehavior<,>));

            return builder;
        }
    }
}
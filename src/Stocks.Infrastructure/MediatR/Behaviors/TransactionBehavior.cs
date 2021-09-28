using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Stocks.Infrastructure.EntityFrameworkCore;

namespace Stocks.Infrastructure.MediatR.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly StockDbContext _dbContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(
            StockDbContext dbContext,
            IDomainEventsDispatcher domainEventsDispatcher,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext;
            _domainEventsDispatcher = domainEventsDispatcher;
            _logger = logger;
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "HeapView.CanAvoidClosure")]
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            
            if (_dbContext.Database.CurrentTransaction != null)
            {
                _logger.LogWarning($"Transaction {_dbContext.Database.CurrentTransaction.TransactionId} already started.");
                return await next();
            }

            IExecutionStrategy strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                _logger.LogInformation($"------ Transaction started {transaction.TransactionId} ------");
                
                try
                {
                    response = await next();

                    // Доменные события
                    await _domainEventsDispatcher.DispatchAllAsync(cancellationToken);
                    _logger.LogInformation("------ Domain events dispatched ------");

                    // Транзакция
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    _logger.LogInformation($"------ Transaction committed {transaction.TransactionId} ------");

                    // Интеграционные события
                    
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError($"------ Transaction rollback {transaction.TransactionId} ------");
                    throw;
                }
            });

            return response;
        }
    }
}
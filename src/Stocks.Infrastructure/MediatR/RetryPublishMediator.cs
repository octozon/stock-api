using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Stocks.Application.Exceptions;
using Stocks.Domain.Exceptions;

namespace Stocks.Infrastructure.MediatR
{
    public class RetryPublishMediator : Mediator
    {
        private readonly RetryPublishMediatorSettings _settings;
        private readonly ILogger<RetryPublishMediator> _logger;

        /// <inheritdoc />
        public RetryPublishMediator(
            [NotNull] ServiceFactory serviceFactory,
            RetryPublishMediatorSettings settings,
            ILogger<RetryPublishMediator> logger)
            : base(serviceFactory)
        {
            _settings = settings;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken)
        {
            Func<INotification, CancellationToken, Task>[] handlers = allHandlers.ToArray();
            
            if (!_settings.Enabled)
            {
                await base.PublishCore(handlers, notification, cancellationToken);
            }
            
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>(ex => ex switch
                {
                    BusinessRuleViolationException => false,
                    EntityException => false,
                    ValidationException => false,
                    _ => true
                })
                .WaitAndRetryAsync(_settings.RetryAttempts, attempt =>
                {
                    TimeSpan retryDelay = _settings.RetryWithExponentialBackoff
                        ? TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * _settings.RetryDelay)
                        : TimeSpan.FromMilliseconds(_settings.RetryDelay);

                    _logger.LogWarning(
                        $"------ {notification.GetType().Name} handler retrying {attempt}, waiting {retryDelay:g} ------");

                    return retryDelay;
                });

            await retryPolicy.ExecuteAsync(async () =>
            {
                await Task.WhenAll(handlers.Select(x => x(notification, cancellationToken)));
            });
        }
    }
}
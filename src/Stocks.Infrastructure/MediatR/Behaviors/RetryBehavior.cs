using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Stocks.Application.Exceptions;
using Stocks.Domain.Exceptions;
using Stocks.Infrastructure.Extensions;

namespace Stocks.Infrastructure.MediatR.Behaviors
{
    public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly RetryBehaviorSettings _settings;
        private readonly ILogger<RetryBehavior<TRequest, TResponse>> _logger;

        public RetryBehavior(RetryBehaviorSettings settings, ILogger<RetryBehavior<TRequest, TResponse>> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (!_settings.Enabled)
            {
                return await next();
            }
            
            AsyncRetryPolicy<TResponse> retryPolicy = Policy<TResponse>
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
                        $"------ Command {request.GetGenericTypeName()} retrying {attempt}, waiting {retryDelay:g} ------");

                    return retryDelay;
                });

            TResponse response = await retryPolicy.ExecuteAsync(async () => await next());

            return response;
        }
    }
}
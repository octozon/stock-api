using System;
using JetBrains.Annotations;

namespace Stocks.Domain.Exceptions
{
    public class BusinessRuleViolationException : Exception
    {
        /// <inheritdoc />
        public BusinessRuleViolationException([NotNull] string message)
            : base(message)
        {
        }
    }
}
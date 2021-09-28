using System;
using JetBrains.Annotations;

namespace Stocks.Application.Exceptions
{
    public abstract class EntityException : Exception
    {
        /// <inheritdoc />
        protected EntityException([CanBeNull] string message)
            : base(message)
        {
        }
    }
}
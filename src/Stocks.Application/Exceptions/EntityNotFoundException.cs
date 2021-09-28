using System;
using JetBrains.Annotations;

namespace Stocks.Application.Exceptions
{
    public abstract class EntityNotFoundException<TKey> : EntityException
        where TKey : IEquatable<TKey>
    {
        /// <inheritdoc />
        protected EntityNotFoundException(TKey entityId, [CanBeNull] string message)
            : base(message)
        {
            EntityId = entityId;
        }

        public TKey EntityId { get; }
    }
}
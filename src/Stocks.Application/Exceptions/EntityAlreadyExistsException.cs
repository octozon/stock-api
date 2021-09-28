using System;
using JetBrains.Annotations;

namespace Stocks.Application.Exceptions
{
    public abstract class EntityAlreadyExistsException<TKey> : EntityException
        where TKey : IEquatable<TKey>
    {
        /// <inheritdoc />
        protected EntityAlreadyExistsException(TKey entityId, [CanBeNull] string message)
            : base(message)
        {
            EntityId = entityId;
        }

        public TKey EntityId { get; }
    }
}
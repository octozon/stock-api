using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stocks.Domain.Abstractions
{
    public abstract class Entity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; private set; }

        protected Entity(TKey id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            if (((Entity<TKey>)obj).IsTransient() || IsTransient())
            {
                return false;
            }

            return Equals((Entity<TKey>)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !Equals(left, right);
        }

        private bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default);
        }

        private bool Equals(Entity<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }
    }
}
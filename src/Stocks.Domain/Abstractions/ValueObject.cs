using System;
using System.Collections.Generic;
using System.Linq;

namespace Stocks.Domain.Abstractions
{
    /// <summary>
    /// Объект-значение
    /// <remarks>
    /// Объекты-значения нельзя изменять (можно только удалить старый, добавить новый).
    /// Необходимо реализовать сравнение по полям: <see cref="GetEqualityFields"/>
    /// </remarks>
    /// </summary>
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityFields();
        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
        
            if (GetType() != obj.GetType())
            {
                return false;
            }
        
            var valueObject = (ValueObject)obj;
        
            return GetEqualityFields().SequenceEqual(valueObject.GetEqualityFields());
        }
        
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
        
            foreach (object equalityField in GetEqualityFields().Where(x => x != null))
            {
                hashCode.Add(equalityField);
            }
        
            return hashCode.ToHashCode();
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
}
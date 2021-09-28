using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Stocks.Domain.Abstractions
{
    public abstract class Enumeration
    {
        private static readonly ConcurrentDictionary<Type, Lazy<PropertyInfo[]>> Types = new();
        private static readonly ConcurrentDictionary<PropertyInfo, object> Properties = new();

        public int Id { get; private set; }

        public string Code { get; private set; }

        public string Name { get; private set; }

        protected Enumeration(int id, string code, string name)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException(
                    "Value cannot be null or empty.",
                    nameof(code));

            Id = id;
            Code = code;
            Name = name;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            return GetPropertyValues(typeof(T)).Cast<T>();
        }

        public static T FromId<T>(int id) where T : Enumeration
        {
            IEnumerable<T> items = GetAll<T>();

            return items.SingleOrDefault(i => i.Id == id);
        }

        public static T FromName<T>(string name) where T : Enumeration
        {
            IEnumerable<T> items = GetAll<T>();

            return items.SingleOrDefault(i =>
                string.Equals(i.Code, name, StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
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

            if (obj is not Enumeration other)
            {
                return false;
            }

            return Id == other.Id && Code == other.Code;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Code, Name);
        }

        public override string ToString()
        {
            return Name ?? Code;
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                {
                    return true;
                }

                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        private static IEnumerable<object> GetPropertyValues(Type type)
        {
            IEnumerable<PropertyInfo> properties = GetTypeProperties(type);

            return properties.Select(GetProperty);
        }

        private static object GetProperty(PropertyInfo property)
        {
            return Properties.GetOrAdd(
                property,
                _ => new Lazy<object>(() => property.GetValue(null)).Value);
        }

        private static IEnumerable<PropertyInfo> GetTypeProperties(Type type)
        {
            return Types.GetOrAdd(
                    type,
                    t =>
                        new Lazy<PropertyInfo[]>(() => t.GetProperties(
                            BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)))
                .Value;
        }
    }
}
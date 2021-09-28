using System;
using System.Linq;

namespace Stocks.Infrastructure.Extensions
{
    internal static class TypeExtensions
    {
        public static string GetGenericTypeName(this object target)
        {
            return GetGenericTypeName(target.GetType());
        }
        
        private static string GetGenericTypeName(Type type)
        {
            string typeName;

            if (type.IsGenericType)
            {
                string genericTypes = string.Join(
                    separator: ",",
                    value: type.GetGenericArguments().Select(t => t.Name).ToArray());
                
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }
    }
}
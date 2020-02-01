using System;
using System.Linq;
using System.Reflection;

namespace InvoicingSystem_SQLite.Logic.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            var propertyAttributes = property.CustomAttributes;
            var containsAttribute = propertyAttributes.Any(a => a.AttributeType == typeof(T));

            return containsAttribute;
        }
    }
}

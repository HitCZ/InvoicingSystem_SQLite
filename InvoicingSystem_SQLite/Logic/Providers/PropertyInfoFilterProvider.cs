using Invoicing.Attributes;
using InvoicingSystem_SQLite.Logic.Extensions;
using System;
using System.Reflection;

namespace InvoicingSystem_SQLite.Logic.Providers
{
    public static class PropertyInfoFilterProvider
    {
        public static Predicate<PropertyInfo> GetExcludeNotInDatabasePropertiesFilter()
        {
            return i => !i.HasAttribute<NotInDatabaseAttribute>();
        }
    }
}

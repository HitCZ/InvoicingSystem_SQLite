using Invoicing.Attributes;
using InvoicingSystem_SQLite.Logic.Extensions;
using System.Linq;

namespace InvoicingSystem_SQLite.Logic.Validators
{
    public static class DbValidator
    {
        /// <summary>
        /// If property with given name doesn't exist in the class or property has attribute
        /// <see cref="NotInDatabaseAttribute"/> it returns false.
        /// </summary>
        /// <typeparam name="T">Type of class with given property (column name).</typeparam>
        /// <param name="columnName">Name of the property, that should correspond with the column name in DB.</param>
        public static bool ColumnExists<T>(string columnName) where T : class
        {
            var typeOfT = typeof(T);
            var property = typeOfT.GetProperties().SingleOrDefault(p => p.Name == columnName);

            if (property is null)
                return false;

            var hasAttribute = property.HasAttribute<NotInDatabaseAttribute>();

            return !hasAttribute;
        }
    }
}

using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using Microsoft.Practices.ServiceLocation;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    public static class SqlProviderFactory<T> where T : IDatabaseStorableObject
    {
        public static SqlDataProvider<T> GetProvider()
        {
            if (typeof(T) == typeof(Invoice))
                return new InvoiceDataProvider(new QueryExecutor(), new TypeToTableMappingManager()) as SqlDataProvider<T>;

            var provider = ServiceLocator.Current.GetInstance<SqlDataProvider<T>>();

            return provider;
        }
    }
}

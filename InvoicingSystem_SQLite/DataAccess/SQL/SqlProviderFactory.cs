using Invoicing.Models;
using Microsoft.Practices.ServiceLocation;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    public static class SqlProviderFactory<T> where T : IDatabaseStorableObject
    {
        public static SqlDataProvider<T> GetProvider()
        {
            var provider = ServiceLocator.Current.GetInstance<SqlDataProvider<T>>();

            return provider;
        }
    }
}

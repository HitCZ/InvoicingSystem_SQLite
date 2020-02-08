using System.Collections.Generic;
using Invoicing.Models;

namespace InvoicingSystem_SQLite.DataAccess.QueryExecution
{
    public interface IQueryExecutor
    {
        string ConnectionString { get; }
        T ExecuteQueryWithSingleResult<T>(string query) where T : IDatabaseStorableObject;
        IEnumerable<T> ExecuteQueryWitMultipleResults<T>(string query) where T : IDatabaseStorableObject;
        bool ExecuteQueryWithFeedback(string query);
        bool ExecuteMultipleQueriesWithFeedback(List<string> queries);
    }
}

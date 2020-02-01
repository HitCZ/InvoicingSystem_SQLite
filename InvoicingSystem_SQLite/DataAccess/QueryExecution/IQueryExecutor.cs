using System.Collections.Generic;

namespace InvoicingSystem_SQLite.DataAccess.QueryExecution
{
    public interface IQueryExecutor
    {
        string ConnectionString { get; }
        T ExecuteQueryWithSingleResult<T>(string query) where T : class;
        IEnumerable<T> ExecuteQueryWitMultipleResults<T>(string query) where T : class;
        int ExecuteQueryWithFeedback(string query);
        int ExecuteMultipleQueriesWithFeedback(List<string> queries);
    }
}

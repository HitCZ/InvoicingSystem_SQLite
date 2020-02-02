﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Invoicing.Models;

namespace InvoicingSystem_SQLite.DataAccess.QueryExecution
{
    [Export(typeof(IQueryExecutor)), PartCreationPolicy(CreationPolicy.Shared)]
    public class QueryExecutor : IQueryExecutor
    {
        public string ConnectionString { get; }

        public QueryExecutor()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["SqliteConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Establishes SQLite Connection, executes passed query and returns single value.
        /// </summary>
        public T ExecuteQueryWithSingleResult<T>(string query) where T : IDatabaseStorableObject
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var output = connection.Query<T>(query);
                return output.Single();
            }
        }

        /// <summary>
        /// Establishes SQLite Connection, executes passed query and returns multiple values.
        /// </summary>
        public IEnumerable<T> ExecuteQueryWitMultipleResults<T>(string query) where T : IDatabaseStorableObject
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var output = connection.Query<T>(query);
                return output;
            }
        }

        /// <summary>
        /// Establishes SQLite Connection, executes passed query and returns number indicating success (0 - fail/1 - success)..
        /// </summary>
        public int ExecuteQueryWithFeedback(string query)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var response = connection.Execute(query);
                return response;
            }
        }

        /// <summary>
        /// Establishes SQLite Connection, executes passed queries and returns number indicating success (0 - fail/1 - success)..
        /// </summary>
        public int ExecuteMultipleQueriesWithFeedback(List<string> queries)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var response = -1;

                foreach (var query in queries)
                {
                    response = connection.Execute(query);

                    if (response != 1)
                        return response;
                }

                return response;
            }
        }
    }
}

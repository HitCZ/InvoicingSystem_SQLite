using System;
using InvoicingSystem.Logic.Extensions;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using InvoicingSystem_SQLite.Logic.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SqlDataProvider<T> where T : class
    {
        #region Fields

        protected readonly IQueryExecutor queryExecutor;
        protected readonly string tableName;

        #endregion Fields

        #region Constructor

        [ImportingConstructor]
        protected SqlDataProvider(IQueryExecutor queryExecutor)
        {
            this.queryExecutor = queryExecutor;

            var typeOfT = typeof(T);
            tableName = TypeToTableMappingManager.GetTableNameByType(typeOfT);

            if (tableName.IsNullOrEmpty())
                throw new TableNotFoundException(typeOfT);
        }

        #endregion Constructor

        #region Virtual Methods

        public int CreateOrUpdate(T item)
        {
            throw new NotImplementedException();
        }

        public int CreateOrUpdate(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public int Delete(T item)
        {
            throw new NotImplementedException();
        }

        public virtual T GetById(int id)
        {
            var query = $"SELECT * FROM {tableName} WHERE Id={id}";
            var result = queryExecutor.ExecuteQueryWithSingleResult<T>(query);

            return result;
        }

        public virtual IEnumerable<T> GetAll()
        {
            var query = $"SELECT * FROM {tableName}";
            var result = queryExecutor.ExecuteQueryWitMultipleResults<T>(query);

            return result;
        }

        /// <summary>
        /// Column name in database should be the same as name of the Property of given type.
        /// </summary>
        public virtual IEnumerable<T> GetBy(string columnName, string constraint)
        {
            var query = $"SELECT * FROM {tableName} WHERE {columnName} LIKE {constraint}";
            var result = queryExecutor.ExecuteQueryWitMultipleResults<T>(query);

            return result;
        }

        #endregion Virtual Methods
    }
}

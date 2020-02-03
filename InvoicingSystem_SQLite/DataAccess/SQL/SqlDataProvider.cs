using Invoicing.Models;
using InvoicingSystem.Logic.Extensions;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using InvoicingSystem_SQLite.Logic.Exceptions;
using InvoicingSystem_SQLite.Logic.Extensions;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SqlDataProvider<T> where T : IDatabaseStorableObject
    {
        #region Fields

        protected readonly IQueryExecutor queryExecutor;
        protected readonly string tableName;

        #endregion Fields

        #region Constructor

        [ImportingConstructor]
        public SqlDataProvider(IQueryExecutor queryExecutor)
        {
            this.queryExecutor = queryExecutor;

            var typeOfT = typeof(T);
            tableName = TypeToTableMappingManager.GetTableNameByType(typeOfT);

            if (tableName.IsNullOrEmpty())
                throw new TableNotFoundException(typeOfT);
        }

        #endregion Constructor

        #region Virtual Methods

        public (string query, int success) CreateOrUpdate(T item)
        {
            var itemExistsInDb = item.Id.HasValue;
            var query = itemExistsInDb ? GetUpdateQuery(item) : GetInsertQuery(item);

            var result = queryExecutor.ExecuteQueryWithFeedback(query);

            return (query, result);
        }

        public List<(string query, int success)> CreateOrUpdate(IEnumerable<T> items)
        {
            var result = items.Select(CreateOrUpdate).ToList();

            return result;
        }

        /// <summary>
        /// It doesn't make sense to return the query.
        /// </summary>
        public int Delete(T item)
        {
            var itemExistsInDb = item.Id.HasValue;

            if (!itemExistsInDb)
                return 0;

            var query = $"DELETE FROM {tableName} WHERE Id = {item.Id}";
            var result = queryExecutor.ExecuteQueryWithFeedback(query);

            return result;
        }

        public List<(int index, int success)> Delete(IEnumerable<T> items)
        {
            var list = items.ToList();
            var result =list.Select(Delete).Select((response, i) => (i, response)).ToList();
            
            return result;
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
            var query = $"SELECT * FROM {tableName} WHERE {columnName} LIKE \"%{constraint}%\"";
            var result = queryExecutor.ExecuteQueryWitMultipleResults<T>(query);

            return result;
        }

        #endregion Virtual Methods

        #region Private Methods

        private string GetInsertQuery(T item)
        {
            var propertyInformation = GetInsertInformation(item);
            var propertyNames = propertyInformation.Select(p => p.ColumnName);
            var propertyValues = propertyInformation.Select(p => p.Value);
            var joinedPropertyNames = propertyNames.JoinToStrings();
            var joinedPropertyValues = propertyValues.JoinToStrings(surroundWith: ((char)34).ToString());

            var query = $"INSERT INTO {tableName} ({joinedPropertyNames}) VALUES ({joinedPropertyValues})";

            return query;
        }

        private string GetUpdateQuery(T item)
        {
            var properties = typeof(T).GetProperties();
            var valueChanges = new List<string>();

            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var valueChange = $"{property.Name} = {property.GetValue(item)}";

                if (i < properties.Length - 1)
                    valueChange.Append(',');
                valueChanges.Add(valueChange);
            }

            var joinedChanges = string.Join(" ", valueChanges);
            var query = $"UPDATE {tableName} SET {joinedChanges} WHERE {nameof(item.Id)} = {item.Id}";

            return query;
        }

        private List<InsertInformation> GetInsertInformation(T item)
        {
            var properties = typeof(T).GetProperties();
            var result = new List<InsertInformation>();

            for (var i = 0; i < properties.Length; i++)
            {
                var currentProperty = properties[i];
                var propertyName = currentProperty.Name;
                var propertyValue = currentProperty.GetValue(item);

                if (propertyValue is null)
                    continue;

                var info = new InsertInformation(i, propertyName, propertyValue);

                result.Add(info);
            }

            return result;
        }

        #endregion Private Methods
    }
}

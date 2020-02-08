using Invoicing.Attributes;
using Invoicing.Models;
using InvoicingSystem.Logic.Extensions;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using InvoicingSystem_SQLite.Logic.Exceptions;
using InvoicingSystem_SQLite.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SqlDataProvider<T> where T : IDatabaseStorableObject
    {
        #region Fields

        protected readonly IQueryExecutor queryExecutor;
        protected readonly ITypeToTableMappingManager typeToTableMappingManager;
        protected readonly string tableName;

        #endregion Fields

        #region Constructor

        [ImportingConstructor]
        public SqlDataProvider(IQueryExecutor queryExecutor, ITypeToTableMappingManager typeToTableMappingManager)
        {
            this.queryExecutor = queryExecutor;
            this.typeToTableMappingManager = typeToTableMappingManager;

            var typeOfT = typeof(T);
            tableName = typeToTableMappingManager.GetTableNameByType(typeOfT);

            if (tableName.IsNullOrEmpty())
                throw new TableNotFoundException(typeOfT);
        }

        #endregion Constructor

        #region Public Methods

        public virtual (string query, bool success) CreateOrUpdate(T item)
        {
            var itemExistsInDb = item.Id.HasValue;
            var query = itemExistsInDb ? GetUpdateQuery(item) : GetInsertQuery(item);

            var success = queryExecutor.ExecuteQueryWithFeedback(query);

            return (query, success);
        }

        public List<(string query, bool success)> CreateOrUpdate(IEnumerable<T> items)
        {
            var result = items.Select(CreateOrUpdate).ToList();

            return result;
        }

        /// <summary>
        /// It doesn't make sense to return the query.
        /// </summary>
        public bool Delete(T item)
        {
            var itemExistsInDb = item.Id.HasValue;

            if (!itemExistsInDb)
                return false;

            var query = $"DELETE FROM {tableName} WHERE Id = {item.Id}";
            var success = queryExecutor.ExecuteQueryWithFeedback(query);

            return success;
        }

        public List<(int index, bool success)> Delete(IEnumerable<T> items)
        {
            var list = items.ToList();
            var result = list.Select(Delete).Select((response, i) => (i, response)).ToList();

            return result;
        }

        public T GetById(int id)
        {
            var query = $"SELECT * FROM {tableName} WHERE Id={id}";
            var result = queryExecutor.ExecuteQueryWithSingleResult<T>(query);

            return result;
        }

        public IEnumerable<T> GetAll()
        {
            var query = $"SELECT * FROM {tableName}";
            var result = queryExecutor.ExecuteQueryWitMultipleResults<T>(query);

            return result;
        }

        /// <summary>
        /// Column name in database should be the same as name of the Property of given type.
        /// </summary>
        public IEnumerable<T> GetBy(string columnName, string constraint)
        {
            var query = $"SELECT * FROM {tableName} WHERE {columnName} LIKE \"%{constraint}%\"";
            var result = queryExecutor.ExecuteQueryWitMultipleResults<T>(query);

            return result;
        }

        /// <summary>
        /// This is used to get a newly created db object and get its new ID.
        /// </summary>
        public IEnumerable<T> GetByEverythingExceptId(T item)
        {
            // Shouldn't contain ID, since it is null
            var propertyInformation = GetPropertiesInformation(item);
            var propertyNames = propertyInformation.Select(p => p.ColumnName).ToList();
            var propertyValues = propertyInformation.Select(p => p.Value).ToList();

            if (propertyNames.Count != propertyValues.Count)
                throw new InvalidPropertyInformationException(
                    $"The count of {nameof(propertyNames)}({propertyNames.Count}) " +
                        $"doesn't match the count of {nameof(propertyValues)}({propertyValues.Count}).");

            var constraints = GetAllConstraintsExceptId(propertyNames, propertyValues);

            var joinedConstraints = constraints.JoinToStrings(" ");
            var query = $"SELECT * FROM {tableName} WHERE {joinedConstraints}";
            var result = queryExecutor.ExecuteQueryWitMultipleResults<T>(query);

            return result;
        }

        #endregion Public Methods

        #region Virtual Methods

        protected virtual (string joinedPropertyNames, string joinedPropertyValues) GetJoinedInsertInformation(T item)
        {
            var propertyInformation = GetPropertiesInformation(item).OrderBy(p => p.ColumnName).ToList();
            var propertyNames = propertyInformation.Select(p => p.ColumnName);
            var propertyValues = propertyInformation.Select(p => p.Value);

            var joinedPropertyNames = propertyNames.JoinToStrings();
            var joinedPropertyValues = propertyValues.JoinToStrings(surroundWith: ((char)34).ToString());

            return (joinedPropertyNames, joinedPropertyValues);
        }

        /// <summary>
        /// Returns strings in format "PropertyName = value" (e.g. FirstName = "John"), without ID.
        /// </summary>
        protected virtual IEnumerable<string> GetJoinedChangesForUpdate(T item)
        {
            var properties = typeof(T).GetProperties().OrderBy(p => p.Name).ToList();
            var valueChanges = new List<string>();

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];

                if (property.HasAttribute<NotInDatabaseAttribute>())
                    continue;
                if (typeof(IDatabaseStorableObject).IsAssignableFrom(property.PropertyType))
                    continue;

                if (property.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var propertyName = property.Name;
                var propertyValue = property.GetValue(item);

                if (propertyValue is DateTime date)
                    propertyValue = date.ToString("dd.MM.yyyy");

                var valueChange = $"{propertyName} = \"{propertyValue}\"";

                if (i < properties.Count - 1)
                    valueChange.Append(',');
                valueChanges.Add(valueChange);
            }

            return valueChanges;
        }

        #endregion Virtual Methods

        #region Protected Methods

        protected List<string> GetAllConstraintsExceptId(List<string> propertyNames, List<object> propertyValues)
        {
            var constraintBuilder = new StringBuilder();
            var constraints = new List<string>();

            for (var i = 0; i < propertyNames.Count; i++)
            {
                var name = propertyNames[i];
                var value = propertyValues[i];

                constraintBuilder.Append($"{name} = \"{value}\"");
                constraints.Add(constraintBuilder.ToString());
                constraintBuilder.Clear();

                if (i < propertyNames.Count - 1)
                    constraints.Add("AND");
            }

            return constraints;
        }

        protected string GetInsertQuery(T item)
        {
            var joinedInsertInformation = GetJoinedInsertInformation(item);

            var query = $"INSERT INTO {tableName} ({joinedInsertInformation.joinedPropertyNames}) VALUES ({joinedInsertInformation.joinedPropertyValues})";

            return query;
        }

        protected string GetUpdateQuery(T item)
        {
            var valueChanges = GetJoinedChangesForUpdate(item);

            var joinedChanges = valueChanges.JoinToStrings();//string.Join(" ", valueChanges);
            var query = $"UPDATE {tableName} SET {joinedChanges} WHERE {nameof(item.Id)} = {item.Id}";

            return query;
        }

        /// <summary>
        /// Returns collection of property indexes, names and values, of all properties, except <see cref="IDatabaseStorableObject"/>.
        /// When property value is null, it is not included within the list.
        /// </summary>
        protected List<PropertyInformation> GetPropertiesInformation(T item)
        {
            var properties = typeof(T).GetProperties();
            var result = new List<PropertyInformation>();

            foreach (var currentProperty in properties)
            {
                var propertyType = currentProperty.PropertyType;

                if ((typeof(IDatabaseStorableObject).IsAssignableFrom(propertyType)))
                    continue;

                var propertyName = currentProperty.Name;
                var propertyValue = currentProperty.GetValue(item);

                if (propertyValue is null)
                    continue;

                var info = new PropertyInformation(propertyName, propertyValue);

                result.Add(info);
            }

            return result;
        }

        #endregion Protected Methods
    }
}

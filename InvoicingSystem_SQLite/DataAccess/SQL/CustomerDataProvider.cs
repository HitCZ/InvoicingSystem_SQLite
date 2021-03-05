using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [Export(nameof(CustomerDataProvider), typeof(ISqlDataProvider<Customer>))]
    public class CustomerDataProvider : SqlDataProvider<Customer>
    {
        #region Fields

        private const string VALUE_SEPARATOR = ", ";
        private readonly ISqlDataProvider<Address> addressProvider;

        #endregion Fields

        #region Constructor

        [ImportingConstructor]
        public CustomerDataProvider
        (
            IQueryExecutor queryExecutor,
            ITypeToTableMappingManager typeToTableMappingManager,
            ISqlDataProvider<Address> addressProvider
        ) : base(queryExecutor, typeToTableMappingManager)
        {
            this.addressProvider = addressProvider;
        }

        #endregion Constructor

        #region Overriden Methods

        public override (string query, bool success) CreateOrUpdate(Customer item)
        {
            EnsureDependenciesAreSet(item);

            return base.CreateOrUpdate(item);
        }

        protected override IEnumerable<string> GetJoinedChangesForUpdate(Customer item)
        {
            var changes = base.GetJoinedChangesForUpdate(item).ToList();

            changes.Add($"Id{nameof(Address)} = \"{item.Address.Id}\"");

            return changes;
        }

        protected override (string joinedPropertyNames, string joinedPropertyValues) GetJoinedInsertInformation(Customer item)
        {
            var joinedInformation = base.GetJoinedInsertInformation(item);
            var namesBuilder = new StringBuilder(joinedInformation.joinedPropertyNames);
            var valuesBuilder = new StringBuilder(joinedInformation.joinedPropertyValues);

            if (namesBuilder.Length > 0)
                namesBuilder.Append(VALUE_SEPARATOR);

            namesBuilder.Append($"Id{nameof(item.Address)}");

            if (valuesBuilder.Length > 0)
            {
                valuesBuilder.Append(VALUE_SEPARATOR);
                valuesBuilder.Append($"\"{item.Address.Id}\"");
            }

            return (namesBuilder.ToString(), valuesBuilder.ToString());
        }

        #endregion Overriden Methods

        #region Private Methods

        private void EnsureDependenciesAreSet(Customer item)
        {
            var address = item.Address;

            if (address is null)
                throw new ArgumentNullException(nameof(address));

            if (address.Id.HasValue)
                return;

            var success = addressProvider.CreateOrUpdate(address).success;

            if (success)
                item.Address = addressProvider.GetByEverythingExceptId(address).First();
        }

        #endregion Private Methods
    }
}

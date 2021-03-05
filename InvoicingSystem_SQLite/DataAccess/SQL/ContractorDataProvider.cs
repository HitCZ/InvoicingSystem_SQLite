using System;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [Export(nameof(ContractorDataProvider), typeof(ISqlDataProvider<Contractor>))]
    public class ContractorDataProvider : SqlDataProvider<Contractor>
    {
        #region Fields

        private const string VALUE_SEPARATOR = ", ";
        private readonly ISqlDataProvider<Address> addressProvider;

        #endregion Fields

        #region Constructor

        [ImportingConstructor]
        public ContractorDataProvider
        (
            IQueryExecutor queryExecutor, 
            ITypeToTableMappingManager typeToTableMappingManager,
            ISqlDataProvider<Address> addressProvider
        )
            : base(queryExecutor, typeToTableMappingManager)
        {
            this.addressProvider = addressProvider;
        }

        #endregion Constructor

        #region Overriden Methods

        public override (string query, bool success) CreateOrUpdate(Contractor item)
        {
            EnsureDependenciesAreSet(item);

            return base.CreateOrUpdate(item);
        }

        protected override IEnumerable<string> GetJoinedChangesForUpdate(Contractor item)
        {
            var changes = base.GetJoinedChangesForUpdate(item).ToList();

            changes.Add($"Id{nameof(Address)} = \"{item.Address.Id}\"");

            return changes;
        }

        protected override (string joinedPropertyNames, string joinedPropertyValues) GetJoinedInsertInformation(Contractor item)
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

        private void EnsureDependenciesAreSet(Contractor item)
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

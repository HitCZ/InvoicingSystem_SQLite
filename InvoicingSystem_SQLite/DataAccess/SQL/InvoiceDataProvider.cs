using System;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    public class InvoiceDataProvider : SqlDataProvider<Invoice>
    {
        private const string VALUE_SEPARATOR = ", ";
        private readonly SqlDataProvider<BankInformation> bankInformationProvider;
        private readonly SqlDataProvider<Customer> customerProvider;
        private readonly SqlDataProvider<Contractor> contractorProvider;

        public InvoiceDataProvider(IQueryExecutor queryExecutor, ITypeToTableMappingManager typeToTableMappingManager) 
            : base(queryExecutor, typeToTableMappingManager)
        {
            bankInformationProvider = SqlProviderFactory<BankInformation>.GetProvider();
            customerProvider = SqlProviderFactory<Customer>.GetProvider();
            contractorProvider = SqlProviderFactory<Contractor>.GetProvider();
        }

        #region Overriden Methods

        public override (string query, bool success) CreateOrUpdate(Invoice item)
        {
            EnsureDependenciesAreSet(item);

            var query = !item.Id.HasValue ? GetInsertQuery(item) : GetUpdateQuery(item);
            var result = queryExecutor.ExecuteQueryWithFeedback(query);
            var success = result == 0;

            return (query, success);
        }

        protected override IEnumerable<string> GetJoinedChangesForUpdate(Invoice item)
        {
            var changes = base.GetJoinedChangesForUpdate(item).ToList();

            if (changes.Any())
                changes.Add(VALUE_SEPARATOR);

            changes.Add($"{VALUE_SEPARATOR}Id{nameof(BankInformation)} = {item.BankInformation.Id}");
            changes.Add($"{VALUE_SEPARATOR}Id{nameof(Customer)} = {item.Customer.Id}");
            changes.Add($"{VALUE_SEPARATOR}Id{nameof(Contractor)} = {item.Contractor.Id}");

            return changes;
        }

        protected override (string joinedPropertyNames, string joinedPropertyValues) GetJoinedInsertInformation(Invoice item)
        {
            var joinedInformation = base.GetJoinedInsertInformation(item);
            var namesBuilder = new StringBuilder(joinedInformation.joinedPropertyNames);
            var valuesBuilder = new StringBuilder(joinedInformation.joinedPropertyValues);

            if (namesBuilder.Length > 0)
                namesBuilder.Append(VALUE_SEPARATOR);

            namesBuilder.Append($"Id{nameof(Invoice.BankInformation)}");
            namesBuilder.Append($"{VALUE_SEPARATOR}Id{nameof(Invoice.Customer)}");
            namesBuilder.Append($"{VALUE_SEPARATOR}Id{nameof(Invoice.Contractor)}");

            if (valuesBuilder.Length > 0)
                valuesBuilder.Append(VALUE_SEPARATOR);

            valuesBuilder.Append($"\"{item.BankInformation.Id}\"");
            valuesBuilder.Append($"{VALUE_SEPARATOR}\"{item.Customer.Id}\"");
            valuesBuilder.Append($"{VALUE_SEPARATOR}\"{item.Contractor.Id}\"");

            return (namesBuilder.ToString(), valuesBuilder.ToString());
        }

        #endregion Overriden Methods

        #region Private Methods

        private void EnsureDependenciesAreSet(Invoice item)
        {
            var bankInfo = item.BankInformation;
            var customer = item.Customer;
            var contractor = item.Contractor;

            if (bankInfo is null)
                throw new ArgumentNullException(nameof(bankInfo));
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));
            if (contractor is null)
                throw new ArgumentNullException(nameof(contractor));

            if (!bankInfo.Id.HasValue)
            {
                var success = bankInformationProvider.CreateOrUpdate(bankInfo).success;
                if (success)
                    item.BankInformation = bankInformationProvider.GetByEverythingExceptId(bankInfo).First();
            }

            if (!customer.Id.HasValue)
            {
                var success = customerProvider.CreateOrUpdate(customer).success;
                if (success)
                    item.Customer = customerProvider.GetByEverythingExceptId(customer).First();
            }

            if (!contractor.Id.HasValue)
            {
                var success = contractorProvider.CreateOrUpdate(contractor).success;
                if (success)
                    item.Contractor = contractorProvider.GetByEverythingExceptId(contractor).First();
            }
        }

        #endregion Private Methods
    }
}

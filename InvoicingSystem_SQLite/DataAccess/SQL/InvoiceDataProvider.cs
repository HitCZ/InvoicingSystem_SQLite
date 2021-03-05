using System;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [Export(nameof(InvoiceDataProvider), typeof(ISqlDataProvider<Invoice>))]
    public class InvoiceDataProvider : SqlDataProvider<Invoice>
    {
        #region Fields

        private const string VALUE_SEPARATOR = ", ";
        private readonly ISqlDataProvider<BankInformation> bankInformationProvider;
        private readonly ISqlDataProvider<Customer> customerProvider;
        private readonly ISqlDataProvider<Contractor> contractorProvider;

        #endregion Fields

        #region Constructor

        [ImportingConstructor]
        public InvoiceDataProvider
        (
            IQueryExecutor queryExecutor,
            ITypeToTableMappingManager typeToTableMappingManager,
            [Import(nameof(CustomerDataProvider), typeof(ISqlDataProvider<Customer>))] ISqlDataProvider<Customer> customerProvider,
            [Import(nameof(ContractorDataProvider), typeof(ISqlDataProvider<Contractor>))] ISqlDataProvider<Contractor> contractorProvider,
            ISqlDataProvider<BankInformation> bankInformationProvider
        )
            : base(queryExecutor, typeToTableMappingManager)
        {
            this.bankInformationProvider = bankInformationProvider;
            this.customerProvider = customerProvider;
            this.contractorProvider = contractorProvider;
        }

        #endregion Constructor

        #region Overriden Methods

        public override (string query, bool success) CreateOrUpdate(Invoice item)
        {
            EnsureDependenciesAreSet(item);

            return base.CreateOrUpdate(item);
        }

        protected override IEnumerable<string> GetJoinedChangesForUpdate(Invoice item)
        {
            var changes = base.GetJoinedChangesForUpdate(item).ToList();

            changes.Add($"Id{nameof(BankInformation)} = \"{item.BankInformation.Id}\"");
            changes.Add($"Id{nameof(Contractor)} = \"{item.Contractor.Id}\"");
            changes.Add($"Id{nameof(Customer)} = \"{item.Customer.Id}\"");

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
            namesBuilder.Append($"{VALUE_SEPARATOR}Id{nameof(Invoice.Contractor)}");
            namesBuilder.Append($"{VALUE_SEPARATOR}Id{nameof(Invoice.Customer)}");

            if (valuesBuilder.Length > 0)
                valuesBuilder.Append(VALUE_SEPARATOR);

            valuesBuilder.Append($"\"{item.BankInformation.Id}\"");
            valuesBuilder.Append($"{VALUE_SEPARATOR}\"{item.Contractor.Id}\"");
            valuesBuilder.Append($"{VALUE_SEPARATOR}\"{item.Customer.Id}\"");

            return (namesBuilder.ToString(), valuesBuilder.ToString());
        }

        #endregion Overriden Methods

        #region Private Methods

        private void EnsureDependenciesAreSet(Invoice item)
        {
            var bankInfo = item.BankInformation;
            var customer = item.Customer;
            var contractor = item.Contractor;
            var customerAddress = item.Customer?.Address;
            var contractorAddress = item.Contractor?.Address;

            if (bankInfo is null)
                throw new ArgumentNullException(nameof(bankInfo));
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));
            if (contractor is null)
                throw new ArgumentNullException(nameof(contractor));
            if (customerAddress is null)
                throw new ArgumentNullException(nameof(customerAddress));
            if (contractorAddress is null)
                throw new ArgumentNullException(nameof(contractorAddress));

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

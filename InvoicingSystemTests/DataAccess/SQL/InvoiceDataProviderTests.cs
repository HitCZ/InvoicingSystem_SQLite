using FluentAssertions;
using Invoicing.Enumerations;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystem_SQLite.Logic.Comparers;
using InvoicingSystemTests.Mocks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace InvoicingSystemTests.DataAccess.SQL
{
    [TestClass]
    public class InvoiceDataProviderTests
    {
        #region Fields

        private const string JOB_DESCRIPTION = "blablabla";
        private Invoice invoice;
        private TestInvoiceDataProvider provider;
        private EnumerableStringEqualityComparer comparer;
        private Mock<ISqlDataProvider<Customer>> customerProviderMock;
        private Mock<ISqlDataProvider<Contractor>> contractorProviderMock;
        private Mock<ISqlDataProvider<BankInformation>> bankInformationProviderMock;

        #endregion Fields

        #region Test Initialize

        [TestInitialize]
        public void TestInitialize()
        {
            var queryExecutorMock = new Mock<IQueryExecutor>();
            var mappingManagerMock = MockProvider.GetMockedMappingManager();

            customerProviderMock = new Mock<ISqlDataProvider<Customer>>();
            contractorProviderMock = new Mock<ISqlDataProvider<Contractor>>();
            bankInformationProviderMock = new Mock<ISqlDataProvider<BankInformation>>();

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(m => m.GetInstance<ISqlDataProvider<BankInformation>>()).Returns(bankInformationProviderMock.Object);
            serviceLocatorMock.Setup(m => m.GetInstance<ISqlDataProvider<Customer>>(nameof(CustomerDataProvider))).Returns(customerProviderMock.Object);
            serviceLocatorMock.Setup(m => m.GetInstance<ISqlDataProvider<Contractor>>(nameof(ContractorDataProvider))).Returns(contractorProviderMock.Object);

            ServiceLocator.SetLocatorProvider(() => serviceLocatorMock.Object);

            invoice = new Invoice(1)
            {
                DateOfIssue = new DateTime(2020, 2, 8),
                DueDate = new DateTime(2020, 2, 10),
                InvoiceNumber = 20200001,
                JobDescription = JOB_DESCRIPTION,
                PaymentMethod = PaymentMethod.BankTransfer,
                Price = 30000,
                BankInformation = new BankInformation(2),
                Contractor = new Contractor(3),
                Customer = new Customer(4)
            };

            provider = new TestInvoiceDataProvider
            (
                queryExecutorMock.Object,
                mappingManagerMock.Object,
                customerProviderMock.Object,
                contractorProviderMock.Object,
                bankInformationProviderMock.Object
            );
            comparer = new EnumerableStringEqualityComparer();
        }

        #endregion Test Initialize

        #region Test Methods

        [TestMethod]
        public void InvoiceDataProvider_CreateOrUpdateTest()
        {
            var localInvoice = new Invoice();
            var mockedBankInfo = new BankInformation(1);
            var mockedCustomer = new Customer(2);
            var mockedContractor = new Contractor(3);

            provider.Invoking(p => p.CreateOrUpdate(localInvoice)).Should().Throw<ArgumentNullException>();
            localInvoice.BankInformation = new BankInformation();

            provider.Invoking(p => p.CreateOrUpdate(localInvoice)).Should().Throw<ArgumentNullException>();
            localInvoice.Customer = new Customer();

            provider.Invoking(p => p.CreateOrUpdate(localInvoice)).Should().Throw<ArgumentNullException>();
            localInvoice.Contractor = new Contractor();

            provider.Invoking(p => p.CreateOrUpdate(localInvoice)).Should().Throw<ArgumentNullException>();
            localInvoice.Customer.Address = new Address();

            provider.Invoking(p => p.CreateOrUpdate(localInvoice)).Should().Throw<ArgumentNullException>();
            localInvoice.Contractor.Address = new Address();

            bankInformationProviderMock.Setup(m => m.CreateOrUpdate(It.IsAny<BankInformation>()))
                .Returns((string.Empty, true));
            bankInformationProviderMock.Setup(m => m.GetByEverythingExceptId(It.IsAny<BankInformation>()))
                .Returns(new List<BankInformation> { mockedBankInfo });
            customerProviderMock.Setup(m => m.CreateOrUpdate(It.IsAny<Customer>())).Returns((string.Empty, true));
            customerProviderMock.Setup(m => m.GetByEverythingExceptId(It.IsAny<Customer>())).Returns(new List<Customer> { mockedCustomer });
            contractorProviderMock.Setup(m => m.CreateOrUpdate(It.IsAny<Contractor>())).Returns((string.Empty, true));
            contractorProviderMock.Setup(m => m.GetByEverythingExceptId(It.IsAny<Contractor>()))
                .Returns(new List<Contractor> { mockedContractor });

            provider.CreateOrUpdate(localInvoice);

            localInvoice.BankInformation.Should().BeEquivalentTo(mockedBankInfo);
            localInvoice.Customer.Should().BeEquivalentTo(mockedCustomer);
            localInvoice.Contractor.Should().BeEquivalentTo(mockedContractor);
        }

        [TestMethod]
        public void InvoiceDataProvider_GetJoinedInsertInformationTest()
        {
            var expectedNames = "Currency, DateOfIssue, DueDate, InvoiceNumber, JobDescription, PaymentMethod, Price, " +
                                "IdBankInformation, IdContractor, IdCustomer";
            var expectedValues = $"\"CZK\", \"08.02.2020\", \"10.02.2020\", \"20200001\", \"{JOB_DESCRIPTION}\", " +
                                 $"\"{PaymentMethod.BankTransfer}\", \"30000\", \"2\", \"3\", \"4\"";

            if (invoice.Id.HasValue)
            {
                var indexInNames = expectedNames.IndexOf("InvoiceNumber", StringComparison.Ordinal);
                expectedNames = expectedNames.Insert(indexInNames, "Id, ");
                var indexInValues = expectedValues.IndexOf(@"20200001", StringComparison.Ordinal) - 1;
                expectedValues = expectedValues.Insert(indexInValues, $"\"{invoice.Id}\", ");
            }

            var actual = provider.GetJoinedInsertInformation(invoice);
            var actualNames = actual.joinedPropertyNames;
            var actualValues = actual.joinedPropertyValues;

            actualNames.Should().Be(expectedNames);
            actualValues.Should().Be(expectedValues);
        }

        [TestMethod]
        public void InvoiceDataProvider_GetJoinedChangesForUpdateTest()
        {
            var expected = new List<string>
            {
                "Currency = \"CZK\"",
                "DateOfIssue = \"08.02.2020\"",
                "DueDate = \"10.02.2020\"",
                "InvoiceNumber = \"20200001\"",
                $"JobDescription = \"{JOB_DESCRIPTION}\"",
                $"PaymentMethod = \"{PaymentMethod.BankTransfer}\"",
                "Price = \"30000\"",
                "IdBankInformation = \"2\"",
                "IdContractor = \"3\"",
                "IdCustomer = \"4\""
            };

            var actual = provider.GetJoinedChangesForUpdate(invoice);
            var areEqual = comparer.Equals(expected, actual);
            Assert.IsTrue(areEqual);
        }

        #endregion Test Methods

        private class TestInvoiceDataProvider : InvoiceDataProvider
        {
            public TestInvoiceDataProvider
            (
                IQueryExecutor queryExecutor,
                ITypeToTableMappingManager typeToTableMappingManager,
                ISqlDataProvider<Customer> customerProvider,
                ISqlDataProvider<Contractor> contractorProvider,
                ISqlDataProvider<BankInformation> bankInformationProvider
            )
                : base(queryExecutor, typeToTableMappingManager, customerProvider, contractorProvider, bankInformationProvider)
            {
            }

            public new IEnumerable<string> GetJoinedChangesForUpdate(Invoice item) => base.GetJoinedChangesForUpdate(item);

            public new(string joinedPropertyNames, string joinedPropertyValues) GetJoinedInsertInformation(
                Invoice item) => base.GetJoinedInsertInformation(item);
        }
    }
}

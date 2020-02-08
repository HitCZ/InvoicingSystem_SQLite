using Invoicing.Enumerations;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystem_SQLite.Logic.Comparers;
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
        private const string JOB_DESCRIPTION = "blablabla";

        private Mock<IQueryExecutor> queryExecutorMock;
        private Mock<IServiceLocator> serviceLocatorMock;

        private IQueryExecutor queryExecutor;
        private ITypeToTableMappingManager typeToTableMappingManager;
        private IServiceLocator serviceLocator;

        private Invoice invoice;
        private TestInvoiceDataProvider provider;
        private SqlDataProvider<BankInformation> bankInformationProvider;
        private SqlDataProvider<Customer> customerProvider;
        private SqlDataProvider<Contractor> contractorProvider;
        private EnumerableStringComparer comparer;

        [TestInitialize]
        public void TestInitialize()
        {
            queryExecutorMock = new Mock<IQueryExecutor>();
            queryExecutor = queryExecutorMock.Object;

            typeToTableMappingManager = new TypeToTableMappingManager();

            bankInformationProvider = new SqlDataProvider<BankInformation>(queryExecutor, typeToTableMappingManager);
            customerProvider = new SqlDataProvider<Customer>(queryExecutor, typeToTableMappingManager);
            contractorProvider = new SqlDataProvider<Contractor>(queryExecutor, typeToTableMappingManager);

            serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(m => m.GetInstance<SqlDataProvider<BankInformation>>()).Returns(bankInformationProvider);
            serviceLocatorMock.Setup(m => m.GetInstance<SqlDataProvider<Customer>>()).Returns(customerProvider);
            serviceLocatorMock.Setup(m => m.GetInstance<SqlDataProvider<Contractor>>()).Returns(contractorProvider);
            serviceLocator = serviceLocatorMock.Object;
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

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

            provider = new TestInvoiceDataProvider(queryExecutor, typeToTableMappingManager);
            comparer = new EnumerableStringComparer();
        }

        #region Test Method

        [TestMethod]
        public void GetJoinedInsertInformationTest()
        {
            var expectedNames = "DateOfIssue, DueDate, InvoiceNumber, JobDescription, PaymentMethod, Price, " +
                                "IdBankInformation, IdContractor, IdCustomer";
            var expectedValues = $"\"08.02.2020\", \"10.02.2020\", \"20200001\", \"{JOB_DESCRIPTION}\", " +
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

            Assert.AreEqual(expectedNames, actualNames);
            Assert.AreEqual(expectedValues, actualValues);
        }

        [TestMethod]
        public void GetJoinedChangesForUpdateTest()
        {
            var expected = new List<string>
            {
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
            var areEqual = comparer.Compare(expected, actual) == 0;
            Assert.IsTrue(areEqual);
        }

        #endregion Test Method

        private class TestInvoiceDataProvider : InvoiceDataProvider
        {
            public TestInvoiceDataProvider(IQueryExecutor queryExecutor, ITypeToTableMappingManager typeToTableMappingManager) : base(queryExecutor, typeToTableMappingManager)
            {
            }

            public new IEnumerable<string> GetJoinedChangesForUpdate(Invoice item) => base.GetJoinedChangesForUpdate(item);

            public new (string joinedPropertyNames, string joinedPropertyValues) GetJoinedInsertInformation(
                Invoice item) => base.GetJoinedInsertInformation(item);
        }
    }
}

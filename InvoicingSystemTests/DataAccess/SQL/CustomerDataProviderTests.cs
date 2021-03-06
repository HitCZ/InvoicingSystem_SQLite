using FluentAssertions;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystemTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvoicingSystemTests.DataAccess.SQL
{
    [TestClass]
    public class CustomerDataProviderTests
    {
        #region Fields

        private Mock<ISqlDataProvider<Address>> addressProviderMock;
        private TestDataProvider provider;
        private Customer customer;

        #endregion Fields

        #region Test Initialize

        [TestInitialize]
        public void TestInitialize()
        {
            var queryExecutorMock = new Mock<IQueryExecutor>();
            var mappingManagerMock = MockProvider.GetMockedMappingManager();
            addressProviderMock = new Mock<ISqlDataProvider<Address>>();

            customer = new Customer { Address = new Address(1) };
            provider = new TestDataProvider
            (
                queryExecutorMock.Object,
                mappingManagerMock.Object,
                addressProviderMock.Object
            );
        }

        #endregion Test Initialize

        #region Test Methods

        [TestMethod]
        public void CustomerDataProvider_CreateOrUpdateTest()
        {
            var localCustomer = new Customer();
            var mockedAddress = new Address(1);

            provider.Invoking(p => p.CreateOrUpdate(localCustomer)).Should().Throw<ArgumentNullException>();
            localCustomer.Address = mockedAddress;

            addressProviderMock.Setup(m => m.CreateOrUpdate(It.IsAny<Address>())).Returns((string.Empty, true));
            addressProviderMock.Setup(m => m.GetByEverythingExceptId(It.IsAny<Address>()))
                .Returns(new List<Address> { mockedAddress });

            localCustomer.Address.Should().BeEquivalentTo(mockedAddress);
        }

        [TestMethod]
        public void ContractorDataProvider_GetJoinedChangesForUpdateTest()
        {
            var actual = provider.GetJoinedChangesForUpdate(customer);

            actual.Last().Should().Be("IdAddress = \"1\"");
        }

        [TestMethod]
        public void ContractorDataProvider_GetJoinedInsertInformation()
        {
            var actual = provider.GetJoinedInsertInformation(customer);

            actual.joinedPropertyNames.EndsWith(", IdAddress").Should().BeTrue();
            actual.joinedPropertyValues.EndsWith(", \"1\"").Should().BeTrue();
        }

        #endregion Test Methods

        #region Helper Class

        private class TestDataProvider : CustomerDataProvider
        {
            public TestDataProvider(IQueryExecutor queryExecutor, ITypeToTableMappingManager typeToTableMappingManager, ISqlDataProvider<Address> addressProvider) : base(queryExecutor, typeToTableMappingManager, addressProvider)
            {
            }

            public new IEnumerable<string> GetJoinedChangesForUpdate(Customer item) => base.GetJoinedChangesForUpdate(item);

            public new(string joinedPropertyNames, string joinedPropertyValues)
                GetJoinedInsertInformation(Customer item) => base.GetJoinedInsertInformation(item);
        }

        #endregion Helper Class
    }
}

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
    public class ContractorDataProviderTests
    {
        #region Fields

        private TestDataProvider provider;
        private Mock<ISqlDataProvider<Address>> addressProviderMock;
        private Contractor contractor;

        #endregion Fields

        #region Test Initialize

        [TestInitialize]
        public void TestInitialize()
        {
            var queryExecutorMock = new Mock<IQueryExecutor>();
            var mappingManagerMock = MockProvider.GetMockedMappingManager();
            addressProviderMock = new Mock<ISqlDataProvider<Address>>();

            contractor = new Contractor
            {
                Address = new Address(1)
            };

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
        public void ContractorDataProvider_CreateOrUpdateTest()
        {
            var localContractor = new Contractor();
            var mockedAddress = new Address(1);

            provider.Invoking(p => p.CreateOrUpdate(localContractor)).Should().Throw<ArgumentNullException>();
            localContractor.Address = mockedAddress;

            addressProviderMock.Setup(m => m.CreateOrUpdate(It.IsAny<Address>())).Returns((string.Empty, true));
            addressProviderMock.Setup(m => m.GetByEverythingExceptId(It.IsAny<Address>()))
                .Returns(new List<Address> {mockedAddress});

            localContractor.Address.Should().BeEquivalentTo(mockedAddress);
        }

        [TestMethod]
        public void ContractorDataProvider_GetJoinedChangesForUpdateTest()
        {
            var actual = provider.GetJoinedChangesForUpdate(contractor);

            actual.Last().Should().Be("IdAddress = \"1\"");
        }

        [TestMethod]
        public void ContractorDataProvider_GetJoinedInsertInformation()
        {
            var actual = provider.GetJoinedInsertInformation(contractor);

            actual.joinedPropertyNames.EndsWith(", IdAddress").Should().BeTrue();
            actual.joinedPropertyValues.EndsWith(", \"1\"").Should().BeTrue();
        }

        #endregion Test Methods

        private class TestDataProvider : ContractorDataProvider
        {
            public TestDataProvider(IQueryExecutor queryExecutor, ITypeToTableMappingManager typeToTableMappingManager, ISqlDataProvider<Address> addressProvider) : base(queryExecutor, typeToTableMappingManager, addressProvider)
            {
            }

            public new IEnumerable<string> GetJoinedChangesForUpdate(Contractor item) =>
                base.GetJoinedChangesForUpdate(item);

            public new (string joinedPropertyNames, string joinedPropertyValues)
                GetJoinedInsertInformation(Contractor item) => base.GetJoinedInsertInformation(item);
        }
    }
}

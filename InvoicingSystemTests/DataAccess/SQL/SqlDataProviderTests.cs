using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.QueryExecution;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystem_SQLite.Logic.Comparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using PropertyInformation = InvoicingSystem_SQLite.DataAccess.SQL.PropertyInformation;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace InvoicingSystemTests.DataAccess.SQL
{
    [TestClass]
    public class SqlDataProviderTests
    {
        private const string TABLE_NAME = "table";

        private Mock<IQueryExecutor> queryExecutorMock;
        private Mock<ITypeToTableMappingManager> typeToTableMappingManagerMock;

        private IQueryExecutor queryExecutor;
        private ITypeToTableMappingManager typeToTableMappingManager;

        private TestDataProvider provider;
        private EnumerableStringComparer enumerableStringComparer;
        private EnumerablePropertyInformationOrderIndependentComparer enumerablePropertyInformationComparer;
        private TestModel testModel;

        [TestInitialize]
        public void TestInitialize()
        {
            queryExecutorMock = new Mock<IQueryExecutor>();
            queryExecutor = queryExecutorMock.Object;

            typeToTableMappingManagerMock = new Mock<ITypeToTableMappingManager>();
            typeToTableMappingManagerMock.Setup(m => m.GetTableNameByType(It.IsAny<Type>())).Returns(TABLE_NAME);
            typeToTableMappingManager = typeToTableMappingManagerMock.Object;

            provider = new TestDataProvider(queryExecutor, typeToTableMappingManager);
            enumerableStringComparer = new EnumerableStringComparer();
            enumerablePropertyInformationComparer = new EnumerablePropertyInformationOrderIndependentComparer();
            testModel = new TestModel(1)
            {
                FirstName = "John",
                LastName = "Doe",
                ZipCode = 27351
            };
        }

        #region Test Methods

        [TestMethod]
        public void GetJoinedInsertInformationTest()
        {
            var expectedNames = "FirstName, LastName, ZipCode";
            var expectedValues = "\"John\", \"Doe\", \"27351\"";

            var actual = provider.GetJoinedInsertInformation(testModel);
            var actualNames = actual.joinedPropertyNames;
            var actualValues = actual.joinedPropertyValues;

            Assert.AreEqual(expectedNames, actualNames);
            Assert.AreEqual(expectedValues, actualValues);
        }

        [TestMethod]
        public void GetJoinedChangesForUpdateTest()
        {
            var expected = new List<string> { "FirstName = \"John\"", "LastName = \"Doe\"", "ZipCode = \"27351\"" };

            var actual = provider.GetJoinedChangesForUpdate(testModel);
            var areEqual = enumerableStringComparer.Compare(expected, actual) == 0;

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void GetInsertQueryTest()
        {
            var expected =
                $"INSERT INTO {TABLE_NAME} (FirstName, LastName, ZipCode) VALUES (\"John\", \"Doe\", \"27351\")";
            var actual = provider.GetInsertQuery(testModel);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUpdateQueryTest()
        {
            var expected = $"UPDATE {TABLE_NAME} SET FirstName = \"John\", LastName = \"Doe\", ZipCode = \"27351\" WHERE Id = 1";
            var actual = provider.GetUpdateQuery(testModel);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPropertiesInformationTest()
        {
            var expected = new List<PropertyInformation>
            {
                new PropertyInformation("FirstName", "John"),
                new PropertyInformation("LastName", "Doe"),
                new PropertyInformation("ZipCode", (uint)27351),
                new PropertyInformation("Id", 1)
            };

            var actual = provider.GetPropertiesInformation(testModel);
            var areEqual = enumerablePropertyInformationComparer.Compare(expected, actual) == 0;

            Assert.IsTrue(areEqual);
        }

        #endregion Test Methods

        #region TestDataProvider

        private class TestDataProvider : SqlDataProvider<TestModel>
        {
            public TestDataProvider(IQueryExecutor queryExecutor, ITypeToTableMappingManager typeToTableMappingManager) 
                : base(queryExecutor, typeToTableMappingManager)
            {
            }

            public new(string joinedPropertyNames, string joinedPropertyValues) GetJoinedInsertInformation(
                TestModel item)
            {
                return base.GetJoinedInsertInformation(item);
            }

            public new IEnumerable<string> GetJoinedChangesForUpdate(TestModel item) => base.GetJoinedChangesForUpdate(item);

            public new string GetInsertQuery(TestModel item) => base.GetInsertQuery(item);

            public new string GetUpdateQuery(TestModel item) => base.GetUpdateQuery(item);

            public new List<PropertyInformation> GetPropertiesInformation(TestModel item) => base.GetPropertiesInformation(item);
        }

        #endregion TestDataProvider

        #region TestModel

        private class TestModel : ModelBase
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public uint ZipCode { get; set; }

            public TestModel(int? id) : base(id)
            {
            }
        }

        #endregion  TestModel
    }
}

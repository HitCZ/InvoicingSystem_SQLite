using InvoicingSystem_SQLite.Logic.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace InvoicingSystemTests.Validators
{
    [TestClass]
    public class DbValidatorTests
    {
        [TestMethod]
        public void ColumnExistsTest()
        {
            var testConfigs = new List<(string columnName, bool expectedResult)>
            {
                (nameof(TestClass.NotInDb), false),
                (nameof(TestClass.InDb), true),
                (string.Empty, false),
                (null, false)
            };

            foreach (var config in testConfigs)
            {
                var columnName = config.columnName;
                var expected = config.expectedResult;

                var actual = DbValidator.ColumnExists<TestClass>(columnName);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}

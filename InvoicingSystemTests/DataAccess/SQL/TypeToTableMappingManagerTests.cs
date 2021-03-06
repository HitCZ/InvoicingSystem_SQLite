using Castle.Core.Internal;
using FluentAssertions;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.SQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace InvoicingSystemTests.DataAccess.SQL
{
    [TestClass]
    public class TypeToTableMappingManagerTests
    {
        #region Test Methods

        [TestMethod]
        public void TypeToTableMappingManager_GetTableNameByTypeTest()
        {
            var allTypes = typeof(ModelBase).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ModelBase)) && !t.IsAbstract);
            var manager = new TypeToTableMappingManager();

            allTypes.All(t => !manager.GetTableNameByType(t).IsNullOrEmpty()).Should().BeTrue();
        }

        #endregion Test Methods
    }
}

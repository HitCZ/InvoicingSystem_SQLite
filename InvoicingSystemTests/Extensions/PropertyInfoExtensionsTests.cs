using Invoicing.Attributes;
using InvoicingSystem_SQLite.Logic.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace InvoicingSystemTests.Extensions
{
    [TestClass]
    public class PropertyInfoExtensionsTests
    {
        [TestMethod]
        public void HasNotInDatabaseAttributeTest()
        {
            var properties = typeof(TestClass).GetProperties();

            var notInDbActual = properties.Single(p => p.Name == nameof(TestClass.NotInDb))
                .HasAttribute<NotInDatabaseAttribute>();
            var inDbActual = properties.Single(p => p.Name == nameof(TestClass.InDb))
                .HasAttribute<NotInDatabaseAttribute>();

            Assert.IsTrue(notInDbActual);
            Assert.IsFalse(inDbActual);
        }
    }
}

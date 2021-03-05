using Invoicing.Attributes;
using Invoicing.Models;

namespace InvoicingSystemTests
{
    internal class TestClass : IDatabaseStorableObject
    {
        [NotInDatabase]
        public string NotInDb { get; set; }
        public string InDb { get; set; }
        public long? Id { get; }
    }
}

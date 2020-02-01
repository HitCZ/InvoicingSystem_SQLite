using Invoicing.Attributes;

namespace InvoicingSystemTests
{
    internal class TestClass
    {
        [NotInDatabase]
        public string NotInDb { get; set; }
        public string InDb { get; set; }
    }
}

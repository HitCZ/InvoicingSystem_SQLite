using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystem_SQLite.ServiceLocation;

namespace InvoicingSystem_SQLite.Views
{
    /// <summary>
    /// Interaction logic for InvoiceView.xaml
    /// </summary>
    public partial class InvoiceView
    {
        public InvoiceView()
        {
            InitializeComponent();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            var provider = SqlProviderFactory<Address>.GetProvider();
            var address = new Address(null)
            {
                BuildingNumber = "71", 
                City = "Kyšice", 
                Country = "Česká republika",
                Street = "Karlovarská", 
                ZipCode = 27351
            };
            provider.CreateOrUpdate(address);
        }
    }
}

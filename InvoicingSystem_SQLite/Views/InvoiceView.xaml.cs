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
        }
    }
}

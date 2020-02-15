using System.Windows;
using System.Windows.Controls;

namespace InvoicingSystem_SQLite.Components.InvoiceControl
{
    public class InvoiceControl : Control
    {
        public InvoiceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InvoiceControl), new FrameworkPropertyMetadata(typeof(InvoiceControl)));
        }
    }
}

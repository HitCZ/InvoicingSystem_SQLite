using System.Globalization;
using System.Threading;
using System.Windows;

namespace InvoicingSystem_SQLite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("cs-CZ");
        }
    }
}

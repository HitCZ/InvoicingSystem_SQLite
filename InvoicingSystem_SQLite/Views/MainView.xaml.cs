using InvoicingSystem_SQLite.ViewModels;

namespace InvoicingSystem_SQLite.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainViewModel ViewModel
        {
            get => DataContext as MainViewModel;
            set => DataContext = value;
        }

        public MainView()
        {
            ViewModel = new MainViewModel { ValidateFunc = () => InvoiceControl.Validate() };
            InitializeComponent();
        }
    }
}

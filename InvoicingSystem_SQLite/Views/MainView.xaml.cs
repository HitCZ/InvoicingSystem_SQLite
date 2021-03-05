using System.ComponentModel.Composition;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystem_SQLite.ViewModels;
using Microsoft.Practices.ServiceLocation;

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
            ViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            ViewModel.ValidateFunc = () => InvoiceControl.Validate();
            InitializeComponent();
        }
    }
}

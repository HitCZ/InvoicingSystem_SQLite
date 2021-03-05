using Invoicing.Enumerations;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystem_SQLite.Logic;
using InvoicingSystem_SQLite.Logic.Extensions;
using InvoicingSystem_SQLite.Logic.TestData;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace InvoicingSystem_SQLite.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly ISqlDataProvider<Invoice> invoiceDataProvider;

        #endregion Fields

        #region Properties

        #region Contractor

        public uint InvoiceNumber
        {
            get => GetPropertyValue<uint>();
            set => SetPropertyValue(value);
        }

        public string ContractorFirstName
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public string ContractorLastName
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public string ContractorStreet
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public string ContractorBuildingNumber
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public uint ContractorZipCode
        {
            get => GetPropertyValue<uint>();
            set => SetPropertyValue(value);
        }

        // ReSharper disable once InconsistentNaming
        public uint ContractorIN
        {
            get => GetPropertyValue<uint>();
            set => SetPropertyValue(value);
        }

        public string ContractorCity
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        // ReSharper disable once InconsistentNaming
        public uint IN
        {
            get => GetPropertyValue<uint>();
            set => SetPropertyValue(value);
        }

        // ReSharper disable once InconsistentNaming
        public bool IsVATPayer
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public string CityOfEvidence
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        #endregion Contractor

        #region Customer

        public string CustomerName
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public string CustomerStreet
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public string CustomerBuildingNumber
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public uint CustomerZipCode
        {
            get => GetPropertyValue<uint>();
            set => SetPropertyValue(value);
        }

        public string CustomerCity
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        // ReSharper disable once InconsistentNaming
        public uint CustomerIN
        {
            get => GetPropertyValue<uint>();
            set => SetPropertyValue(value);
        }

        // ReSharper disable once InconsistentNaming
        public string CustomerVATIN
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        #endregion Customer

        #region Other

        public DateTime SelectedDateOfIssue
        {
            get => GetPropertyValue<DateTime>();
            set => SetPropertyValue(value);
        }

        public DateTime SelectedDueDate
        {
            get => GetPropertyValue<DateTime>();
            set => SetPropertyValue(value);
        }

        public string JobDescription
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public ObservableCollection<ValueDescription> Currencies
        {
            get => GetPropertyValue<ObservableCollection<ValueDescription>>();
            set => SetPropertyValue(value);
        }

        public string IssuedBy
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public ObservableCollection<ValueDescription> PaymentMethods
        {
            get => GetPropertyValue<ObservableCollection<ValueDescription>>();
            set => SetPropertyValue(value);
        }

        public string BankConnection
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public string BankAccount
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public string VariableSymbol
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public Currency SelectedCurrency
        {
            get => GetPropertyValue<Currency>();
            set => SetPropertyValue(value);
        }

        public string Total
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public PaymentMethod SelectedPaymentMethod
        {
            get => GetPropertyValue<PaymentMethod>();
            set => SetPropertyValue(value);
        }

        #endregion Other

        public Func<bool> ValidateFunc
        {
            get => GetPropertyValue<Func<bool>>();
            set => SetPropertyValue(value);
        }

        #endregion Properties

        #region Commands

        public ICommand SaveCommand { get; set; }

        #endregion Commands

        [ImportingConstructor]
        public MainViewModel([Import(nameof(InvoiceDataProvider), typeof(ISqlDataProvider<Invoice>))] ISqlDataProvider<Invoice> invoiceDataProvider)
        {
            Initialize();
            InitializeCommands();

            this.invoiceDataProvider = invoiceDataProvider;
        }

        private void Initialize()
        {
            Currencies = new ObservableCollection<ValueDescription>(EnumExtensions<Currency>.GetAllValueDescriptions());
            SelectedCurrency = Currency.CZK;
            PaymentMethods = new ObservableCollection<ValueDescription>(EnumExtensions<PaymentMethod>.GetAllValueDescriptions(true));
            SelectedPaymentMethod = PaymentMethod.BankTransfer;
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);
        }

        private void SaveCommandExecute()
        {
            var invoice = SetupInvoiceObject();

            var response = invoiceDataProvider.CreateOrUpdate(InvoiceMocksProvider.GetInvoice());
        }

        private bool SaveCommandCanExecute() => ValidateFunc();

        private Invoice SetupInvoiceObject()
        {
            var invoice = new Invoice
            {
                BankInformation = SetupBankInformationObject(),
                Contractor = SetupContractorObject(),
                Customer = SetupCustomerObject(),
                PaymentMethod = SelectedPaymentMethod,
                Currency = SelectedCurrency,
                DueDate = SelectedDueDate,
                DateOfIssue = SelectedDateOfIssue,
                JobDescription = JobDescription,
                InvoiceNumber = InvoiceNumber,
                Price = Total.ConvertToDecimal() ?? 0
            };

            return invoice;
        }

        private Customer SetupCustomerObject()
        {
            var result = new Customer
            {
                Address = new Address
                {
                    ZipCode = CustomerZipCode,
                    BuildingNumber = CustomerBuildingNumber,
                    Street = CustomerStreet,
                    City = CustomerCity
                },
                CorporationName = CustomerName,
                IdentificationNumber = (int)CustomerIN,
                VATIN = CustomerVATIN
            };

            return result;
        }

        private BankInformation SetupBankInformationObject()
        {
            var result = new BankInformation
            {
                AccountNumber = BankAccount,
                VariableSymbol = VariableSymbol,
                BankConnection = BankConnection
            };

            return result;
        }

        private Contractor SetupContractorObject()
        {
            var result = new Contractor
            {
                Address = new Address
                {
                    BuildingNumber = ContractorBuildingNumber,
                    City = ContractorCity,
                    ZipCode = ContractorZipCode,
                    Street = ContractorStreet
                },
                IdentificationNumber = (int)ContractorIN,
                CityOfEvidence = ContractorCity,
                FirstName = ContractorFirstName,
                LastName = ContractorLastName,
                IsVatPayer = IsVATPayer
            };

            return result;
        }
    }
}

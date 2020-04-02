using Invoicing.Enumerations;
using Invoicing.Models;
using InvoicingSystem_SQLite.Logic;
using InvoicingSystem_SQLite.Logic.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InvoicingSystem_SQLite.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
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

        public MainViewModel()
        {
            Initialize();
            InitializeCommands();
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
        }

        private bool SaveCommandCanExecute() => ValidateFunc();

        private Invoice SetupInvoiceObject()
        {
            var invoice = new Invoice(null)
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
            var result = new Customer(null)
            {
                Address = new Address(null)
                {
                    ZipCode = CustomerZipCode,
                    BuildingNumber = CustomerBuildingNumber,
                    Street = CustomerStreet,
                    City = CustomerCity
                },
                CorporationName = CustomerName,
                IN = (int) CustomerIN,
                VATIN = CustomerVATIN
            };

            return result;
        }

        private BankInformation SetupBankInformationObject()
        {
            var result = new BankInformation(null)
            {
                AccountNumber = BankAccount,
                VariableSymbol = VariableSymbol,
                BankConnection = BankConnection
            };

            return result;
        }

        private Contractor SetupContractorObject()
        {
            var result = new Contractor(null)
            {
                Address = new Address(null)
                {
                    BuildingNumber = ContractorBuildingNumber,
                    City = ContractorCity,
                    ZipCode = ContractorZipCode,
                    Street = ContractorStreet
                },
                IN = (int) ContractorIN,
                CityOfEvidence = ContractorCity,
                FirstName = ContractorFirstName,
                LastName = ContractorLastName,
                IsVatPayer = IsVATPayer
            };

            return result;
        }
    }
}

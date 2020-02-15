using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Invoicing.Enumerations;
using InvoicingSystem_SQLite.Logic;

namespace InvoicingSystem_SQLite.Components.InvoiceControl
{
    public class InvoiceControl : Control
    {
        #region Dependency Properties

        public ulong InvoiceNumber
        {
            get => (ulong)GetValue(InvoiceNumberProperty);
            set => SetValue(InvoiceNumberProperty, value);
        }

        public static readonly DependencyProperty InvoiceNumberProperty =
            DependencyProperty.Register("InvoiceNumber", typeof(ulong), typeof(InvoiceControl));

        #region Contractor

        public string ContractorName
        {
            get => (string)GetValue(ContractorNameProperty);
            set => SetValue(ContractorNameProperty, value);
        }

        public static readonly DependencyProperty ContractorNameProperty =
            DependencyProperty.Register("ContractorName", typeof(string), typeof(InvoiceControl), new FrameworkPropertyMetadata(OnNameChanged));

        private static void OnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public string ContractorStreet
        {
            get => (string)GetValue(ContractorStreetProperty);
            set => SetValue(ContractorStreetProperty, value);
        }

        public static readonly DependencyProperty ContractorStreetProperty =
            DependencyProperty.Register("ContractorStreet", typeof(string), typeof(InvoiceControl));

        public string ContractorBuildingNumber
        {
            get => (string)GetValue(ContractorBuildingNumberProperty);
            set => SetValue(ContractorBuildingNumberProperty, value);
        }

        public static readonly DependencyProperty ContractorBuildingNumberProperty =
            DependencyProperty.Register("ContractorBuildingNumber", typeof(string), typeof(InvoiceControl));

        public uint ContractorZipCode
        {
            get => (uint)GetValue(ContractorZipCodeProperty);
            set => SetValue(ContractorZipCodeProperty, value);
        }

        public static readonly DependencyProperty ContractorZipCodeProperty =
            DependencyProperty.Register("ContractorZipCode", typeof(uint), typeof(InvoiceControl));

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public uint ContractorIN
        {
            get => (uint)GetValue(ContractorINProperty);
            set => SetValue(ContractorINProperty, value);
        }

        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty ContractorINProperty =
            DependencyProperty.Register("ContractorIN", typeof(uint), typeof(InvoiceControl));

        public string ContractorCity
        {
            get => (string)GetValue(ContractorCityProperty);
            set => SetValue(ContractorCityProperty, value);
        }

        public static readonly DependencyProperty ContractorCityProperty =
            DependencyProperty.Register("ContractorCity", typeof(string), typeof(InvoiceControl));

        // ReSharper disable once InconsistentNaming
        public uint IN
        {
            get => (uint)GetValue(INProperty);
            set => SetValue(INProperty, value);
        }

        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty INProperty =
            DependencyProperty.Register("IN", typeof(uint), typeof(InvoiceControl));

        // ReSharper disable once InconsistentNaming
        public bool IsVATPayer
        {
            get => (bool)GetValue(IsVATPayerProperty);
            set => SetValue(IsVATPayerProperty, value);
        }

        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty IsVATPayerProperty =
            DependencyProperty.Register("IsVATPayer", typeof(bool), typeof(InvoiceControl));

        public string CityOfEvidence
        {
            get => (string)GetValue(CityOfEvidenceProperty);
            set => SetValue(CityOfEvidenceProperty, value);
        }

        public static readonly DependencyProperty CityOfEvidenceProperty =
            DependencyProperty.Register("CityOfEvidence", typeof(string), typeof(InvoiceControl));

        #endregion Contractor

        #region Customer

        public string CustomerName
        {
            get => (string)GetValue(CustomerNameProperty);
            set => SetValue(CustomerNameProperty, value);
        }

        public static readonly DependencyProperty CustomerNameProperty =
            DependencyProperty.Register("CustomerName", typeof(string), typeof(InvoiceControl));

        public string CustomerStreet
        {
            get => (string)GetValue(CustomerStreetProperty);
            set => SetValue(CustomerStreetProperty, value);
        }

        public static readonly DependencyProperty CustomerStreetProperty =
            DependencyProperty.Register("CustomerStreet", typeof(string), typeof(InvoiceControl));

        public string CustomerBuildingNumber
        {
            get => (string)GetValue(CustomerBuildingNumberProperty);
            set => SetValue(CustomerBuildingNumberProperty, value);
        }

        public static readonly DependencyProperty CustomerBuildingNumberProperty =
            DependencyProperty.Register("CustomerBuildingNumber", typeof(string), typeof(InvoiceControl));

        public uint CustomerZipCode
        {
            get => (uint)GetValue(CustomerZipCodeProperty);
            set => SetValue(CustomerZipCodeProperty, value);
        }

        public static readonly DependencyProperty CustomerZipCodeProperty =
            DependencyProperty.Register("CustomerZipCode", typeof(uint), typeof(InvoiceControl));

        public string CustomerCity
        {
            get => (string)GetValue(CustomerCityProperty);
            set => SetValue(CustomerCityProperty, value);
        }

        public static readonly DependencyProperty CustomerCityProperty =
            DependencyProperty.Register("CustomerCity", typeof(string), typeof(InvoiceControl));


        // ReSharper disable once InconsistentNaming
        public uint CustomerIN
        {
            get => (uint)GetValue(CustomerINProperty);
            set => SetValue(CustomerINProperty, value);
        }

        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty CustomerINProperty =
            DependencyProperty.Register("CustomerIN", typeof(uint), typeof(InvoiceControl));

        // ReSharper disable once IdentifierTypo
        // ReSharper disable once InconsistentNaming
        public string CustomerVATIN
        {
            get => (string)GetValue(CustomerVATINProperty);
            set => SetValue(CustomerVATINProperty, value);
        }

        // ReSharper disable once IdentifierTypo
        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty CustomerVATINProperty =
            DependencyProperty.Register("CustomerVATIN", typeof(string), typeof(InvoiceControl));

        #endregion Customer

        #region Payment Conditions

        public ObservableCollection<ValueDescription<PaymentMethod>> PaymentMethods
        {
            get => (ObservableCollection<ValueDescription<PaymentMethod>>)GetValue(PaymentMethodsProperty);
            set => SetValue(PaymentMethodsProperty, value);
        }

        public static readonly DependencyProperty PaymentMethodsProperty =
            DependencyProperty.Register("PaymentMethods", typeof(ObservableCollection<ValueDescription<PaymentMethod>>), typeof(InvoiceControl));

        public string BankConnection
        {
            get => (string)GetValue(BankConnectionProperty);
            set => SetValue(BankConnectionProperty, value);
        }

        public static readonly DependencyProperty BankConnectionProperty =
            DependencyProperty.Register("BankConnection", typeof(string), typeof(InvoiceControl));

        public string BankAccount
        {
            get => (string)GetValue(BankAccountProperty);
            set => SetValue(BankAccountProperty, value);
        }

        public static readonly DependencyProperty BankAccountProperty =
            DependencyProperty.Register("BankAccount", typeof(string), typeof(InvoiceControl));

        public string VariableSymbol
        {
            get => (string)GetValue(VariableSymbolProperty);
            set => SetValue(VariableSymbolProperty, value);
        }

        public static readonly DependencyProperty VariableSymbolProperty =
            DependencyProperty.Register("VariableSymbol", typeof(string), typeof(InvoiceControl));

        #endregion Payment Conditions

        public DateTime DateOfIssue
        {
            get => (DateTime)GetValue(DateOfIssueProperty);
            set => SetValue(DateOfIssueProperty, value);
        }

        public static readonly DependencyProperty DateOfIssueProperty =
            DependencyProperty.Register("DateOfIssue", typeof(int), typeof(InvoiceControl));

        public DateTime DueDate
        {
            get => (DateTime)GetValue(DueDateProperty);
            set => SetValue(DueDateProperty, value);
        }

        public static readonly DependencyProperty DueDateProperty =
            DependencyProperty.Register("DueDate", typeof(int), typeof(InvoiceControl));

        public string JobDescription
        {
            get => (string)GetValue(JobDescriptionProperty);
            set => SetValue(JobDescriptionProperty, value);
        }

        public static readonly DependencyProperty JobDescriptionProperty =
            DependencyProperty.Register("JobDescription", typeof(string), typeof(InvoiceControl));

        public ObservableCollection<ValueDescription<Currency>> Currencies
        {
            get => (ObservableCollection<ValueDescription<Currency>>)GetValue(CurrenciesProperty);
            set => SetValue(CurrenciesProperty, value);
        }

        public static readonly DependencyProperty CurrenciesProperty =
            DependencyProperty.Register("Currencies", typeof(ObservableCollection<ValueDescription<Currency>>), typeof(InvoiceControl));

        public string IssuedBy
        {
            get => (string)GetValue(IssuedByProperty);
            set => SetValue(IssuedByProperty, value);
        }

        public static readonly DependencyProperty IssuedByProperty =
            DependencyProperty.Register("IssuedBy", typeof(string), typeof(InvoiceControl));

        #endregion Dependency Properties

        public InvoiceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InvoiceControl),
                new FrameworkPropertyMetadata(typeof(InvoiceControl)));
        }
    }
}

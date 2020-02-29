﻿using Invoicing.Enumerations;
using InvoicingSystem_SQLite.Logic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using InvoicingSystem_SQLite.Logic.Extensions;

namespace InvoicingSystem_SQLite.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        #region Contractor

        public ulong InvoiceNumber
        {
            get => GetPropertyValue<ulong>();
            set => SetPropertyValue(value);
        }

        public string ContractorName
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

        #endregion Properties

        public MainViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            Currencies = new ObservableCollection<ValueDescription>(EnumExtensions<Currency>.GetAllValueDescriptions());
            SelectedCurrency = Currency.CZK;
            PaymentMethods = new ObservableCollection<ValueDescription>(EnumExtensions<PaymentMethod>.GetAllValueDescriptions(true));
            SelectedPaymentMethod = PaymentMethod.BankTransfer;
        }

    }
}

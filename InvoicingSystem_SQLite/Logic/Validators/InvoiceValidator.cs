using System;
using InvoicingSystem_SQLite.Logic.Extensions;
using InvoicingSystem_SQLite.Properties;
using System.ComponentModel.Composition;

namespace InvoicingSystem_SQLite.Logic.Validators
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class InvoiceValidator : TextValidator
    {
        #region Fields

        private readonly DateValidator dateValidator;

        #endregion Fields

        #region Constructor

        [ImportingConstructor]
        public InvoiceValidator(DateValidator dateValidator)
        {
            this.dateValidator = dateValidator;
        }

        #endregion Constructor

        #region Public Methods

        public string ValidateName(string name) => ValidateAlphabeticalString(name, Strings.Name);

        public string ValidateStreet(string street) 
            => ValidateAlphabeticalString(street, Strings.Street);

        public string ValidateBuildingNumber(string buildingNumber) 
            => ValidateAlphaNumericString(buildingNumber, Strings.BuildingNumber);

        public string ValidateCity(string city) => ValidateAlphabeticalString(city, Strings.City);

        /// <summary>
        /// Checks if zipcode is bigger than 10 (only Jamaica seems to have a double digit zipcode).
        /// https://en.wikipedia.org/wiki/List_of_postal_codes
        /// </summary>
        public string ValidateZipCode(uint zipCode) => zipCode > 10 ? null : GetInvalidFormatMessage(Strings.ZipCode);

        /// <summary>
        /// Standard length of the IdentificationNumber should be 8 digits.
        /// https://cs.wikipedia.org/wiki/Identifika%C4%8Dn%C3%AD_%C4%8D%C3%ADslo_osoby
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string ValidateIN(uint IN) =>
            IN > 10000000 ? null : GetInvalidFormatMessage(Strings.IdentificationNumber);

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        public string ValidateVATIN(string VATIN) 
            => ValidateStartingWithTwoLettersAndAtLeastEightDigitsString(VATIN) 
                ? null : GetInvalidFormatMessage(Strings.VatIdentificationNumber);

        public string ValidateBankConnection(string bankConnection)
        {
            if (bankConnection.IsNullOrEmpty())
                return null;

            return ValidateTenDigitsString(bankConnection) ? null : GetInvalidFormatMessage(Strings.BankConnection);
        }

        public string ValidateBankAccountNumber(string bankAccountNumber)
            => ValidateTenDigitsSlashFourDigitsString(bankAccountNumber) 
                ? null : GetInvalidFormatMessage(Strings.AccountNumber);

        public string ValidateVariableSymbol(string variableSymbol)
        {
            if (variableSymbol.IsNullOrEmpty())
                return null;

            return ValidateAtLeastFourDigitsString(variableSymbol) 
                ? null : GetInvalidFormatMessage(Strings.VariableSymbol);
        }

        public string ValidateDate(DateTime date)
        {
            var isMatch = dateValidator.Validate(date);

            return isMatch
                ? null
                : string.Format(Strings.MSG_InvalidYear, dateValidator.MinimalAllowedYear,
                    dateValidator.MaximalAllowedYear);
        }

        public string ValidateJobDescription(string jobDescription) 
            => jobDescription.IsNullOrEmpty() ? GetCannotBeEmptyMessage(Strings.JobDescription) : null;

        public string ValidateIssuedBy(string issuedBy) 
            => ValidateAlphabeticalString(issuedBy) ? null : GetInvalidFormatMessage(Strings.IssuedBy);

        public string ValidateTotal(string total) 
            => ValidateNumericString(total) ? null : GetInvalidFormatMessage(Strings.Total);

        #endregion Public Methods

        #region Private Methods

        private string GetInvalidFormatMessage(string invalidPropertyName)
        {
            return string.Format(Strings.MSG_InvalidFormat, invalidPropertyName);
        }

        private string GetCannotBeEmptyMessage(string invalidPropertyName)
        {
            return string.Format(Strings.MSG_CannotBeEmpty, invalidPropertyName); 
        }

        private string ValidateAlphabeticalString(string input, string invalidPropertyName)
        {
            if (input.IsNullOrEmpty())
                return GetCannotBeEmptyMessage(invalidPropertyName);

            var isMatch = ValidateAlphabeticalString(input);
            return isMatch ? null : GetInvalidFormatMessage(invalidPropertyName);
        }

        private string ValidateAlphaNumericString(string input, string invalidPropertyName)
        {
            if (input.IsNullOrEmpty())
                return GetCannotBeEmptyMessage(invalidPropertyName);
            var isMatch = ValidateAlphaNumericString(input);
            return isMatch ? null : GetInvalidFormatMessage(invalidPropertyName);
        }

        #endregion Private Methods
    }
}

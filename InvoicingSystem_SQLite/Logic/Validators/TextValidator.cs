using InvoicingSystem_SQLite.Logic.Extensions;
using System.Text.RegularExpressions;

namespace InvoicingSystem_SQLite.Logic.Validators
{
    public abstract class TextValidator
    {
        #region Constants

        protected const string ALPHABETICAL_REGEX = "^[a-zA-Z]+$";
        protected const string ALPHA_NUMERIC_REGEX = "^[a-zA-Z0-9]+$";
        protected const string TWO_LETTERS_AT_LEAST_EIGHT_DIGITS_REGEX = "^[A-Z]{2}[0-9]{8,}$";
        protected const string TEN_DIGITS_SLASH_FOUR_DIGITS_REGEX = @"^[0-9]{10}\/[0-9]{4}$";
        protected const string TEN_DIGITS_REGEX = "^[0-9]{10}$";
        protected const string AT_LEAST_FOUR_DIGITS_REGEX = "^[0-9]{4,}$";

        #endregion Constants

        #region Protected Methods

        protected bool ValidateAlphabeticalString(string input) 
            => !input.IsNullOrEmpty() && CheckRegex(ALPHABETICAL_REGEX, input);

        protected bool ValidateAlphaNumericString(string input)
            => !input.IsNullOrEmpty() && CheckRegex(ALPHA_NUMERIC_REGEX, input);

        // ReSharper disable once InconsistentNaming
        protected bool ValidateStartingWithTwoLettersAndAtLeastEightDigitsString(string input)
            => !input.IsNullOrEmpty() && CheckRegex(TWO_LETTERS_AT_LEAST_EIGHT_DIGITS_REGEX, input);

        protected bool ValidateTenDigitsSlashFourDigitsString(string input)
            => !input.IsNullOrEmpty() && CheckRegex(TEN_DIGITS_SLASH_FOUR_DIGITS_REGEX, input);

        protected bool ValidateTenDigitsString(string input) 
            => !input.IsNullOrEmpty() && CheckRegex(TEN_DIGITS_REGEX, input);

        protected bool ValidateAtLeastFourDigitsString(string input) 
            => !input.IsNullOrEmpty() && CheckRegex(AT_LEAST_FOUR_DIGITS_REGEX, input);

        #endregion Protected Methods

        #region Private Methods

        private bool CheckRegex(string pattern, string input)
        {
            var regex = new Regex(pattern);
            var isMatch = regex.IsMatch(input);

            return isMatch;
        }

        #endregion Private Methods
    }
}

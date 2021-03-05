using System;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.Logic.Extensions
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value) => (T)Enum.Parse(typeof(T), value, true);

        public static bool IsNullOrEmpty(this string value) => value is null || value == string.Empty;

        public static bool IsNumber(this string value) => int.TryParse(value.Replace(" ", string.Empty), out _);

        public static decimal? ConvertToDecimal(this string value)
        {
            var canConvert = decimal.TryParse(value?.Replace(" ", string.Empty), out var number);

            if (canConvert)
                return number;

            return null;
        }

        /// <summary>
        /// Accepts string consisting of digits and/or separators. Formats the text into e.g. 10000 =>  10 000.
        /// </summary>
        public static string FormatIntoNumbers(this string value, char numericSeparator = ' ', int marginBetweenSeparators = 3)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            if (!value.All(c => char.IsDigit(c) || c == numericSeparator))
                throw new ArgumentException($"{value} must contain only digits and/or separators.");
            if (marginBetweenSeparators <= 0 || marginBetweenSeparators >= value.Length)
                return value;

            var builder = new StringBuilder();
            var textWithoutSeparators = value.Replace(numericSeparator.ToString(), string.Empty).Trim(numericSeparator, ' ');
            var textLength = textWithoutSeparators.Length;
            var step = 0;

            // Take chars from the end to beginning and count how many numbers have been added, if count is 3, insert separator
            for (var i = textLength - 1; i > -1; i--)
            {
                step++;
                var currentChar = textWithoutSeparators[i];

                builder.Insert(0, currentChar);

                if (step != marginBetweenSeparators)
                    continue;

                builder.Insert(0, numericSeparator);
                step = 0;
            }

            return builder.ToString().Trim(' ', numericSeparator);
        }
    }
}

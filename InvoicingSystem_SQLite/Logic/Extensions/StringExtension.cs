using System;

namespace InvoicingSystem.Logic.Extensions
{
    public static class StringExtension
    {
        public static T ParseEnum<T>(this string value) => (T)Enum.Parse(typeof(T), value, true);

        public static bool IsNullOrEmpty(this string value) => value is null || value == string.Empty;
    }
}

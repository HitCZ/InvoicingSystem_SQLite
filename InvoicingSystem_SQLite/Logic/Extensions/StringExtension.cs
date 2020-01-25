using System;

namespace InvoicingSystem.Logic.Extensions {
    public static class StringExtension {
        public static T ParseEnum<T> (this string value) {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Invoicing.Enumerations;
using InvoicingSystem_SQLite.Logic.Enumerations;

namespace InvoicingSystem_SQLite.Logic.Extensions
{
    public static class EnumExtensions<T> where T : Enum
    {
        public static List<ValueDescription> GetAllValueDescriptions(bool localizeDescription = false)
        {
            var enumType = typeof(T);
            var values = Enum.GetValues(enumType).OfType<T>().ToList();
            var result = values.Select(v => new ValueDescription(v, GetEnumDescription(v, localizeDescription))).ToList();

            return result;
        }

        public static string GetEnumDescription(T value, bool localizeDescription = false)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .ToList();

            var anyAttributes = attributes.Any();

            if (!anyAttributes)
                return value.ToString();

            if (!localizeDescription)
                return attributes.First().Description;

            // TODO: prasárna
            if (typeof(T) == typeof(PaymentMethod) &&
                Equals(Thread.CurrentThread.CurrentUICulture, CultureInfo.GetCultureInfo("cs-CZ")))
            {
                var allValueDescriptions = EnumExtensions<PaymentMethodCs>
                    .GetAllValueDescriptions();
                var description = allValueDescriptions.FirstOrDefault(v => v.Value.ToString() == value.ToString())
                    .Description;

                if (!(description is null))
                    return description;
            }

            return attributes.First().Description;
        }
    }
}

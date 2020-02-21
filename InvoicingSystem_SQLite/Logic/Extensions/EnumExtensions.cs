using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

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
            var description = GetDescriptionFromAttribute(typeof(T), value);

            if (!localizeDescription)
                return description;

            if (!Equals(Thread.CurrentThread.CurrentUICulture, CultureInfo.GetCultureInfo("cs-CZ")))
                return description;

            var localizedDescription = GetLocalizedDescription(value);

            return localizedDescription ?? description;
        }

        private static string GetLocalizedDescription(T value)
        {
            var enumName = typeof(T).Name;
            var localizedEnumType = Assembly.GetExecutingAssembly().GetTypes()
                .SingleOrDefault(t => t.IsEnum && t.IsPublic && t.Name.Equals($"{enumName}cs", StringComparison.OrdinalIgnoreCase));

            if (localizedEnumType is null)
                return null;

            var localizedDescription = GetDescriptionFromAttribute(localizedEnumType, value);

            return localizedDescription;
        }

        private static string GetDescriptionFromAttribute(Type type, T value)
        {
            var fieldInfo = type.GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .ToList();

            var anyAttributes = attributes.Any();

            return !anyAttributes ? value.ToString() : attributes.First().Description;
        }
    }
}

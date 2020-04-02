using InvoicingSystem_SQLite.Logic.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace InvoicingSystem_SQLite.Logic.Converters
{
    public class BoolToVisibilityMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.IsNullOrEmpty() || values.Any(v => !(v is bool)))
                return null;

            var hasFocus = (bool)values[0];
            var hasText = !(bool)values[1];

            return (hasFocus || hasText) ?  Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

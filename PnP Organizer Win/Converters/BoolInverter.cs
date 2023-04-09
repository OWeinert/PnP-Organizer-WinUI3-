using Microsoft.UI.Xaml.Data;
using System;

namespace PnPOrganizer.Converters
{
    public class BoolInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => Invert(value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => Invert(value);

        private bool Invert(object value)
        {
            if (value is not bool)
                throw new ArgumentException("Value type is not bool!", nameof(value));

            return !(bool)value;
        }
    }
}

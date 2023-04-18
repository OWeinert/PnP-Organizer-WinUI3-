using Microsoft.UI.Xaml.Data;
using System;

namespace PnPOrganizer.Converters
{
    public class BoolToAcceptCancelGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not bool)
                throw new ArgumentException(nameof(value), "value Type is not bool!");

            var bValue = (bool)value;
            return bValue ? "\uE8FB" : "\uE711";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

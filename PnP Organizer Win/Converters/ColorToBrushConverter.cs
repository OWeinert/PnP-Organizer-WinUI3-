using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace PnPOrganizer.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not Color)
                throw new ArgumentException("value Type is not Windows.UI.Color", nameof(value));
            var colValue = (Color)value;
            return new SolidColorBrush(colValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is not SolidColorBrush)
                throw new ArgumentException("value Type is not SolidColorBrush", nameof(value));
            var scbValue = (SolidColorBrush)value;
            return scbValue.Color;
        }
    }
}

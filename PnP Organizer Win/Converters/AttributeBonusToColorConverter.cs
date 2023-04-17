using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace PnPOrganizer.Converters
{
    /// <summary>
    /// Converts an int value into a SolidColorBrush with the Color depending on the value.
    /// value < 0 => Red
    /// value = 0 => White
    /// value > 0 => Green
    /// 
    /// Generally used in Attribute boni
    /// </summary>
    public class AttributeBonusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is not int)
                throw new ArgumentException(nameof(value));

            var aValue = (int)value;
            if (aValue < 0)
                return new SolidColorBrush(Colors.Red);
            if (aValue > 0)
                return new SolidColorBrush(Colors.LimeGreen);
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

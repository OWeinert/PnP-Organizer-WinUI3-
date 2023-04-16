using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace PnPOrganizer.Converters
{
    /// <summary>
    /// Returns a SolidColorBrush depending on the given pearl count.
    /// count > 0 => Green, count > 3 => Yellow, count > 7 => Red.
    /// </summary>
    public class PearlsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var iValue = (int)value;
            if (iValue >= 1)
            {
                if (iValue >= 4)
                {
                    if (iValue >= 7)
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    return new SolidColorBrush(Colors.Yellow);
                }
                return new SolidColorBrush(Colors.LimeGreen);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

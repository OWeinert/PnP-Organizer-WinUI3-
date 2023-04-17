using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Core.Character.SkillSystem;
using System;
using Windows.UI;

namespace PnPOrganizer.Converters
{
    public class SkillCategoryToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not SkillCategory)
                throw new ArgumentException("value type is not SkillCategory!", nameof(value));

            return new SolidColorBrush((Color)new SkillCategoryToColorConverter().Convert(value, targetType, parameter, language));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

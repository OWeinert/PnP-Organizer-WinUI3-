using Microsoft.UI.Xaml.Data;
using PnPOrganizer.Core;
using PnPOrganizer.Core.Character.SkillSystem;
using System;
namespace PnPOrganizer.Converters
{
    /// <summary>
    /// Converts the given SkillCategory into a Color
    /// </summary>
    public class SkillCategoryToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not SkillCategory)
                throw new ArgumentException("value type is not SkillCategory!", nameof(value));

            // HACK
            unchecked
            {
                return (SkillCategory)value switch
                {
                    SkillCategory.Melee => Palette.Blue,
                    SkillCategory.Ranged => Palette.Green,
                    _ => Palette.LightRed,
                };
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

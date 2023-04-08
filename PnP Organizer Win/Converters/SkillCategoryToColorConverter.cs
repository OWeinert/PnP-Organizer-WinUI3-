using CommunityToolkit.WinUI.UI.Controls.ColorPickerConverters;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Core;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Core.Character.SkillSystem;
using System;
namespace PnPOrganizer.Converters
{
    public class SkillCategoryToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not SkillCategory)
                throw new ArgumentException("value type is not SkillCategory!", nameof(value));

            return (SkillCategory)value switch
            {
                SkillCategory.Melee => Utils.ColorFromArgbValue(ISkillsService.MeleeSkillColor),
                SkillCategory.Ranged => Utils.ColorFromArgbValue(ISkillsService.RangedSkillColor),
                _ => Utils.ColorFromArgbValue(ISkillsService.CharSkillColor),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

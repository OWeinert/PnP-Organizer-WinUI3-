using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.ViewModels;

namespace PnPOrganizer.Selectors
{
    public class SaveFileInfoTemplateSelector : DataTemplateSelector
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DataTemplate EmptyCharacter { get; set; }
        public DataTemplate ValidCharacter { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if(item is SaveFileInfoViewModel saveFileInfo && saveFileInfo.HasSaveFileInfo)
            {
                return ValidCharacter;
            }
            return EmptyCharacter;
        }
    }
}

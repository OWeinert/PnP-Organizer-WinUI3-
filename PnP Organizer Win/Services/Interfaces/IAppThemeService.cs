using Microsoft.UI.Xaml;
using System.Collections.Generic;

namespace PnPOrganizer.Interfaces
{
    public interface IAppThemeService
    {
        IEnumerable<ElementTheme> AvailableThemes { get; }

        void Initialize(FrameworkElement rootElement);

        ElementTheme? LoadThemeSettings();

        bool SaveThemeSettings(ElementTheme theme);

        ElementTheme? GetTheme();

        void SetTheme(ElementTheme theme);
    }
}
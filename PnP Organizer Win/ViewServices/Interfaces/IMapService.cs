using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace PnPOrganizer.ViewServices.Interfaces
{
    public interface IMapService
    {
        public Frame MapFrame { get; }
        public Page? LastMap { get; }
        public Page? CurrentMap { get; }
        public Page? RootMap { get; }

        public void Initialize(Frame contentFrame);

        public void SetRootMap<TPage>() where TPage : Page;

        public void NavigateToMap<TPage>(object? parameters = null, NavigationTransitionInfo? transitionInfo = null) where TPage : Page;

        public void NavigateBack(object? parameters = null, NavigationTransitionInfo? transitionInfo = null);
    }
}

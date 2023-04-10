using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PnPOrganizer.ViewServices.Interfaces;
using System.Collections.ObjectModel;
using Windows.Services.Maps;

namespace PnPOrganizer.ViewServices
{
    public class MapService : IMapService
    {
        private Frame _mapFrame = new();
        public Frame MapFrame => _mapFrame;

        private Page? _lastMap;
        public Page? LastMap => _lastMap;

        private Page? _currentMap;
        public Page? CurrentMap => _currentMap;

        private Page? _rootMap;
        public Page? RootMap => _rootMap;

        public void Initialize(Frame contentFrame) 
        { 
            _mapFrame = contentFrame; 
        }

        public void SetRootMap<TPage>() where TPage : Page
        {
            MapFrame!.Navigate(typeof(TPage), null, new SuppressNavigationTransitionInfo());
            if (MapFrame.Content is TPage page)
            {
                _currentMap = page;
                _rootMap = _currentMap;
            }
        }

        public void NavigateBack(object? parameters = null, NavigationTransitionInfo? transitionInfo = null)
        {
            if (CurrentMap!.GetType() == RootMap!.GetType())
                return;
            if(LastMap != null)
            {
                transitionInfo ??= new SuppressNavigationTransitionInfo();
                PrepareAnimation();
                MapFrame.GoBack();
                if (MapFrame.Content is Page page)
                {
                    _currentMap = page;
                }
            }
        }

        public void NavigateToMap<TPage>(object? parameters = null, NavigationTransitionInfo? transitionInfo = null) where TPage : Page
        {
            transitionInfo ??= new SuppressNavigationTransitionInfo();
            PrepareAnimation();
            _mapFrame.Navigate(typeof(TPage), parameters, transitionInfo);
            if(MapFrame.Content is TPage page)
            {
                _lastMap = _currentMap;
                _currentMap = page;
            }
        }

        private void PrepareAnimation()
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", _mapFrame);
            ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation").Configuration = new DirectConnectedAnimationConfiguration();
        }
    }
}

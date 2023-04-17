using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace PnPOrganizer.Services.Interfaces
{
    public interface INavigationViewService
    {
        void Initialize(NavigationView navigationView, Frame contentFrame);

        void NavigateTo(Type pageType, NavigationTransitionInfo navigationTransitionInfo);
    }
}
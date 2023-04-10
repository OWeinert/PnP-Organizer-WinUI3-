﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PnPOrganizer.Views.Interfaces;
using PnPOrganizer.ViewModels.Maps;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;

namespace PnPOrganizer.Views.Maps
{
    public sealed partial class MapPageAlera : Page, IViewFor<MapPageAleraViewModel>
    {
        public MapPageAleraViewModel ViewModel { get; }

        public MapPageAlera()
        {
            ViewModel = Ioc.Default.GetService<MapPageAleraViewModel>()!;
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            anim?.TryStart(ContentGrid);
        }
    }
}
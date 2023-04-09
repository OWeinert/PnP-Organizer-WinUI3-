using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.ViewModels.Interfaces;
using PnPOrganizer.Views.Maps;
using PnPOrganizer.ViewServices.Interfaces;
using System;

namespace PnPOrganizer.ViewModels
{
    public partial class MapPageViewModel : IViewModel
    {
        private bool _initialized;
        public bool IsInitialized => _initialized;

        public event EventHandler? Initialized;

        public IMapService MapService { get; private set; }

        public MapPageViewModel(IMapService mapService)
        {
            MapService = mapService;
            Initialize();
        }

        public void SetupMapService(Frame contentFrame)
        {
            MapService.Initialize(contentFrame);
            MapService.SetRootMap<MapPageAlera>();
        }

        public void Initialize()
        {
            _initialized = true;
        }

        [RelayCommand]
        private void NavigateBack() => MapService.NavigateBack();
    }
}

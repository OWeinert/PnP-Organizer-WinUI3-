using CommunityToolkit.Mvvm.Input;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels.Interfaces;
using PnPOrganizer.Views.Maps;
using System;

namespace PnPOrganizer.ViewModels.Maps
{
    public partial class MapPageAleraViewModel : IViewModel
    {
        private bool _isInitialized;
        public bool IsInitialized { get; }

        public event EventHandler? Initialized;

        private IMapService _mapService;

        public MapPageAleraViewModel(IMapService mapService)
        {
            _mapService = mapService;
            Initialize();
        }

        public void Initialize()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void NavigateToParcia()
        {
            _mapService.NavigateToMap<MapPageParcia>();
        }

        [RelayCommand]
        private void NavigateToAttica()
        {
            _mapService.NavigateToMap<MapPageAttica>();
        }
    }
}

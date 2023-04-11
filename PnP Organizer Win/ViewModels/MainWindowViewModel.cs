using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Threading.Tasks;

namespace PnPOrganizer.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject, IViewModel
    {
        private bool _isInitialized;
        public bool IsInitialized => _isInitialized;

        public event EventHandler? Initialized;

        private readonly ISaveDataService _saveDataService;
        
        public MainWindowViewModel(ISaveDataService saveDataService) 
        {
            _saveDataService = saveDataService;
            Initialize();
        }

        public void Initialize()
        {
            if (_saveDataService.LoadedCharacter == null)
                _saveDataService.CreateNewCharacter();

            Initialized?.Invoke(this, EventArgs.Empty);
            _isInitialized = true;
        }

        [RelayCommand]
        private async Task OpenCharacter()
        {
            await _saveDataService.ShowOpenCharacterFilePicker();
        }

        [RelayCommand]
        private async Task SaveCharacter()
        {
            await _saveDataService.ShowOpenCharacterFilePicker();
        }

        [RelayCommand]
        private async Task SaveCharacterAs()
        {
            await _saveDataService.ShowSaveCharacterFilePicker();
        }
    }
}
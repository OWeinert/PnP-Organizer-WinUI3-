using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.Helpers;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels.Interfaces;
using PnPOrganizer.Views;
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
            if (!_saveDataService.IsSaved)
            {
                var window = Ioc.Default.GetRequiredService<MainWindow>();
                var result = await Dialogs.GetLoadCharacterConfirmDialog(window.XamlRoot, window.ActualTheme).ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await _saveDataService.ShowSaveCharacterFilePicker();
                    await ShowFilePicker();
                }
                else if (result == ContentDialogResult.Secondary)
                    await ShowFilePicker();
            }
            else
                await ShowFilePicker();

            async Task ShowFilePicker()
            {
                await _saveDataService.ShowOpenCharacterFilePicker();
            }
        }

        [RelayCommand]
        private async Task SaveCharacter()
        {
            if (_saveDataService.LoadedCharacterSaveInfo == null)
                await _saveDataService.ShowSaveCharacterFilePicker();
            else
                await _saveDataService.SaveLoadedCharacter();
        }

        [RelayCommand]
        private async Task SaveCharacterAs()
        {
            await _saveDataService.ShowSaveCharacterFilePicker();
        }
    }
}
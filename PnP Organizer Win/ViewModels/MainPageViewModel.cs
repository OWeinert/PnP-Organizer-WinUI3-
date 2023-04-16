using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PnPOrganizer.Services.Data;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels.Interfaces;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PnPOrganizer.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IViewModel
    {
        public const int MAX_SAVE_FILE_COUNT = 7;

        private bool _isInitialized;
        public bool IsInitialized => _isInitialized;

        public event EventHandler? Initialized;

        [ObservableProperty]
        private ObservableCollection<SaveFileInfoViewModel> _saveFileInfos = new();

        [ObservableProperty]
        private SaveFileInfoViewModel? _selectedSave;

        private int _currentSaveIndex = 0;

        public event EventHandler<SaveFileInfoViewModel?>? SelectedSaveChanged;

        private readonly ISaveDataService _saveDataService;
        private readonly ISaveFileInfoService _saveFileInfoService;

        private event EventHandler? _initAsync;

        public MainPageViewModel(ISaveDataService saveDataService, ISaveFileInfoService saveFileInfoService) 
        {
            _saveDataService = saveDataService;
            _saveFileInfoService = saveFileInfoService;
            Initialize();
        }

        partial void OnSelectedSaveChanged(SaveFileInfoViewModel? value) => SelectedSaveChanged?.Invoke(this, value);

        public void Initialize()
        {
            SaveFileInfos = new();
            for (var i = 0; i < MAX_SAVE_FILE_COUNT; i++)
            {
                var saveFileInfoVM = new SaveFileInfoViewModel(this);
                if (_saveFileInfoService.SaveFileInfos != null
                    && i < _saveFileInfoService.SaveFileInfos.Count
                    && _saveFileInfoService.SaveFileInfos[i] != null)
                {
                    saveFileInfoVM.SaveFileInfo = _saveFileInfoService.SaveFileInfos[i];
                }
                saveFileInfoVM.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName is nameof(SaveFileInfoViewModel.SaveFileInfo))
                    {
                        var saveFileInfo = ((SaveFileInfoViewModel)sender!).SaveFileInfo!;
                        _saveFileInfoService.AddSaveFileInfo(saveFileInfo);
                    }
                };
                SaveFileInfos.Add(saveFileInfoVM);
            }
            _initAsync += InitializeAsync;
            _initAsync?.Invoke(this, new EventArgs());
        }

        private async void InitializeAsync(object? sender, EventArgs e)
        {
            var _loadedSaveFiles = await _saveFileInfoService.LoadSaveFileInfos();
            _currentSaveIndex = _loadedSaveFiles != null ? _loadedSaveFiles.Count : 0;
            _currentSaveIndex %= MAX_SAVE_FILE_COUNT;
            if (_loadedSaveFiles != null) {
                for (var i = 0; i < _loadedSaveFiles!.Count; i++)
                {
                    SaveFileInfos[i].SaveFileInfo = _loadedSaveFiles[i];
                }
            }
            _saveDataService.CharacterSaveInfoCreated += SaveDataService_CharacterSaveInfoCreated;
            _isInitialized = true;
            Initialized?.Invoke(sender, e);
        }

        private void SaveDataService_CharacterSaveInfoCreated(object? sender, SaveFileInfo e)
        {
            var fileExists = SaveFileInfos
                .Where(sfiVM => sfiVM.SaveFileInfo != null)
                .Any(sfiVM => sfiVM.SaveFileInfo!.FilePath == e.FilePath || sfiVM.SaveFileInfo! == e);
            if(fileExists)
                return;

            SaveFileInfos[_currentSaveIndex].SaveFileInfo = e;
            _currentSaveIndex++;
            if(_currentSaveIndex >= MAX_SAVE_FILE_COUNT)
            {
                _currentSaveIndex = 0;
            }
        }
    }
}
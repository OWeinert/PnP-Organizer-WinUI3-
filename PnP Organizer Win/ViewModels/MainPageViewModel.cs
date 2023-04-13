using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.Services.Data;
using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace PnPOrganizer.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IViewModel
    {
        private bool _isInitialized;
        public bool IsInitialized => _isInitialized;

        public event EventHandler? Initialized;

        [ObservableProperty]
        private ObservableCollection<SaveFileInfo>? _saveFileInfos;

        public MainPageViewModel() 
        {
            Initialize();
        }

        public void Initialize()
        {
            SaveFileInfos = new();
            for(int i = 0; i < 5; i++)
            {
                SaveFileInfos.Add(SaveFileInfo.Dummy);
            }

            _isInitialized = true;
            Initialized?.Invoke(this, new EventArgs());
        }
    }
}
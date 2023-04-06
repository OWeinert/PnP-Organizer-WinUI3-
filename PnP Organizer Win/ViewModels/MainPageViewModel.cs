using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.ViewModels.Interfaces;
using System;

namespace PnPOrganizer.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IViewModel
    {
        public bool IsInitialized => throw new NotImplementedException();

        public event EventHandler? Initialized;

        public MainPageViewModel() 
        {
            Initialize();
        }

        public void Initialize()
        {
            
        }
    }
}
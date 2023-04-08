using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.ViewModels.Interfaces;
using PnPOrganizer.Views;
using PnPOrganizer.Views.Interfaces;
using System;

namespace PnPOrganizer.ViewModels
{
    public class CharacterPageViewModel : ObservableObject, IViewModel
    {
        private bool _isInitialized = false;
        internal static object MaxEnergyModifierBonus;

        public bool IsInitialized => _isInitialized;

        public static object MaxHealthModifierBonus { get; internal set; }
        public static object MaxStaminaModifierBonus { get; internal set; }
        public static object InitiativeModifierBonus { get; internal set; }

        public event EventHandler? Initialized;

        public CharacterPageViewModel()
        {
            Initialize();
        }

        public void Initialize()
        {

        }
    }
}

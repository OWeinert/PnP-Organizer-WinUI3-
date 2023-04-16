using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Globalization.NumberFormatting;

namespace PnPOrganizer.ViewModels
{
    public partial class CharacterPageViewModel : ObservableObject, IViewModel
    {
        private bool _isInitialized = false;
        
        public bool IsInitialized => _isInitialized;

        public IList<Core.Attributes.Attribute>? Attributes { get; private set; }
        public IList<Pearl>? Pearls { get; private set; }

        [ObservableProperty]
        private CharacterData? _characterData;

        // HACK
        public static object MaxHealthModifierBonus { get; internal set; }
        public static object MaxStaminaModifierBonus { get; internal set; }
        public static object InitiativeModifierBonus { get; internal set; }

        internal static object MaxEnergyModifierBonus;
        //

        public event EventHandler? Initialized;

        private readonly ISaveDataService _saveDataService;
        private readonly IPearlService _pearlService;
        private readonly IAttributeService _attributeService;

        public CharacterPageViewModel(ISaveDataService saveDataService, IPearlService pearlService, IAttributeService attributeService)
        {
            _saveDataService = saveDataService;
            _pearlService = pearlService;
            _attributeService = attributeService;
            Initialize();
        }

        public void Initialize()
        {
            CharacterData = _saveDataService.LoadedCharacter;
            Attributes = _attributeService.Attributes.Values.ToList();
            Pearls = _pearlService.Pearls.Values.ToList();
            foreach(var pearl in Pearls)
            {
                var index = Pearls.IndexOf(pearl);
                pearl.PropertyChanged += (sender, e) =>
                {
                    if(e.PropertyName is nameof(Pearl.Amount))
                    {
                        if (pearl.Amount <= 0)
                            pearl.Form = 1;
                    }
                };
            }
        }

        public static INumberFormatter2 GetIntFormatter()
        {
            var rounder = new IncrementNumberRounder
            {
                Increment = 1
            };

            var formatter = new DecimalFormatter
            {
                IntegerDigits = 1,
                FractionDigits = 0,
                NumberRounder = rounder
            };
            return formatter;
        }
    }
}
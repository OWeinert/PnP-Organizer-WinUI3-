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

        [ObservableProperty]
        private int _maxHealth;
        [ObservableProperty]
        private int _maxEnergy;
        [ObservableProperty]
        private int _maxStamina;
        [ObservableProperty]
        private int _initiative;

        [ObservableProperty]
        private int _maxHealthModifierBonus;
        [ObservableProperty]
        private int _maxEnergyModifierBonus;
        [ObservableProperty]
        private int _maxStaminaModifierBonus;
        [ObservableProperty]
        private int _initiativeModifierBonus;

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

            CharacterData.PropertyChanged += CharacterData_PropertyChanged;
            PropertyChanged += CharacterPageViewModel_PropertyChanged;

            foreach (var pearl in Pearls)
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
            foreach(var attribute in Attributes)
            {
                attribute.PropertyChanged += (sender, e) =>
                {
                    if(e.PropertyName is not nameof(Core.Attributes.Attribute.DisplayName))
                    {
                        CalculateMaxStats();
                    }
                };
            }
            CalculateMaxStats();
        }

        private void CharacterPageViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(MaxHealthModifierBonus) or nameof(MaxEnergyModifierBonus)
                or nameof(MaxStaminaModifierBonus) or nameof(InitiativeModifierBonus))
            {
                CalculateMaxStats();
            }
        }

        private void CharacterData_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName is nameof(CharacterData.MaxHealthBonus) or nameof(CharacterData.MaxEnergyBonus)
                or nameof(CharacterData.MaxStaminaBonus) or nameof(CharacterData.InitiativeBonus))
            {
                CalculateMaxStats();
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

        private void CalculateMaxStats()
        {
            var strength = _attributeService.Attributes[Core.Attributes.AttributeType.Strength].Value;
            var constitution = _attributeService.Attributes[Core.Attributes.AttributeType.Constitution].Value;
            var charisma = _attributeService.Attributes[Core.Attributes.AttributeType.Charisma].Value;
            var intelligence = _attributeService.Attributes[Core.Attributes.AttributeType.Intelligence].Value;
            var dexterity = _attributeService.Attributes[Core.Attributes.AttributeType.Dexterity].Value;
            var constitutionBonus = _attributeService.Attributes[Core.Attributes.AttributeType.Constitution].Bonus;
            var dexterityBonus = _attributeService.Attributes[Core.Attributes.AttributeType.Dexterity].Bonus;

            MaxHealth = strength + constitution + CharacterData.MaxHealthBonus + MaxHealthModifierBonus;
            MaxEnergy = (int)Math.Ceiling((constitution + charisma + intelligence) / 3f) + CharacterData.MaxEnergyBonus + MaxEnergyModifierBonus;
            MaxStamina = 15 + constitutionBonus + dexterityBonus + CharacterData.MaxStaminaBonus + MaxStaminaModifierBonus;
            Initiative = constitution + dexterity;
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.Core.Attributes;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Core.Character.Attributes;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.Core.Character.StatModifiers;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Globalization.NumberFormatting;

namespace PnPOrganizer.ViewModels
{
    public partial class CharacterPageViewModel : ObservableObject, IViewModel
    {
        private bool _isInitialized = false;
        
        public bool IsInitialized => _isInitialized;

        public List<AttributeCheck>? AttributeChecks { get; private set; }
        public IList<Core.Attributes.Attribute>? Attributes { get; private set; }
        public IList<Pearl>? Pearls { get; private set; }

        [ObservableProperty]
        private ObservableCollection<Profession>? _professions;

        [ObservableProperty]
        private ObservableCollection<SkillAttrCheckStatModContainer>? _activeSkills;

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
        private readonly IAttributeCheckService _attributeCheckService;
        private readonly ISkillsService _skillsService;

        public CharacterPageViewModel(ISaveDataService saveDataService, IPearlService pearlService, IAttributeService attributeService,
            IAttributeCheckService attributeCheckService, ISkillsService skillsService)
        {
            _saveDataService = saveDataService;
            _pearlService = pearlService;
            _attributeService = attributeService;
            _attributeCheckService = attributeCheckService;
            _skillsService = skillsService;

            Initialize();
        }

        public void Initialize()
        {
            CharacterData = _saveDataService.LoadedCharacter;
            Attributes = _attributeService.Attributes.Values.ToList();
            AttributeChecks = _attributeCheckService.GetAsList();
            Pearls = _pearlService.Pearls.Values.ToList();
            Professions = new ObservableCollection<Profession>();

            _saveDataService.CharacterCreated += SaveDataService_CharacterCreated;
            CharacterData!.PropertyChanged += CharacterData_PropertyChanged;
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

        private void SaveDataService_CharacterCreated(object? sender, CharacterData e)
        {
            CharacterData = e;
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
            var strength = _attributeService.Attributes[AttributeType.Strength].Value;
            var constitution = _attributeService.Attributes[AttributeType.Constitution].Value;
            var charisma = _attributeService.Attributes[AttributeType.Charisma].Value;
            var intelligence = _attributeService.Attributes[AttributeType.Intelligence].Value;
            var dexterity = _attributeService.Attributes[AttributeType.Dexterity].Value;
            var constitutionBonus = _attributeService.Attributes[AttributeType.Constitution].Bonus;
            var dexterityBonus = _attributeService.Attributes[AttributeType.Dexterity].Bonus;

            MaxHealth = strength + constitution + CharacterData!.MaxHealthBonus + MaxHealthModifierBonus;
            MaxEnergy = (int)Math.Ceiling((constitution + charisma + intelligence) / 3f) + CharacterData.MaxEnergyBonus + MaxEnergyModifierBonus;
            MaxStamina = 15 + constitutionBonus + dexterityBonus + CharacterData.MaxStaminaBonus + MaxStaminaModifierBonus;
            Initiative = constitution + dexterity + CharacterData.InitiativeBonus + InitiativeModifierBonus;
        }

        public void RefreshAttributeCheckBoni()
        {
            _attributeCheckService.ApplyStatModifiers();
            _attributeCheckService.RefreshAll();
        }

        public void RefreshActiveSkills()
        {
            ActiveSkills ??= new();
            ActiveSkills?.Clear();
            var validStatMods = _skillsService.GetActiveStatModifiers<AttributeCheckStatModifier>();
            var validSkills = _skillsService.GetFromStatModifierType<AttributeCheckStatModifier>()
                .Where(skill =>
                {
                    if(skill.StatModifiers != null)
                    {
                        return skill.StatModifiers.Any(statMod => statMod is AttributeCheckStatModifier attrCheckStatMod && validStatMods.Contains(attrCheckStatMod));
                    }
                    return false;
                });
            var result = validSkills.Zip(validStatMods).ToList().ConvertAll(t => new SkillAttrCheckStatModContainer(t.First, t.Second));
            ActiveSkills = new (result);
        }

        public void CreateProfession(AttributeCheck attributeCheck)
        {
            Professions!.Add(new Profession(attributeCheck));
        }

        public void RemoveProfession(Profession profession)
        {
            profession.RemoveFromAttributeCheck();
            Professions!.Remove(profession);
        }
    }

    public readonly struct SkillAttrCheckStatModContainer
    {
        public string SkillName { get; }
        public IStatModifier StatModifier { get; }

        public SkillAttrCheckStatModContainer(Skill skill, AttributeCheckStatModifier statModifier)
        {
            SkillName = skill.DisplayName;
            StatModifier = statModifier;
        }
    }
}
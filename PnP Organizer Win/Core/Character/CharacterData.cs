using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.Core.Character.Inventory;
using System;
using System.Collections.Generic;

namespace PnPOrganizer.Core.Character
{
    [INotifyPropertyChanged]
    public partial class CharacterData
    {
        public string FileName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public float Height { get; set; }
        public byte[] CharacterImage { get; set; } = Array.Empty<byte>();
        public string CharacterImageFileExt { get; set; } = string.Empty;

        public int CurrentHealth { get; set; }
        public int MaxHealthBonus { get; set; }
        public int CurrentEnergy { get; set; }
        public int MaxEnergyBonus { get; set; }
        public int CurrentStamina { get; set; }
        public int MaxStaminaBonus { get; set; }
        public int InitiativeBonus { get; set; }

        [ObservableProperty]
        private CharacterPearls _pearls = new();

        [ObservableProperty]
        private CharacterAttributes _attributes = new();

        public List<SkillSaveData> Skills { get; set; }
        public List<InventoryItem> Inventory { get; set; }

        public List<ProfessionSaveData> Professions { get; set; }

        public byte[] Notes { get; set; } = Array.Empty<byte>();

        public CharacterData()
        {
            Skills = new List<SkillSaveData>();
            Inventory = new List<InventoryItem>();
            Professions = new List<ProfessionSaveData>();
        }
    }

    public struct CharacterPearls
    {
        public int Earth { get; set; } = 0;
        public int Fire { get; set; } = 0;
        public int Air { get; set; } = 0;
        public int Metal { get; set; } = 0;
        public int Wood { get; set; } = 0;
        public int Water { get; set; } = 0;

        public CharacterPearls() { } 
    }

    public struct CharacterAttributes
    {
        public int Strength { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Charisma { get; set; } = 10;
        public int Intelligence { get; set; } = 10;

        public CharacterAttributes() { }
    }
}
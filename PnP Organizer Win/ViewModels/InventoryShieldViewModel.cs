﻿using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.Core.BattleAssistant;
using PnPOrganizer.Core.Character.Inventory;
using System.Collections.Generic;

namespace PnPOrganizer.Models
{
    public partial class InventoryShieldViewModel : InventoryItemViewModel
    {
        [ObservableProperty]
        private int _paradeBonus = 0;
        [ObservableProperty]
        private Dice? _paradeDiceBonus = Dice.D4;
        [ObservableProperty]
        private float _weight = 1.0f;

        public InventoryShieldViewModel() : this (new InventoryShield()) { }

        public InventoryShieldViewModel(InventoryShield inventoryShield) : base(inventoryShield)
        {
            IsInitialized = false;

            ParadeBonus = inventoryShield.ParadeBonus;
            ParadeDiceBonus = inventoryShield.ParadeDiceBonus;
            Weight = inventoryShield.Weight;
            PropertyChanged += InventoryShieldModel_PropertyChanged;

            IsInitialized = true;
        }

        private void InventoryShieldModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var inventoryShield = (InventoryShield)InventoryItem;

            inventoryShield.ParadeBonus = ParadeBonus;
            inventoryShield.ParadeDiceBonus = ParadeDiceBonus;
            inventoryShield.Weight = Weight;
        }

        internal override InventoryItemViewModel Copy()
        {
            return new InventoryShieldViewModel()
            {
                Name = Name,
                Description = Description,
                InventoryItem = InventoryItem,
                ItemImage = ItemImage,
                Brush = Brush,
                ParadeBonus = ParadeBonus,
                Weight = Weight,
                ParadeDiceBonus = ParadeDiceBonus
            };
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Core;
using PnPOrganizer.Core.BattleAssistant;
using PnPOrganizer.Core.Character.Inventory;
using System.Collections.Generic;

namespace PnPOrganizer.Models
{
    public partial class InventoryShieldModel : InventoryItemModel
    {
        [ObservableProperty]
        private int _paradeBonus = 0;
        [ObservableProperty]
        private Dice _paradeDiceBonus = Dice.D4;
        [ObservableProperty]
        private float _weight = 1.0f;

        [ObservableProperty]
        private List<Dice>? _dices;

        public InventoryShieldModel() : this (new InventoryShield()) { }

        public InventoryShieldModel(InventoryShield inventoryShield) : base(inventoryShield)
        {
            IsInitialized = false;

            Dices = Dice.Dices;

            ParadeBonus = inventoryShield.ParadeBonus;
            ParadeDiceBonus = inventoryShield.ParadeDiceBonus;
            Weight = inventoryShield.Weight;

            if (inventoryShield.Color != Utils.GetArgbColorValue(((SolidColorBrush)Application.Current.Resources["PalettePrimaryBrush"]).Color)
                && inventoryShield.Color != Utils.GetArgbColorValue(((SolidColorBrush)Application.Current.Resources["PaletteDeepPurpleBrush"]).Color))
            {
                Brush = new SolidColorBrush(Utils.GetColorFromArgbValue(inventoryShield.Color));
            }
            else
                Brush = (SolidColorBrush)Application.Current.Resources["PaletteDeepPurpleBrush"];

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
    }
}

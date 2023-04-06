using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Core;
using PnPOrganizer.Core.Character.Inventory;

namespace PnPOrganizer.Models
{
    public partial class InventoryArmorModel : InventoryItemModel
    {
        [ObservableProperty]
        private int _armor = 0;
        [ObservableProperty]
        private float _putOnTime = 0.0f;
        [ObservableProperty]
        private float _weight = 1.0f;
        [ObservableProperty]
        private float _loudness = 0.0f;

        public InventoryArmorModel() : this (new InventoryArmor()) { }

        public InventoryArmorModel(InventoryArmor inventoryArmor) : base(inventoryArmor)
        {
            IsInitialized = false;

            Armor = inventoryArmor.Armor;
            PutOnTime = inventoryArmor.PutOnTime;
            Weight = inventoryArmor.Weight;
            Loudness = inventoryArmor.Loudness;

            if (inventoryArmor.Color != Utils.GetArgbColorValue(((SolidColorBrush)Application.Current.Resources["PalettePrimaryBrush"]).Color)
                && inventoryArmor.Color != Utils.GetArgbColorValue(((SolidColorBrush)Application.Current.Resources["PaletteBrownBrush"]).Color))
            {
                Brush = new SolidColorBrush(Utils.GetColorFromArgbValue(inventoryArmor.Color));
            }
            else
                Brush = (SolidColorBrush)Application.Current.Resources["PaletteBrownBrush"];

            PropertyChanged += InventoryArmorModel_PropertyChanged;

            IsInitialized = true;
        }

        private void InventoryArmorModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var inventoryArmor = (InventoryArmor)InventoryItem;
            inventoryArmor.Armor = Armor;
            inventoryArmor.PutOnTime = PutOnTime;
            inventoryArmor.Weight = Weight;
            inventoryArmor.Loudness = Loudness;
        }
    }
}

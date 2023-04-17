using CommunityToolkit.Mvvm.ComponentModel;
using PnPOrganizer.Core.Character.Inventory;

namespace PnPOrganizer.Models
{
    public partial class InventoryArmorViewModel : InventoryItemViewModel
    {
        [ObservableProperty]
        private int _armor = 0;
        [ObservableProperty]
        private float _putOnTime = 0.0f;
        [ObservableProperty]
        private float _weight = 1.0f;
        [ObservableProperty]
        private float _loudness = 0.0f;

        public InventoryArmorViewModel() : this (new InventoryArmor()) { }

        public InventoryArmorViewModel(InventoryArmor inventoryArmor) : base(inventoryArmor)
        {
            IsInitialized = false;

            Armor = inventoryArmor.Armor;
            PutOnTime = inventoryArmor.PutOnTime;
            Weight = inventoryArmor.Weight;
            Loudness = inventoryArmor.Loudness;
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

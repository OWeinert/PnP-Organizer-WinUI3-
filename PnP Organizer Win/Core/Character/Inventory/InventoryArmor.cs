using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI;
using PnPOrganizer.Models;

namespace PnPOrganizer.Core.Character.Inventory
{
    public class InventoryArmor : InventoryItem
    {
        public int Armor { get; set; }
        public float PutOnTime { get; set; }
        public float Weight { get; set; }
        public float Loudness { get; set; }

        public InventoryArmor() : base()
        {
            Armor = 1;
            PutOnTime = 0.0f;
            Weight = 1.0f;
            Loudness = 1.0f;
        }

        public InventoryArmor(InventoryArmorViewModel inventoryShieldModel) : base(inventoryShieldModel)
        {
            Armor = inventoryShieldModel.Armor;
            PutOnTime = inventoryShieldModel.PutOnTime;
            Weight = inventoryShieldModel.Weight;
            Loudness = inventoryShieldModel.Loudness;
        }

        protected override int GetBaseColor() => DefaultPalette.Green.ToInt();
    }
}

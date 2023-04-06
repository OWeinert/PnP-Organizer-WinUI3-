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

        public InventoryArmor(InventoryArmorModel inventoryShieldModel) : base(inventoryShieldModel)
        {
            Armor = inventoryShieldModel.Armor;
            PutOnTime = inventoryShieldModel.PutOnTime;
            Weight = inventoryShieldModel.Weight;
            Loudness = inventoryShieldModel.Loudness;
        }
    }
}

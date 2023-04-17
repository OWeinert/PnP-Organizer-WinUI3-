using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI;
using PnPOrganizer.Core.BattleAssistant;
using PnPOrganizer.Models;

namespace PnPOrganizer.Core.Character.Inventory
{
    public class InventoryShield : InventoryItem
    {
        public int ParadeBonus { get; set; }
        public Dice ParadeDiceBonus { get; set; }
        public float Weight { get; set; }

        public InventoryShield() : base()
        {
            ParadeBonus = 0;
            ParadeDiceBonus = Dice.D4;
            Weight = 1.0f;
        }

        public InventoryShield(InventoryShieldViewModel inventoryShieldModel) : base(inventoryShieldModel)
        {
            ParadeBonus = inventoryShieldModel.ParadeBonus;
            ParadeDiceBonus = inventoryShieldModel.ParadeDiceBonus;
            Weight = inventoryShieldModel.Weight;
        }

        protected override int GetBaseColor() => Palette.LightRed.ToInt();
    }
}

using PnPOrganizer.Core.BattleAssistant;
using PnPOrganizer.Models;

namespace PnPOrganizer.Core.Character.Inventory
{
    public class InventoryWeapon : InventoryItem
    {
        public AttackMode AttackMode { get; set; }
        public int DiceRollCount { get; set; }
        public Dice BaseDamageDice { get; set; }
        public int BaseDamageBonus { get; set; }
        public int Armorpen { get; set; }
        public int HitBonus { get; set; }

        public float Weight { get; set; }
        public bool IsTwoHanded { get; set; }

        public InventoryWeapon() : base()
        {
            AttackMode = AttackMode.Melee;
            DiceRollCount = 1;
            BaseDamageDice = Dice.D6;
            BaseDamageBonus = 0;
            Armorpen = 0;
            HitBonus = 0;

            Weight = 1.0f;
            IsTwoHanded = false;
        }

        public InventoryWeapon(InventoryWeaponViewModel inventoryWeaponModel) : base(inventoryWeaponModel) 
        {
            AttackMode = inventoryWeaponModel.AttackMode;
            DiceRollCount = inventoryWeaponModel.DiceRollCount;
            BaseDamageDice = inventoryWeaponModel.BaseDamageDice;
            BaseDamageBonus = inventoryWeaponModel.BaseDamageBonus;
            Armorpen = inventoryWeaponModel.Armorpen;
            HitBonus = inventoryWeaponModel.HitBonus;

            Weight = inventoryWeaponModel.Weight;
            IsTwoHanded = inventoryWeaponModel.IsTwoHanded;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.Core.BattleAssistant;
using PnPOrganizer.Core.Character.Inventory;
using System.Collections.Generic;

namespace PnPOrganizer.Models
{
    public partial class InventoryWeaponViewModel : InventoryItemViewModel
    {
        [ObservableProperty]
        private AttackMode _attackMode = AttackMode.Melee;
        [ObservableProperty]
        private int _diceRollCount = 1;
        [ObservableProperty]
        private Dice? _baseDamageDice = Dice.D6;
        [ObservableProperty]
        private int _baseDamageBonus = 0;
        [ObservableProperty]
        private int _armorpen = 0;
        [ObservableProperty]
        private int _hitBonus = 0;

        [ObservableProperty]
        private float _weight = 1.0f;
        [ObservableProperty]
        private bool _isTwoHanded = false;

        [ObservableProperty]
        private Symbol _trueFalseSymbol = Symbol.Cancel;

        public InventoryWeaponViewModel() : this (new InventoryWeapon()) { }

        public InventoryWeaponViewModel(InventoryWeapon inventoryWeapon) : base(inventoryWeapon)
        {
            IsInitialized = false;

            AttackMode = inventoryWeapon.AttackMode;
            DiceRollCount = inventoryWeapon.DiceRollCount;
            BaseDamageDice = inventoryWeapon.BaseDamageDice;
            BaseDamageBonus = inventoryWeapon.BaseDamageBonus;
            Armorpen = inventoryWeapon.Armorpen;
            HitBonus = inventoryWeapon.HitBonus;

            Weight = inventoryWeapon.Weight;
            IsTwoHanded = inventoryWeapon.IsTwoHanded;

            PropertyChanged += InventoryWeaponModel_PropertyChanged;

            IsInitialized = true;
        }

        private void InventoryWeaponModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var inventoryWeapon = (InventoryWeapon)InventoryItem;
            if (e.PropertyName is not nameof(TrueFalseSymbol))
            {
                inventoryWeapon.AttackMode = AttackMode;
                inventoryWeapon.DiceRollCount = DiceRollCount;
                inventoryWeapon.BaseDamageDice = BaseDamageDice;
                inventoryWeapon.BaseDamageBonus = BaseDamageBonus;
                inventoryWeapon.Armorpen = Armorpen;
                inventoryWeapon.HitBonus = HitBonus;
                inventoryWeapon.Weight = Weight;
                inventoryWeapon.IsTwoHanded = IsTwoHanded;

                TrueFalseSymbol = IsTwoHanded ? Symbol.Accept : Symbol.Cancel;
            }
        }

        internal override InventoryItemViewModel Copy()
        {
            return new InventoryWeaponViewModel()
            {
                Name = Name,
                Description = Description,
                InventoryItem = InventoryItem,
                ItemImage = ItemImage,
                Brush = Brush,
                AttackMode = AttackMode,
                DiceRollCount = DiceRollCount,
                BaseDamageDice = BaseDamageDice,
                BaseDamageBonus = BaseDamageBonus,
                Armorpen = Armorpen,
                HitBonus = HitBonus,
                Weight = Weight,
                IsTwoHanded = IsTwoHanded
            };
        }
    }
}

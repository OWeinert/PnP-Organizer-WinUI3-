using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Core;
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
        private Dice _baseDamageDice = Dice.D6;
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

        [ObservableProperty]
        private List<Dice>? _dices;

        public InventoryWeaponViewModel() : this (new InventoryWeapon()) { }

        public InventoryWeaponViewModel(InventoryWeapon inventoryWeapon) : base(inventoryWeapon)
        {
            IsInitialized = false;

            Dices = Dice.Dices;

            AttackMode = inventoryWeapon.AttackMode;
            DiceRollCount = inventoryWeapon.DiceRollCount;
            BaseDamageDice = inventoryWeapon.BaseDamageDice;
            BaseDamageBonus = inventoryWeapon.BaseDamageBonus;
            Armorpen = inventoryWeapon.Armorpen;
            HitBonus = inventoryWeapon.HitBonus;

            Weight = inventoryWeapon.Weight;
            IsTwoHanded = inventoryWeapon.IsTwoHanded;

            if (inventoryWeapon.Color != Utils.GetArgbColorValue(((SolidColorBrush)Application.Current.Resources["PalettePrimaryBrush"]).Color)
                && inventoryWeapon.Color != Utils.GetArgbColorValue(((SolidColorBrush)Application.Current.Resources["PaletteIndigoBrush"]).Color))
            {
                Brush = new SolidColorBrush(Utils.ColorFromArgbValue(inventoryWeapon.Color));
            }
            else
                Brush = (SolidColorBrush)Application.Current.Resources["PaletteIndigoBrush"];

            PropertyChanged += InventoryWeaponModel_PropertyChanged;

            IsInitialized = true;
        }

        private void InventoryWeaponModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var inventoryWeapon = (InventoryWeapon)InventoryItem;
            if (e.PropertyName is not nameof(TrueFalseSymbol) and not nameof(Dices))
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
    }
}

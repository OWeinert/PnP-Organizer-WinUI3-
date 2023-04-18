﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.Core.Character.Inventory;
using PnPOrganizer.Models;
using System.Diagnostics;

namespace PnPOrganizer.Selectors
{
    public class InventoryItemTemplateSelector : DataTemplateSelector
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DataTemplate Item { get; set; }
        public DataTemplate Weapon { get; set; }
        public DataTemplate Armor { get; set; }
        public DataTemplate Shield { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if(item != null && item is InventoryItemViewModel itemViewModel)
            {
                return itemViewModel.InventoryItem switch
                {
                    InventoryWeapon => Weapon,
                    InventoryArmor => Armor,
                    InventoryShield => Shield,
                    InventoryItem => Item,
                    _ => base.SelectTemplateCore(item)
                };
            }
            return base.SelectTemplateCore(item);
        }

    }
}
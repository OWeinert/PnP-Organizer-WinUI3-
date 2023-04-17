using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Media.Imaging;
using PnPOrganizer.Models;
using System;
using System.Xml.Serialization;

namespace PnPOrganizer.Core.Character.Inventory
{
    [XmlInclude(typeof(InventoryWeapon))]
    [XmlInclude(typeof(InventoryArmor))]
    [XmlInclude(typeof(InventoryShield))]
    public class InventoryItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[]? ItemImageBytes { get; set; }
        [Obsolete("Not used anymore! Only there because of SaveData (de)serialization")]
        public string ItemImageFileExt { get; set; } = string.Empty;
        public int Color { get; set; }

        public InventoryItem(string name, string description)
        {
            Color = GetBaseColor();
            Name = name;
            Description = description;
            ItemImageBytes = Array.Empty<byte>();
        }

        public InventoryItem(InventoryItemViewModel inventoryItemViewModel)
        {
            Color = inventoryItemViewModel.Brush!.Color.ToInt();
            ItemImageBytes = inventoryItemViewModel.ItemImage != null ? Utils.BitmapToBytesAsync(inventoryItemViewModel.ItemImage).Result : null;
            Name = inventoryItemViewModel.Name;
            Description = inventoryItemViewModel.Description;
        }

        public InventoryItem() : this(string.Empty, string.Empty) { }

        public void SetItemImage(BitmapImage image)
        {
            ItemImageBytes = Utils.BitmapToBytesAsync(image).Result;
        }

        protected virtual int GetBaseColor() => Palette.DarkGray.ToInt();
    }
}

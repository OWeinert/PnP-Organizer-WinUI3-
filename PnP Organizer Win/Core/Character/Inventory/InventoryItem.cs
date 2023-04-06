using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Models;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Graphics.Imaging;

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
        public string ItemImageFileExt { get; set; }
        public uint Color { get; set; }

        public InventoryItem(string name, string description)
        {
            Name = name;
            Description = description;
            ItemImageBytes = Array.Empty<byte>();
            ItemImageFileExt = string.Empty;
            Color = 0xFF222222;
        }

        public InventoryItem(InventoryItemModel inventoryItemModel)
        {
            Color = Utils.GetArgbColorValue(inventoryItemModel.Brush!.Color);
            //ItemImageBytes = Utils.BitmapToBytesAsync(inventoryItemModel.ItemImage).Result.GetResults().ToArray();
            //ItemImageFileExt = inventoryItemModel.ItemImage != null ? Path.GetExtension(inventoryItemModel.ItemImage.UriSource.AbsolutePath) : string.Empty;
            Name = inventoryItemModel.Name;
            Description = inventoryItemModel.Description;
        }

        public InventoryItem() : this(string.Empty, string.Empty) { }

        public void SetItemImage(SoftwareBitmap image)
        {
            ItemImageBytes = Utils.BitmapToBytesAsync(image).Result.GetResults().ToArray();
            ItemImageFileExt = ""; // TODO ItemImageFileExt
        }
    }
}

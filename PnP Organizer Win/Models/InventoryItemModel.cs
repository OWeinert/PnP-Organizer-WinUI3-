using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using PnPOrganizer.Core;
using PnPOrganizer.Core.Character.Inventory;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;

namespace PnPOrganizer.Models
{
    /// <summary>
    /// Data for Inventory items
    /// </summary>
    public partial class InventoryItemModel : ObservableObject
    {
        private InventoryItem _inventoryItem;
        public InventoryItem InventoryItem
        {
            get { return _inventoryItem; }
            set { _inventoryItem = value; }
        }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private SoftwareBitmapSource? _itemImage;
        private string _itemImageFileExt = string.Empty;

        [ObservableProperty]
        private SolidColorBrush? _brush;

        [ObservableProperty]
        private SolidColorBrush? foreground;

        protected bool IsInitialized = false;

        public InventoryItemModel() : this(new InventoryItem()) { }

        public InventoryItemModel(InventoryItem inventoryItem)
        {
            PropertyChanged += OnItemPropertyChanged;
            InventoryItem = inventoryItem;

            Name = _inventoryItem!.Name;
            Description = _inventoryItem.Description;
            /*
             * if (_inventoryItem.ItemImageBytes != null)
                ItemImage = Utils.BitmapImageFromBytesAsync(_inventoryItem.ItemImageBytes.AsBuffer()).Result;
             */
            _itemImageFileExt = inventoryItem.ItemImageFileExt;
            Brush = new SolidColorBrush(Utils.GetColorFromArgbValue(_inventoryItem.Color));
            Foreground = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];

            IsInitialized = true;
        }

        private async void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (IsInitialized)
            {
                switch (e.PropertyName)
                {
                    case nameof(Name):
                        _inventoryItem.Name = Name;
                        break;
                    case nameof(Description):
                        _inventoryItem.Description = Description;
                        break;
                    case nameof(ItemImage):
                        //_inventoryItem.ItemImageBytes = (await Utils.BitmapToBytesAsync(ItemImage, _itemImageFileExt)).GetResults().ToArray();
                        _inventoryItem.ItemImageFileExt = _itemImageFileExt;
                        break;
                    case nameof(Brush):
                        _inventoryItem.Color = Utils.GetArgbColorValue(Brush!.Color);
                        break;
                    default:
                        break;
                }

                //FileIO.IsCharacterSaved = false;
            }            
        }
    }
}

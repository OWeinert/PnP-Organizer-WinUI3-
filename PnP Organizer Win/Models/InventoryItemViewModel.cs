﻿using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using PnPOrganizer.Core;
using PnPOrganizer.Core.Character.Inventory;
using System;
using System.ComponentModel;

namespace PnPOrganizer.Models
{
    /// <summary>
    /// Data for Inventory items
    /// </summary>
    public partial class InventoryItemViewModel : ObservableObject
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
        private BitmapImage? _itemImage;

        [ObservableProperty]
        private SolidColorBrush? _brush;

        [ObservableProperty]
        private SolidColorBrush? foreground;

        protected bool IsInitialized = false;

        public InventoryItemViewModel() : this(new InventoryItem()) { }

        public InventoryItemViewModel(InventoryItem inventoryItem)
        {
            PropertyChanged += OnItemPropertyChanged;
            InventoryItem = inventoryItem;

            Name = _inventoryItem!.Name;
            Description = _inventoryItem.Description;
            if (_inventoryItem.ItemImageBytes != null)
                ItemImage = Utils.BitmapFromBytes(_inventoryItem.ItemImageBytes);
            Brush = new SolidColorBrush(Utils.ColorFromArgbValue(_inventoryItem.Color));
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
                        _inventoryItem.ItemImageBytes = (await Utils.BitmapToBytesAsync(ItemImage));
                        break;
                    case nameof(Brush):
                        _inventoryItem.Color = Utils.GetArgbColorValue(Brush!.Color);
                        break;
                    default:
                        break;
                }
            }            
        }

        internal InventoryItemViewModel Copy()
        {
            return new InventoryItemViewModel()
            {
                Name = Name,
                Description = Description,
                ItemImage = ItemImage,
                Brush = Brush,
                Foreground = Foreground,
                InventoryItem = _inventoryItem
            };
        }
    }
}
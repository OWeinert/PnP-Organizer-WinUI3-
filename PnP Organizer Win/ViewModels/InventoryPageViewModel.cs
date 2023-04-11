using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml.Media;
using PnPOrganizer.Models;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels.Interfaces;
using System;
using System.Collections.ObjectModel;
using Windows.UI;

namespace PnPOrganizer.ViewModels
{
    public partial class InventoryPageViewModel : ObservableObject, IViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InventoryItemModel>? _itemModels;

        private IAdvancedCollectionView? _itemsView;
        public IAdvancedCollectionView ItemsView
        {
            get
            {
                _itemsView ??= new AdvancedCollectionView(ItemModels);
                return _itemsView;
            }
            set => SetProperty(ref _itemsView, value);
        }

        private bool _initialized = false;
        public bool IsInitialized => _initialized;

        public event EventHandler? Initialized;

        private readonly IInventoryService _inventoryService;

        public InventoryPageViewModel(IInventoryService inventoryService) 
        {
            _inventoryService = inventoryService;
            Initialize();
        }
        
        public void Initialize()
        {
            ItemModels = _inventoryService.ItemModels;

            // HACK Placeholder until real items are fully implemented
            var random = new Random();
            for(var i = 0; i < 64; i++)
            {
                var itemModel = new InventoryItemModel()
                {
                    Name = $"Item No. {i + 1}",
                    Description = $"Description of Item No. {i + 1}",
                    Brush = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 256), (byte)random.Next(0, 256), (byte)random.Next(0, 256)))
                };
                _inventoryService.AddItem(itemModel);
            }
            _initialized = true;
            Initialized?.Invoke(this, new EventArgs());
        }
    }
}

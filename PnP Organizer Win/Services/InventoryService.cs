using PnPOrganizer.Core.Character;
using PnPOrganizer.Core.Character.Inventory;
using PnPOrganizer.Models;
using PnPOrganizer.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace PnPOrganizer.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ObservableCollection<InventoryItemViewModel> _itemModels = new();
        public ObservableCollection<InventoryItemViewModel> ItemModels => _itemModels;

        public InventoryItemViewModel AddItem(InventoryItem item)
        {
            var itemModel = new InventoryItemViewModel(item);
            return AddItem(itemModel);
        }

        public InventoryItemViewModel AddItem(InventoryItemViewModel item)
        {
            ItemModels.Add(item);
            return item;
        }

        public void RemoveItem(InventoryItemViewModel item) => ItemModels.Remove(item);

        public void ClearInventory() => ItemModels.Clear();

        public void LoadFromCharacter(CharacterData data)
        {
            ClearInventory();
            var characterInventory = data.Inventory;
            foreach(var item in characterInventory)
            {
                AddItem(item);
            }
        }

        public void SaveToCharacter(ref CharacterData data)
        {
            var currentInventory = ItemModels.ToList().ConvertAll(itemModel => itemModel.InventoryItem);
            data.Inventory = currentInventory;
        }

        public void ResetInventory()
        {
            ItemModels.Clear();
        }
    }
}

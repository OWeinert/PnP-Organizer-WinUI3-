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
        private readonly ObservableCollection<InventoryItemModel> _itemModels = new();
        public ObservableCollection<InventoryItemModel> ItemModels => _itemModels;

        public InventoryItemModel AddItem(InventoryItem item)
        {
            var itemModel = new InventoryItemModel(item);
            return AddItem(itemModel);
        }

        public InventoryItemModel AddItem(InventoryItemModel item)
        {
            ItemModels.Add(item);
            return item;
        }

        public void RemoveItem(InventoryItemModel item) => ItemModels.Remove(item);

        public void ClearInventory() => ItemModels.Clear();

        public void LoadInventory(CharacterData data)
        {
            ClearInventory();
            var characterInventory = data.Inventory;
            foreach(var item in characterInventory)
            {
                AddItem(item);
            }
        }

        public void SaveInventory(ref CharacterData data)
        {
            var currentInventory = ItemModels.ToList().ConvertAll(itemModel => itemModel.InventoryItem);
            data.Inventory = currentInventory;
        }
    }
}

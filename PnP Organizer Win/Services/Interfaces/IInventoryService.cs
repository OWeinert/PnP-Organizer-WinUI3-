using PnPOrganizer.Core.Character;
using PnPOrganizer.Core.Character.Inventory;
using PnPOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnPOrganizer.Services.Interfaces
{
    public interface IInventoryService
    {
        public ObservableCollection<InventoryItemModel> ItemModels { get; }

        public InventoryItemModel AddItem(InventoryItem item);

        public InventoryItemModel AddItem(InventoryItemModel item);

        public void RemoveItem(InventoryItemModel item);

        public void ClearInventory();

        public void LoadInventory(CharacterData data);

        public void SaveInventory(ref CharacterData data);
    }
}

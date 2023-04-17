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
    public interface IInventoryService : ISaveData
    {
        public ObservableCollection<InventoryItemViewModel> ItemModels { get; }

        public InventoryItemViewModel AddItem(InventoryItem item);

        public InventoryItemViewModel AddItem(InventoryItemViewModel item);

        public void RemoveItem(InventoryItemViewModel item);

        public void ClearInventory();
    }
}

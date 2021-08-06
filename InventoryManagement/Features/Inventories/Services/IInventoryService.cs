using InventoryManagement.Features.Inventories.Models;
using InventoryManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Inventories.Services
{
    public interface IInventoryService
    {
        Task<InventoryDetails> AddInventory(string inventoryName);
        Task<IEnumerable<InventoryDetails>> AddInventories(AddInventories[] inventories);
        Task DeleteInventory(int id);
        Task<IEnumerable<InventoryDetails>> GetInventories();
        Task<InventoryDetails> GetInventoryById(int id);
        Task<Inventory> UpdateInventory(string inventoryId, UpdateInventory inventory);
    }
}

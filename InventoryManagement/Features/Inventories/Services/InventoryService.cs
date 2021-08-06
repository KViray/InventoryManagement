using InventoryManagement.Context;
using InventoryManagement.Features.Inventories.Models;
using InventoryManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Inventories.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly Functions _functions;

        public InventoryService(InventoryDbContext inventoryDbContext,Functions functions)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
        }

        public Task<InventoryDetails> AddInventory(string inventoryName)
        {
            var newInventory = new Inventory
            {
                InventoryName = inventoryName
            };

            _inventoryDbContext.Inventory.Add(newInventory);
            _inventoryDbContext.SaveChanges();
            return Task.FromResult(MapToInventoryDetails(newInventory));
        }
        public async Task<IEnumerable<InventoryDetails>> AddInventories(AddInventories[] inventories)
        {
            var listOfInventories = new List<InventoryDetails>();
            foreach (var inventory in inventories)
            {
                var newInventory = new Inventory
                {
                    Image = inventory.Image,
                    InventoryName = inventory.InventoryName,
                    IsDeleted = 0
                };
                listOfInventories.Add(MapToInventoryDetails(newInventory));
                _inventoryDbContext.Inventory.Add(newInventory);
                _inventoryDbContext.SaveChanges();
            }
            return await Task.FromResult(listOfInventories);
        }
        public async Task DeleteInventory(int id)
        {
            var inv = _inventoryDbContext.Inventory.Where(inventory => inventory.InventoryId == id).SingleOrDefault();
            inv.IsDeleted = 1;
            await Task.FromResult(_inventoryDbContext.SaveChanges());

        }

        public async Task<IEnumerable<InventoryDetails>> GetInventories()
        {
            var inventory = _inventoryDbContext.Inventory.Where(inv => inv.IsDeleted == 0).Select(MapToInventoryDetails);
            return await Task.FromResult(inventory);
        }

        public Task<InventoryDetails> GetInventoryById(int id)
        {
            var details = new InventoryDetails();
            var getInventory = _inventoryDbContext.Inventory.Where(inv => inv.IsDeleted == 0 && inv.InventoryId == id).FirstOrDefault();

            if (getInventory == null) return null;

            return Task.FromResult(MapToInventoryDetails(getInventory));

        }

        public Task<Inventory> UpdateInventory(string inventoryId, UpdateInventory inventory)
        {
            var inv = _inventoryDbContext.Inventory.Where(invent => $"{invent.InventoryId}" == inventoryId && invent.IsDeleted == 0).FirstOrDefault();
            if (inv == null) return null;

            _functions.UpdateDetails(inventoryId, inv, inventory);
            _inventoryDbContext.SaveChanges();
            return Task.FromResult(inv);

        }
        private InventoryDetails MapToInventoryDetails(Inventory inventory)
        {
            return new InventoryDetails
            {
                InventoryId = inventory.InventoryId,
                InventoryName = inventory.InventoryName,
                Image = inventory.Image
            };
        }
    }
}

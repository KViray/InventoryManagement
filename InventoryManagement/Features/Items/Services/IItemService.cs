using InventoryManagement.Classes.Paging;
using InventoryManagement.Enums;
using InventoryManagement.Features.Items.Models;
using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Items.Services
{
    public interface IItemService
    {
        Task<ItemDetails> AddItem(AddItem addItem,string userId);

        Task<IEnumerable<ItemDetails>> AddItems(AddItems[] itemDetails);

        Task DeleteItem(string id);
        Task<ItemDetails> GetItemById(string id);
        Task<Item> UpdateItem(string id, UpdateItem updateItem, string userId);
        Task<IEnumerable<ItemDetails>> UpdateItemStatus(Guid employeeId, Guid[] itemIds, string type);
        Task<PagedData> GetItems(GetItem getItem, Paging paging);
    }
}

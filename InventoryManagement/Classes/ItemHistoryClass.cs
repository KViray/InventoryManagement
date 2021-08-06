using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Classes
{
    public abstract class ItemHistoryClass
    {
        public abstract ItemHistory AddOrUpdateItemHistory(Item item, string message, string userId);
    }
    public class AddItemHistory : ItemHistoryClass
    {
        public override ItemHistory AddOrUpdateItemHistory(Item item, string message, string userId)
        {
            return new ItemHistory
            {
                Id = Guid.NewGuid(),
                ItemId = item.ItemId,
                Message = message,
                CreatedBy = Guid.Parse(userId),
                DateCreated = DateTime.Now,
                IsDeleted = 0
            };
        }
    }
    public class UpdateItemHistory : ItemHistoryClass
    {
        public override ItemHistory AddOrUpdateItemHistory(Item item, string message, string userId)
        {
            return new ItemHistory
            {
                Id = Guid.NewGuid(),
                ItemId = item.ItemId,
                Message = message,
                UpdatedBy = Guid.Parse(userId),
                DateUpdated = DateTime.Now,
                IsDeleted = 0
            };
        }
    }
}

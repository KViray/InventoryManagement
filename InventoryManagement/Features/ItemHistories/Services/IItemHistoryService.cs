using InventoryManagement.Classes.Paging;
using InventoryManagement.Features.ItemHistories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.ItemHistories.Services
{
    public interface IItemHistoryService
    {
        Task<PagedData> GetItemHistoryDetails(GetItemHistoryDetails getItemHistoryDetails, Paging paging);
    }
}

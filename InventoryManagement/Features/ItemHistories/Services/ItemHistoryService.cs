using InventoryManagement.Classes.Paging;
using InventoryManagement.Context;
using InventoryManagement.Features.ItemHistories.Models;
using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Features.ItemHistories.Services
{
    public class ItemHistoryService : IItemHistoryService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly Functions _functions;
        public ItemHistoryService(InventoryDbContext inventoryDbContext, Functions functions)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
        }

        public async Task<PagedData> GetItemHistoryDetails(GetItemHistoryDetails getItemHistoryDetails, Paging paging)
        {
            IEnumerable<ItemHistoryDetails> getItemHistory;
            if (_functions.ParameterNullChecker(getItemHistoryDetails))
            {
                getItemHistory = _inventoryDbContext.ItemHistory.Where(history => history.IsDeleted == 0).Select(MapToItemHistoryDetails);
            }
            else
            {
                getItemHistory = _inventoryDbContext.ItemHistory.Where(history => ((getItemHistoryDetails.ItemId == null || history.ItemId == getItemHistoryDetails.ItemId)
                                                                               && (getItemHistoryDetails.CreatedBy == null || history.CreatedBy == getItemHistoryDetails.CreatedBy)
                                                                               && (getItemHistoryDetails.UpdatedBy == null || history.UpdatedBy == getItemHistoryDetails.UpdatedBy)
                                                                               && (getItemHistoryDetails.DateCreated == null ||
                                                                               (getItemHistoryDetails.DateCreated.Value.Date == history.DateCreated.Value.Date
                                                                               && getItemHistoryDetails.DateCreated.Value.Month == history.DateCreated.Value.Month
                                                                               && getItemHistoryDetails.DateCreated.Value.Year == history.DateCreated.Value.Year))
                                                                               && (getItemHistoryDetails.DateUpdated == null ||
                                                                               (getItemHistoryDetails.DateUpdated.Value.Date == history.DateUpdated.Value.Date
                                                                               && getItemHistoryDetails.DateUpdated.Value.Month == history.DateUpdated.Value.Month
                                                                               && getItemHistoryDetails.DateUpdated.Value.Year == history.DateUpdated.Value.Year)))
                                                                               && history.IsDeleted == 0).Select(MapToItemHistoryDetails);
            }

            if (_functions.ParameterNullChecker(paging))
            {
                return await Task.FromResult(_functions.MapToPagedData(PagedList<ItemHistoryDetails>.ToPagedList(getItemHistory, 1, getItemHistory.Count())));
            }

            return await Task.FromResult(_functions.MapToPagedData(PagedList<ItemHistoryDetails>.ToPagedList(getItemHistory, paging.Page.Value, paging.Limit.Value)));
        }

        private ItemHistoryDetails MapToItemHistoryDetails(ItemHistory itemHistory)
        {
            return new ItemHistoryDetails
            {
                ItemId = itemHistory.ItemId,
                Message = itemHistory.Message,
                CreatedBy = itemHistory.CreatedBy,
                DateCreated = itemHistory.DateCreated,
                UpdatedBy = itemHistory.UpdatedBy,
                DateUpdated = itemHistory.DateUpdated
            };
        }
    }
}

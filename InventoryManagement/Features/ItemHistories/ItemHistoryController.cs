using InventoryManagement.Classes.Paging;
using InventoryManagement.Features.ItemHistories.Models;
using InventoryManagement.Features.ItemHistories.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace InventoryManagement.Features.ItemHistories
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemHistoryController : ControllerBase
    {
        private readonly IItemHistoryService _itemHistoryService;
        public ItemHistoryController(IItemHistoryService itemHistoryService)
        {
            _itemHistoryService = itemHistoryService;
        }
        [HttpGet]
        public async Task<ActionResult<OkResult>> GetItemHistory([FromQuery] GetItemHistoryDetails getItemHistoryDetails, [FromQuery] Paging paging)
        {
            var result = await _itemHistoryService.GetItemHistoryDetails(getItemHistoryDetails, paging);
            if (paging.Page > result.TotalPages) return BadRequest(new IndexOutOfRangeException());
            
            return Ok(result);
        }
    }

}

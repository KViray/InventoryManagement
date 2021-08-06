using InventoryManagement.Classes.Paging;
using InventoryManagement.Enums;
using InventoryManagement.Exceptions;
using InventoryManagement.Features.Items.Models;
using InventoryManagement.Features.Items.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Items
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController: ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService employeeService)
        {
            _itemService = employeeService;
        }

        [HttpPost]
        public async Task<ItemDetails> AddItem(AddItem item)
        {
            string userId = HttpContext.User.Identity.Name;
            return await _itemService.AddItem(item, userId);
        }
        [HttpPost("addItems")]
        public async Task<ActionResult<OkResult>> AddItems(AddItems[] itemDetails)
        {
            var items = await _itemService.AddItems(itemDetails);
            return Ok(items);
        }
        [HttpPatch("updateItem")]
        public async Task<ActionResult<OkResult>> UpdateItem(string id, [FromForm] UpdateItem item)
        {
            string userId = HttpContext.User.Identity.Name;
            var items = await _itemService.UpdateItem(id, item, userId);
            if (items == null)  return NotFound(new IdNotFoundException("Item"));

            return Ok(items);
        }
        
        [HttpPatch("updateItemStatus")]
        public async Task<ActionResult<OkResult>> UpdateItemStatus(Guid employeeId, Guid[] itemIds, ItemStatusTypes type)
        {
            var update = await _itemService.UpdateItemStatus(employeeId, itemIds, $"{type}");
            return Ok(update);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OkResult>> GetItemById(string id)
        {
            var getItembyId = await _itemService.GetItemById(id);
            if (getItembyId == null) return NotFound();
            
            return Ok(getItembyId);
        }

        [HttpGet]
        public async Task<ActionResult<OkResult>> GetItems([FromQuery] GetItem getItem, [FromQuery]Paging paging)
        {
            var result = await _itemService.GetItems(getItem, paging);
            if (paging.Page > result.TotalPages) return BadRequest(new IndexOutOfRangeException());
            return Ok(result);
        }

        [HttpPut("delete")]
        public async Task DeleteItem(string id)
        {
            await _itemService.DeleteItem(id);
        }
    }

}

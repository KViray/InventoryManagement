using InventoryManagement.Classes.Paging;
using InventoryManagement.Exceptions;
using InventoryManagement.Features.Inventories.Models;
using InventoryManagement.Features.Inventories.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Inventories
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost]
        public async Task<InventoryDetails> AddInventory(string inventoryName)
        {
            return await _inventoryService.AddInventory(inventoryName);
        }
        [HttpPost("addInventories")]
        public async Task<ActionResult<OkResult>> AddInventories(AddInventories[] inventories)
        {
            var result =  await _inventoryService.AddInventories(inventories);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<OkResult>> GetAllInventories()
        { 
            var result =  await _inventoryService.GetInventories();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OkResult>> GetInventoryById(int id)
        {
            var getInventory = await _inventoryService.GetInventoryById(id);
            if (getInventory == null) return NotFound();
            return Ok(getInventory);

        }
        [HttpPatch("updateInventory")]
        public async Task<ActionResult<OkResult>> UpdateInventory(string inventoryId, [FromForm] UpdateInventory inventory)
        {
            var inv = await _inventoryService.UpdateInventory(inventoryId, inventory);
            if (inv == null) return NotFound(new IdNotFoundException("Inventory"));

            return Ok(inv);
        }
        [HttpPut("delete")]
        public async Task DeleteInventory(int id)
        {
            await _inventoryService.DeleteInventory(id);
        }
    }
}

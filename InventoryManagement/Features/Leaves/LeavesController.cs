using InventoryManagement.Exceptions;
using InventoryManagement.Features.Leaves.Models;
using InventoryManagement.Features.Leaves.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Leaves
{

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LeavesController: ControllerBase
    {
        private readonly ILeavesService _leavesService;
        public LeavesController(ILeavesService leavesService)
        {
            _leavesService = leavesService;
        }

        [HttpPost]
        public async Task<ActionResult<OkResult>> AddLeave([FromForm] AddLeave leaves)
        {
            var leave = await _leavesService.AddLeave(leaves);
            if(leave == null) return NotFound(new IdNotFoundException());
            return Ok(leave);
        }
        [HttpPut("updateLeaveDetails")]
        public async Task<ActionResult<OkResult>> UpdateLeaveDetails(string employeeId,[FromForm] UpdateLeave leavesDetails)
        {
            var leave = await _leavesService.UpdateLeaves(employeeId, leavesDetails);
            if (leave == null) return NotFound(new IdNotFoundException());
            return Ok(leave);
        }
        [HttpGet]
        public async Task<ActionResult<OkResult>> GetLeaves([FromQuery] GetLeaves getLeaves)
        {
            var leave = await _leavesService.GetLeaves(getLeaves);
            return Ok(leave);
        }

    }
}

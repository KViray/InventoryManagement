using InventoryManagement.Features.Leaves.Models;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Leaves.Services
{
    public interface ILeavesService
    {
        Task<LeavesDetails> AddLeave(AddLeave leaves);
        Task<IEnumerable<LeavesDetails>> GetLeaves([FromQuery] GetLeaves getLeaves);
        Task<Leave> UpdateLeaves(string employeeId, UpdateLeave leavesDetails);
    }
}

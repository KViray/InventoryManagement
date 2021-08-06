using InventoryManagement.Context;
using InventoryManagement.Features.Leaves.Models;
using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Leaves.Services
{
    internal class LeavesService : ILeavesService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly Functions _functions;
        public LeavesService(InventoryDbContext inventoryDbContext,Functions functions)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
        }

        public async Task<LeavesDetails> AddLeave(AddLeave leaves)
        {
            var findEmployee = _inventoryDbContext.Employee.Where(emp => $"{emp.Id}" == leaves.EmployeeId).FirstOrDefault();
            if (findEmployee == null) return null;

            var newLeave = new Leave
            {
                EmployeeId = leaves.EmployeeId,
                StartDate = leaves.StartDate,
                EndDate = leaves.EndDate,
                TypeOfLeave = $"{leaves.TypeOfLeave}",
                WhereToSpendLeave = leaves.WhereToSpendLeave,
                Reason = leaves.Reason,
                WorkingDaysAppliedFor = leaves.WorkingDaysAppliedFor,
                Status = $"{Enums.Status.Pending}"
            };
            _inventoryDbContext.Leave.Add(newLeave);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(MapToLeavesDetails(newLeave));
        }

        public async Task<IEnumerable<LeavesDetails>> GetLeaves(GetLeaves getLeaves)
        {
            IEnumerable<LeavesDetails> getAllLeaves;
            if (_functions.ParameterNullChecker(getLeaves))
            {
                getAllLeaves = _inventoryDbContext.Leave.Where(leave => leave.IsDeleted == 0).Select(MapToLeavesDetails);
            }
            else
            {
                getAllLeaves = _inventoryDbContext.Leave.Where(leave => ((getLeaves.EmployeeId == null || leave.EmployeeId == getLeaves.EmployeeId)
                                                                         && (getLeaves.StartDate == null || leave.StartDate == getLeaves.StartDate)
                                                                         && (getLeaves.EndDate == null || leave.EndDate == getLeaves.EndDate)
                                                                         && ($"{getLeaves.TypeOfLeave}" == null || leave.TypeOfLeave == $"{getLeaves.TypeOfLeave}"))
                                                                         && leave.IsDeleted == 0).Select(MapToLeavesDetails);
            }
            return await Task.FromResult(getAllLeaves);
        }

        public async Task<Leave> UpdateLeaves(string employeeId, UpdateLeave leavesDetails)
        {
            var leave = _inventoryDbContext.Leave.Where(leaves => leaves.EmployeeId == employeeId
                                                               && leaves.IsDeleted == 0).FirstOrDefault();
            if (leave == null) return null;

            _functions.UpdateDetails(employeeId, leave, leavesDetails);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(leave);
        }

        private LeavesDetails MapToLeavesDetails(Leave leaves)
        {
            return new LeavesDetails
            {
                EmployeeId = leaves.EmployeeId,
                StartDate = leaves.StartDate,
                EndDate = leaves.EndDate,
                TypeOfLeave = leaves.TypeOfLeave,
                WhereToSpendLeave = leaves.WhereToSpendLeave,
                Reason = leaves.Reason,
                WorkingDaysAppliedFor = leaves.WorkingDaysAppliedFor,
                Status = leaves.Status
            };
        }
    }
}

using InventoryManagement.Enums;
using System;

namespace InventoryManagement.Features.Leaves.Models
{
    public class AddLeave
    {
        public string EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveTypes TypeOfLeave { get; set; }
        public string WhereToSpendLeave { get; set; }
        public string Reason { get; set; }
        public int WorkingDaysAppliedFor { get; set; }
    }
    public class UpdateLeave
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveTypes TypeOfLeave { get; set; }
        public string WhereToSpendLeave { get; set; }
        public string Reason { get; set; }
        public int WorkingDaysAppliedFor { get; set; }
        public Status Status { get; set; }
    }
    public class GetLeaves
    {
        public string EmployeeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeaveTypes TypeOfLeave { get; set; }
    }
    public class LeavesDetails
    {
        public string EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TypeOfLeave { get; set; }
        public string WhereToSpendLeave { get; set; }
        public string Reason { get; set; }
        public int WorkingDaysAppliedFor { get; set; }
        public string Status { get; set; }
    }

}

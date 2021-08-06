using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Leave : Common
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LeaveId { get; set; }
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

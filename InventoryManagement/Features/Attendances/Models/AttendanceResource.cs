using System;

namespace InventoryManagement.Features.Attendances.Models
{
    public class AttendanceResource
    {
        public string EmployeeId { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }
    }
    public class GetAttendance
    {
        public string EmployeeId { get; set; }
        public DateTime? Date { get; set; }
    }
    public class UpdateAttendance
    {
        public DateTime TimeOut { get; set; }
        public string Remarks { get; set; }
    }
}

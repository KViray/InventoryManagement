using InventoryManagement.Features.Attendances.Models;
using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Attendances.Services
{
    public interface IAttendanceService
    {
        Task<AttendanceResource> TimeInOrOut(string employeeId, string remarks);
        Task<IEnumerable<AttendanceResource>> GetAttendance(GetAttendance getAttendance);
    }
}


using InventoryManagement.Context;
using InventoryManagement.Features.Attendances.Models;
using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Attendances.Services
{
    internal class AttendanceService : IAttendanceService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly Functions _functions;
        public AttendanceService(InventoryDbContext inventoryDbContext,Functions functions)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
        }
        public async Task<IEnumerable<AttendanceResource>> GetAttendance(GetAttendance getAttendance)
        {
            IEnumerable<AttendanceResource> findAttendance;

            if (_functions.ParameterNullChecker(getAttendance))
            {
                findAttendance = _inventoryDbContext.Attendance.Select(MapToAttendanceResource);
            }
            else
            {
                findAttendance = _inventoryDbContext.Attendance.Where(findAtt => (getAttendance.EmployeeId == null || findAtt.EmployeeId.ToLower() == getAttendance.EmployeeId.ToLower()) 
                                                                              && (getAttendance.Date == null || findAtt.Date.Year == getAttendance.Date.Value.Year
                                                                              && findAtt.Date.Month == getAttendance.Date.Value.Month
                                                                              && findAtt.Date.Day == getAttendance.Date.Value.Day))
                                                                              .Select(MapToAttendanceResource);
            }
            return await Task.FromResult(findAttendance);
        }

        public async Task<AttendanceResource> TimeInOrOut(string employeeId, string remarks)
        {
            var findEmployee = _inventoryDbContext.Employee.Any(emp => emp.EmployeeId.ToLower() == employeeId.ToLower());
            if (findEmployee)
            {
                var findDate = _inventoryDbContext.Attendance.Select(attend => attend.Date.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")
                                                                            && attend.EmployeeId.ToLower() == employeeId.ToLower()).FirstOrDefault();
                if (!findDate)
                {
                    var addAttendance = new Attendance
                    {
                        EmployeeId = employeeId.ToUpper(),
                        TimeIn = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")),
                        Date = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")),
                        Remarks = remarks
                    };
                    _inventoryDbContext.Attendance.Add(addAttendance);
                    _inventoryDbContext.SaveChanges();
                    return await Task.FromResult(MapToAttendanceResource(addAttendance));
                }
                else
                {
                    var attendance = _inventoryDbContext.Attendance.Where(att => att.Date.Year == DateTime.Now.Year
                                                                              && att.Date.Month == DateTime.Now.Month
                                                                              && att.Date.Day == DateTime.Now.Day
                                                                              && att.EmployeeId == employeeId).FirstOrDefault();

                    var updateAttendance = new UpdateAttendance
                    {
                        TimeOut = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")),
                        Remarks = remarks
                    };
                    _functions.UpdateDetails(employeeId, attendance, updateAttendance);
                    _inventoryDbContext.SaveChanges();

                    return await Task.FromResult(MapToAttendanceResource(attendance));
                }
            }
            return null;

        }
        private AttendanceResource MapToAttendanceResource(Attendance attendance)
        {
            return new AttendanceResource
            {
                EmployeeId = attendance.EmployeeId,
                TimeIn = attendance.TimeIn,
                TimeOut = attendance.TimeOut,
                Date = attendance.Date,
                Remarks = attendance.Remarks
            };
        }
    }
}

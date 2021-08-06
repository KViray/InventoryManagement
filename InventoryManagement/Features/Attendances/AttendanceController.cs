using InventoryManagement.Exceptions;
using InventoryManagement.Features.Attendances.Models;
using InventoryManagement.Features.Attendances.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Attendances
{

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AttendanceController: ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPut]
        public async Task<ActionResult<OkResult>> AddOrUpdateAttendance(string employeeId, string remarks)
        {
            var attendance = await _attendanceService.TimeInOrOut(employeeId,remarks);

            if(attendance == null) return NotFound(new IdNotFoundException());

            return Ok(attendance);
        }
        [HttpGet]
        public async Task<ActionResult<OkResult>> GetAttendance([FromQuery] GetAttendance getAttendance)
        {
            var attendance = await _attendanceService.GetAttendance(getAttendance);
            return Ok(attendance);
        }
    }
}

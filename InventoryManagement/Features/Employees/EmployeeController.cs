using InventoryManagement.Classes.Paging;
using InventoryManagement.Exceptions;
using InventoryManagement.Features.Employees.Models;
using InventoryManagement.Features.Employees.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Employees
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController: ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<ActionResult<OkResult>> AddEmployee([FromForm] AddEmployee employee)
        {
            var result = await _employeeService.AddEmployee(employee);
            return Ok(result);
        }
        [HttpPost("addEmployees")]
        public async Task<ActionResult<OkResult>> AddEmployees(AddEmployees[] employees)
        {
            var result = await _employeeService.AddEmployees(employees);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<OkResult>> GetAllEmployees([FromQuery] Paging paging)
        {
            var result = await _employeeService.GetEmployees(paging);

            if (paging.Page > result.TotalPages) return BadRequest(new IndexOutOfRangeException());

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<ActionResult<OkResult>> GetEmployeesById(string id)
        {
            var getEmployees = await _employeeService.GetEmployeeById(id);

            if (getEmployees == null) return NotFound();

            return Ok(getEmployees);

        }

        [HttpPatch("updateEmployee")]
        public async Task<ActionResult<OkResult>> UpdateEmployee(string employeeId, [FromForm] EmployeeDetails<IFormFile> employee)
        {
            var emp = await _employeeService.UpdateEmployee(employeeId, employee);

            if (emp == null) return NotFound(new IdNotFoundException("Employee"));

            return Ok(emp);

        }

        [HttpPut("delete")]
        public async Task DeleteEmployee(string id)
        {
            await _employeeService.DeleteEmployee(id);
        }

    }
}

using InventoryManagement.Exceptions;
using InventoryManagement.Features.Salaries.Model;
using InventoryManagement.Features.Salaries.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Logins
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;
        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpPost]
        public async Task<ActionResult<OkResult>> AddSalary(string employeeId, SalaryBinding salaryBinding)
        {
            var salary = await _salaryService.AddSalary(employeeId, salaryBinding);
            if (salary == null) return NotFound(new IdNotFoundException());

            return Ok(salary);

        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<OkResult>> GetSalary(string employeeId)
        {
            var salaryDetails = await _salaryService.GetSalaryDetails(employeeId);
            if (salaryDetails == null) return NotFound(new IdNotFoundException());

            return Ok(salaryDetails);
        }

        [HttpPost("UpdateSalary")]
        public async Task<ActionResult<OkResult>> UpdateSalary(string employeeId, SalaryResource salaryResource)
        {
            var salary = await _salaryService.UpdateSalary(employeeId, salaryResource);
            if (salary == null) return NotFound(new IdNotFoundException());

            return Ok(salary);

        }
    }
}

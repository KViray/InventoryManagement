using InventoryManagement.Classes.Paging;
using InventoryManagement.Features.Employees.Models;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Employees.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDetails<string>> AddEmployee(AddEmployee addEmployee);
        Task<IEnumerable<EmployeeDetails<string>>> AddEmployees(AddEmployees[] employees);
        Task DeleteEmployee(string id);
        Task<PagedData> GetEmployees(Paging paging);
        Task<EmployeeDetails<string>> GetEmployeeById(string id);

        Task<Employee> UpdateEmployee(string employeeId,EmployeeDetails<IFormFile> employee);
    }
}

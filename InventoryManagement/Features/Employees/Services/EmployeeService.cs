using InventoryManagement.Classes.Paging;
using InventoryManagement.Context;
using InventoryManagement.Features.Employees.Models;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Employees.Services
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly Functions _functions;
        
        public EmployeeService(InventoryDbContext inventoryDbContext, Functions functions)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
        }
        public async Task<EmployeeDetails<string>> AddEmployee(AddEmployee addEmployee)
        {
            DateTime dateToday = DateTime.Today;
            var age = dateToday.Year - addEmployee.BirthDate.Year;
            if(addEmployee.BirthDate.Date > dateToday.AddYears(-age))
            {
                age--;
            }
            var newEmployee = new Employee
            {
                Id = new Guid(),
                EmployeeId = addEmployee.EmployeeId,
                UserType = $"{addEmployee.UserType}",
                FirstName = addEmployee.FirstName,
                LastName = addEmployee.LastName,
                MiddleName = addEmployee.MiddleName,
                BirthDate = addEmployee.BirthDate,
                Age = age,
                Sex = addEmployee.Sex,
                Address = addEmployee.Address,
                EmployedDate = addEmployee.EmployedDate,
                Email = addEmployee.Email
            };
            _inventoryDbContext.Employee.Add(newEmployee);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(_functions.MaptoEmployeeDetails(newEmployee));
        }

        public async Task<IEnumerable<EmployeeDetails<string>>> AddEmployees(AddEmployees[] employees)
        {
            var listOfEmployees = new List<EmployeeDetails<string>>();
            foreach(var employee in employees)
            {
                var newEmployee = new Employee
                {
                    Id = Guid.Parse(employee.Id),
                    EmployeeId = employee.EmployeeId,
                    UserType = "User",
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Image = employee.Image,
                };
                listOfEmployees.Add(_functions.MaptoEmployeeDetails(newEmployee));
                _inventoryDbContext.Employee.Add(newEmployee);
                _inventoryDbContext.SaveChanges();
            }
            return await Task.FromResult(listOfEmployees);
        }

        public async Task DeleteEmployee(string id)
        {
            var emp = _inventoryDbContext.Employee.Where(employee => employee.EmployeeId == id).SingleOrDefault();
            emp.IsDeleted = 1;
            await Task.FromResult(_inventoryDbContext.SaveChanges());

        }
        public async Task<PagedData> GetEmployees(Paging paging)
        {
            var employees = _inventoryDbContext.Employee.Where(emp => emp.IsDeleted == 0).Select(_functions.MaptoEmployeeDetails);
            if (_functions.ParameterNullChecker(paging))
            {
                return await Task.FromResult(_functions.MapToPagedData(PagedList<EmployeeDetails<string>>.ToPagedList(employees, 1, employees.Count())));
            }

            return await Task.FromResult(_functions.MapToPagedData(PagedList<EmployeeDetails<string>>.ToPagedList(employees, paging.Page.Value, paging.Limit.Value)));
        }

        public async Task<EmployeeDetails<string>> GetEmployeeById(string id)
        {
            var getEmployees = _inventoryDbContext.Employee.Where(emp => emp.IsDeleted == 0 && (emp.EmployeeId == id || $"{emp.Id}" == id)).FirstOrDefault();

            if (getEmployees == null) return null;

            return await Task.FromResult(_functions.MaptoEmployeeDetails(getEmployees));
        }

        public async Task<Employee> UpdateEmployee(string employeeId, EmployeeDetails<IFormFile> employee)
        {
            var emp = _inventoryDbContext.Employee.Where(emp => $"{emp.Id}" == employeeId && emp.IsDeleted == 0).FirstOrDefault();

            if (emp == null) return null;
            
            _functions.UpdateDetails(employeeId, emp, employee);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(emp);
            
        }
    }
}

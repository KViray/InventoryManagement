using InventoryManagement.Context;
using InventoryManagement.Features.Salaries.Model;
using InventoryManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Salaries.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly Functions _functions;
        public SalaryService(InventoryDbContext inventoryDbContext,Functions functions)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
        }
        public async Task<SalaryResource> AddSalary(string employeeId, SalaryBinding salaryResource)
        {
            var employee = _inventoryDbContext.Employee.Where(emp => $"{emp.Id}" == employeeId).FirstOrDefault();
            var dailyRate = salaryResource.BasicPay / 11;
            var hourlyRate = dailyRate / 8;
            decimal totalOvertime = 0;

            if (employee == null) return null;

            foreach (var overtime in salaryResource.OvertimePay)
            {
                var total = hourlyRate * (overtime.Rate / 100) * overtime.HoursOfOvertime;
                totalOvertime += total;
            }
            var salary = new Salary
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                BasicPay = salaryResource.BasicPay,
                HolidayPay = dailyRate * (salaryResource.TotalDaysPresentDuringHoliday + ((decimal)0.3 * salaryResource.TotalDaysPresentDuringHoliday)),
                ThirteenthMonthPay = salaryResource.TotalMonthsintheCompanyInWholeYear == 0 ? 0 : salaryResource.BasicPay * (salaryResource.TotalMonthsintheCompanyInWholeYear / 12),
                Absences = dailyRate * -salaryResource.TotalDaysOfAbsences,
                UnderTime = hourlyRate * -salaryResource.TotalHoursOfUndertime,
                OvertimePay = totalOvertime,
                Allowance = salaryResource.Allowance,
                SSS = salaryResource.SSS,
                PhilHealth = salaryResource.PhilHealth,
                HDMF = salaryResource.HDMF,
                WithholdingTax = salaryResource.WithholdingTax,
                Others = salaryResource.Others
            };
            salary.TotalSalary = CalculateSalary(MapToSalaryResource(salary));
            _inventoryDbContext.Salary.Add(salary);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(MapToSalaryResource(salary));

        }
        public async Task<SalaryResource> GetSalaryDetails(string employeeId)
        {
            var employeeSalary = _inventoryDbContext.Salary.Where(empSalary => empSalary.EmployeeId == employeeId).FirstOrDefault();
            if (employeeSalary == null) return null;

            return await Task.FromResult(MapToSalaryResource(employeeSalary));
        }
        private decimal CalculateSalary(SalaryResource employeeSalary)
        {
            decimal totalSalary = 0;
            var properties = employeeSalary.GetType().GetProperties().Where(prop => !"TotalSalary".Equals(prop.Name)).ToArray();
            for (var x = 0; x < 7; x++)
            {
                totalSalary += (decimal)properties[x].GetValue(employeeSalary);
            }
            for (var x = 7; x < properties.Length; x++)
            {
                totalSalary -= (decimal)properties[x].GetValue(employeeSalary);
            }
            return totalSalary;
        }

        public async Task<SalaryResource> UpdateSalary(string employeeId, SalaryResource salaryResource)
        {
            var employeeSalary = _inventoryDbContext.Salary.Where(empSalary => empSalary.EmployeeId == employeeId).FirstOrDefault();
            if (employeeSalary == null) return null;

            _functions.UpdateDetails(employeeId, employeeSalary, salaryResource);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(MapToSalaryResource(employeeSalary));
        }
        private SalaryResource MapToSalaryResource(Salary salary)
        {
            return new SalaryResource
            {
                BasicPay = salary.BasicPay,
                HolidayPay = salary.HolidayPay,
                ThirteenthMonthPay = salary.ThirteenthMonthPay,
                Absences = salary.Absences,
                UnderTime = salary.UnderTime,
                OvertimePay = salary.OvertimePay,
                Allowance = salary.Allowance,
                SSS = salary.SSS,
                PhilHealth = salary.PhilHealth,
                HDMF = salary.HDMF,
                WithholdingTax = salary.WithholdingTax,
                Others = salary.Others,
                TotalSalary = salary.TotalSalary
            };
        }

        
    }
}

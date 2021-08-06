using InventoryManagement.Features.Salaries.Model;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Salaries.Services
{
    public interface ISalaryService
    {
        Task<SalaryResource> AddSalary(string employeeId, SalaryBinding salaryResource);
        Task<SalaryResource> GetSalaryDetails(string employeeId);
        Task<SalaryResource> UpdateSalary(string employeeId, SalaryResource salaryResource);
    }
}

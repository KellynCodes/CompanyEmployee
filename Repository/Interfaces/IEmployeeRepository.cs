using Entities.Models;
using Shared.RequestFeatures;

namespace Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);

        Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
        Task CreateEmployeeForCompanyAsync(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}

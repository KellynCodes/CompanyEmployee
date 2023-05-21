using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Extensions;
using Repository.Interfaces;
using Shared.RequestFeatures;

namespace Repository.Implementations
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompEmpDbContext repositoryContext)
        : base(repositoryContext)
        {
        }
        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            List<Employee> employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
            .Search(employeeParameters.SearchTerm)
            .OrderBy(e => e.Name)
            .Sort(employeeParameters.OrderBy)
            .ToListAsync();
            return PagedList<Employee>.ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefault();
        }

        public async Task CreateEmployeeForCompanyAsync(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
           await CreateAsync(employee);
        }


        public void DeleteEmployee(Employee employee) => Delete(employee);
    }

}

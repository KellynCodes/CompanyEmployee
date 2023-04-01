using Entities.Models;
using Repository.Context;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompEmpDbContext repositoryContext)
        : base(repositoryContext)
        {
        }
        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
 FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
 .OrderBy(e => e.Name).ToList();

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges) =>
 FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id),
trackChanges)
 .SingleOrDefault();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }


        public void DeleteEmployee(Employee employee) => Delete(employee);
    }

}

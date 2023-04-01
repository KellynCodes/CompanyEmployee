using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Interfaces;

namespace Repository.Implementations
{
    internal sealed class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(CompEmpDbContext repositoryContext)
        : base(repositoryContext)
        {
        }
        public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToList();

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
FindByCondition(x => ids.Contains(x.Id), trackChanges)
.ToList();

        public Company GetCompany(Guid companyId, bool trackChanges) => FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefault();
        public IEnumerable<Company> GetCompany(bool trackChanges)
        {
            return GetAllAsync(trackChanges).Include(c => c.Employees)
             .Select(c => new Company
             {
                 Id = c.Id,
                 Name = c.Name,
                 Address = c.Address,
                 Country = c.Country,
                 Employees = c.Employees.Select(emp => new Employee
                 {
                     Id = emp.Id,
                     CompanyId = c.Id,
                     Name = emp.Name,
                     Age = emp.Age,
                     Position = emp.Position,
                 })
             }).ToList();
        }

        public void CreateCompany(Company company) => Create(company);

    }
}

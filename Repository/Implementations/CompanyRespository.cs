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
        { }
            public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
        {
             return  await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges)
        {
          return await FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
         return await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetCompanyAsync(bool trackChanges)
        {
            return GetAll(trackChanges).Include(c => c.Employees)
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

        public async Task CreateCompanyAsync(Company company) => await CreateAsync(company);
        public void DeleteCompany(Company company) => Delete(company);


    }
}

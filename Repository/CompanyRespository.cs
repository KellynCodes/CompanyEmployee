using Contracts;
using Entities.Models;
using Repository.Context;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(CompEmpDbContext repositoryContext)
        : base(repositoryContext)
        {
        }
    }

}

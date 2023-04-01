using Entities.Models;

namespace Repository.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company GetCompany(Guid companyId, bool trackChanges);
        IEnumerable<Company> GetCompany(bool trackChanges);
        void CreateCompany(Company company);
        IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    }
}

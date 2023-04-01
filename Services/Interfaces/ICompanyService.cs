using Entities.Models;
using Repository.Dtos;

namespace Service.Interfaces
{
    public interface ICompanyService
    {
        CompanyDto GetCompany(Guid companyId, bool trackChanges);
        IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
        IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
        IEnumerable<Company> GetCompanyWithEmployee(bool trackChanges);
        CompanyDto CreateCompany(CompanyForCreationDto company);
        CompanyDto CreateCompanyWithEmployee(CreateCompanyWithEmployeesDto company);
    }
}

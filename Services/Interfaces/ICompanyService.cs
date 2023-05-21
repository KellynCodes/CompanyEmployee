using Entities.Models;
using Repository.Dtos;

namespace Service.Interfaces
{
    public interface ICompanyService
     {
         Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);
         Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
         Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
         Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);
         Task<IEnumerable<Company>> GetCompanyWithEmployeeAsync(bool trackChanges);
         Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
         Task<CompanyDto> CreateCompanyWithEmployeeAsync(CreateCompanyWithEmployeesDto company);
         Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
         Task UpdateCompanyAsync(Guid companyid, CompanyForUpdateDto companyForUpdate, bool trackChanges);
     }

}

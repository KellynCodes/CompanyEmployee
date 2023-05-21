using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repository.Dtos;

namespace Services.Interfaces
{
    public interface IEmployeeLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto,
        string fields, Guid companyId, HttpContext httpContext);
    }
}

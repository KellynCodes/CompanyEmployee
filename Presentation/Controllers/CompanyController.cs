using Entities.ErrorModel;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilter;
using Presentation.ModelBinders;
using Repository.Dtos;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/companies")]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service) => _service = service;

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");
            return Ok();
        }

        [HttpGet(Name = "GetCompanies")]
        [Authorize(Roles = "Manager")]
        [SwaggerOperation(Summary = "Fetch all companies")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Company fetched.", Type = typeof(CompanyDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "No company found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch companies", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async  Task<IActionResult> GetCompanies()
        {
            IEnumerable<CompanyDto> companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get single company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company fetched", Type = typeof(CompanyDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company with provided id does not exist", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            CompanyDto company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
            return Ok(company);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all company with thier employees")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company and employees fetched", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Sorry this details provided does'nt match any information in our system.", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            IEnumerable<CompanyDto> companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpPost("collection", Name = "CreateCompanyCollection")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create collection of your company.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "companies creaed successfully", Type = typeof(CompanyDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Sorry an error occured.", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Creation failed.", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var (companies, ids) = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { ids },
            companies);
        }


        [HttpGet("Get-all-company-with-their-employees", Name = "CompanyAndEmployees")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get single company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company fetched", Type = typeof(CompanyDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company with provided id does not exist", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetCompanyAndEmployee()
        {
            IEnumerable<Company> company = await _service.CompanyService.GetCompanyWithEmployeeAsync(trackChanges: false);
            return Ok(company);
        }

        [HttpPost("Create-company-with-employee", Name = "Create company with employees.")]
        [SwaggerOperation(Summary = "Create Company with employe")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company Created", Type = typeof(CompanyForCreationDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company creation failed", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CreateCompanyWithEmployees([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");
            CompanyDto createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
        }


        [HttpPost("create-company", Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation(Summary = "Create Company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company Created", Type = typeof(CompanyDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company creation failed", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create user", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            CompanyDto createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpDelete("Delete-Company", Name = "Delete company by id.")]
        [SwaggerOperation(Summary = "Delete company by id.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Company Deleted", Type = typeof(CompanyDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Deleting of Company failed", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to delete", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
           await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation(Summary = "Update company by id.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Company details updated", Type = typeof(CompanyDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company update failed", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to delete", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();
        }
    }
}
using Entities.ErrorModel;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ModelBinders;
using Repository.Dtos;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service) => _service = service;

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Fetch all companies")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Company fetched.", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "No company found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch companies", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult GetCompanies()
        {
            var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get single company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company fetched", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company with provided id does not exist", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult GetCompany(Guid id)
        {
            var company = _service.CompanyService.GetCompany(id, trackChanges: false);
            return Ok(company);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all company with thier employees")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company and employees fetched", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Sorry this details provided does'nt match any information in our system.", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpPost("collection", Name = "CreateCompanyCollection")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create collection of your company.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "companies creaed successfully", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Sorry an error occured.", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Creation failed.", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var (companies, ids) = _service.CompanyService.CreateCompanyCollection(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { ids },
            companies);
        }


        [HttpGet("Get-all-company-with-their-employees", Name = "CompanyAndEmployees")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get single company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company fetched", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company with provided id does not exist", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult GetCompanyAndEmployee()
        {
            var company = _service.CompanyService.GetCompanyWithEmployee(trackChanges: false);
            return Ok(company);
        }

        [HttpPost("create-company",Name = "Create Company")]
        [SwaggerOperation(Summary = "Create Company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company Created", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company creation failed", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create user", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult CreateCompanyWithEmployees([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");
            var createdCompany = _service.CompanyService.CreateCompany(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
        }


        [HttpPost("Create-company-with-employee", Name = "Create company with employees.")]
        [SwaggerOperation(Summary = "Create Company with employe")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "company Created", Type = typeof(Company))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Company creation failed", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
      
        public IActionResult CreateCompany([FromBody] CreateCompanyWithEmployeesDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");
            var createdCompany = _service.CompanyService.CreateCompanyWithEmployee(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
        }
    }
}
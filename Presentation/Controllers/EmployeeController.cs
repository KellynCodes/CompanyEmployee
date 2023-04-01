using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Entities.ErrorModel;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Dtos;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{

    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IServiceManager _service;
        public EmployeeController(IServiceManager service) => _service = service;

        [HttpGet]
        [SwaggerOperation(Summary = "Get Employees of company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Empoyees fetched successfully.", Type = typeof(Employee))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Employe not found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch employee details", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get single Employee of company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Empoyee fetched successfully.", Type = typeof(Employee))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Employe not found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch employee details", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create Employee for a company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Empoyees created successfully.", Type = typeof(Employee))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Employe not found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create employee details", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForCreationDto object is null");
            var employeeToReturn =
            _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id =  employeeToReturn.Id }, employeeToReturn);
        }

    }
}


using Entities.ErrorModel;
using Entities.LinkModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilter;
using Repository.Dtos;
using Entities.Models;
using Service.Interfaces;
using Shared.RequestFeatures;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace Presentation.Controllers
{

    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IServiceManager _service;
        public EmployeeController(IServiceManager service) => _service = service;

        [HttpGet]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [SwaggerOperation(Summary = "Get Employees of company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Empoyees fetched successfully.", Type = typeof(EmployeeDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Employe not found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch employee details", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,[FromQuery] EmployeeParameters employeeParameters)
        {
            var linkParams = new LinkParameters(employeeParameters, HttpContext);
            var result = await _service.EmployeeService.GetEmployeesAsync(companyId,
            linkParams, trackChanges: false);
            Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(result.metaData));

              return result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);
        }


        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get single Employee of company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Empoyee fetched successfully.", Type = typeof(EmployeeDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Employe not found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to fetch employee details", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            EmployeeDto employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation(Summary = "Create Employee for a company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Empoyees created successfully.", Type = typeof(EmployeeForCreationDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Employe not found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create employee details", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            EmployeeDto employeeToReturn = await _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);
            return CreatedAtRoute("GetEmployeeForCompany", new
            {
                companyId,
                id = employeeToReturn.Id
            },
            employeeToReturn);
        }


        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation(Summary = "Update Employee for a company")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Empoyees updated successfully.", Type = typeof(EmployeeForUpdateDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Employe not found", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to update employee details", Type = typeof(ErrorDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorDetails))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
           await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee, compTrackChanges: false, empTrackChanges: true);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");
            (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, compTrackChanges: false, empTrackChanges: true);
            patchDoc.ApplyTo(result.employeeToPatch);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            TryValidateModel(result.employeeToPatch);

            await _service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
            return NoContent();
        }


    }
}


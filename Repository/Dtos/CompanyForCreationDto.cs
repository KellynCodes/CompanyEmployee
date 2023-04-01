namespace Repository.Dtos
{
    public record CompanyForCreationDto(string Name, string Address, string Country);
    public record CreateCompanyWithEmployeesDto(string Name, string Address, string Country, IEnumerable<EmployeeForCreationDto> Employees);
}

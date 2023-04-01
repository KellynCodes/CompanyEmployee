namespace Repository.Dtos
{
    [Serializable]
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    }

    public class CompanyClassDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
        public EmployeeDto EmployeeDto { get; init; }
    }
}

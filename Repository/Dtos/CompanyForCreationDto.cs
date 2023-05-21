using System.Runtime.Serialization;

namespace Repository.Dtos
{
    [Serializable]
    [DataContract]
    public class CompanyForCreationDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
    }
    public class CreateCompanyWithEmployeesDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}

using AutoMapper;
using Entities.Models;
using Repository.Dtos;

namespace CompanyEmployee.MapProfile
{
    public class ProfileMapper : Profile
    {
            public ProfileMapper()
            {
                CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Company, Company>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<CreateCompanyWithEmployeesDto, Company>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeForCreationDto, Employee>();

            //CreateMap<UserForRegistrationDto, User>();

        }
    }
}

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
            .ForCtorParam("FullAddress",
            opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
        }
    }
}

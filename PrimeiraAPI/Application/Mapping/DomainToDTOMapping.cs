using AutoMapper;
using PrimeiraAPI.Domain.DTOs;
using PrimeiraAPI.Domain.Model.EmployeeAggregate;

namespace PrimeiraAPI.Application.Mapping
{
    public class DomainToDTOMapping : Profile
    {
        public DomainToDTOMapping()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.EmployeeName, m => m.MapFrom(orig => orig.name))
                .ForMember(dest => dest.EmployeeAge, m => m.MapFrom(orig => orig.age));
        }
    }
}

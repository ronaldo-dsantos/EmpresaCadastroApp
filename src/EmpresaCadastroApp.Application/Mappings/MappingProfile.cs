using AutoMapper;
using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.Models;
using EmpresaCadastroApp.Domain.Entities;

namespace EmpresaCadastroApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyResponseDto>();
            
            CreateMap<ReceitaWsResponse, Company>()
                .ForMember(dest => dest.AtividadePrincipal,
                    opt => opt.MapFrom(src => src.AtividadePrincipal != null && src.AtividadePrincipal.Any()
                        ? src.AtividadePrincipal.First().Text
                        : null));
        }
    }
}

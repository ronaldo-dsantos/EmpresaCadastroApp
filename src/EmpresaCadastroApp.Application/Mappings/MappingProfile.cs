using AutoMapper;
using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.DTOs.User;
using EmpresaCadastroApp.Application.Models;
using EmpresaCadastroApp.Domain.Entities;

namespace EmpresaCadastroApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<Company, CompanyResponseDto>();

            CreateMap<ReceitaWsResponse, Company>()
                .ForMember(dest => dest.AtividadePrincipal,
                    opt => opt.MapFrom(src => src.AtividadePrincipal != null && src.AtividadePrincipal.Any()
                        ? src.AtividadePrincipal.First().Text
                        : null));
        }
    }
}

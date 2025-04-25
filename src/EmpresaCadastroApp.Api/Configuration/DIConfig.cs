using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.DTOs.User;
using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Services;
using EmpresaCadastroApp.Application.Validators;
using EmpresaCadastroApp.Domain.Interfaces;
using EmpresaCadastroApp.Infrastructure.Repositories;
using EmpresaCadastroApp.Infrastructure.Services;
using FluentValidation;

namespace EmpresaCadastroApp.Api.Configuration
{
    public static class DIConfig
    {
        public static WebApplicationBuilder AddDIConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();

            builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterDtoValidator>();
            builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
            builder.Services.AddScoped<IValidator<CompanyCreateDto>, CompanyCreateDtoValidator>();

            return builder;
        }
    }
}

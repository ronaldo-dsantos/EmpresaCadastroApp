using AutoMapper;
using EmpresaCadastroApp.Application.DTOs.User;
using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Utils;
using EmpresaCadastroApp.Application.Validators;
using EmpresaCadastroApp.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpresaCadastroApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IValidator<UserRegisterDto> _registerValidator;
        private readonly IValidator<UserLoginDto> _loginValidator;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager,
                           IConfiguration configuration,
                           IValidator<UserRegisterDto> registerValidator,
                           IValidator<UserLoginDto> loginValidator,
                           IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _mapper = mapper;
        }

        public async Task<Result<UserResponseDto>> RegisterAsync(UserRegisterDto dto)
        {
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)            
                return Result<UserResponseDto>.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            

            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return Result<UserResponseDto>.Fail("E-mail já cadastrado.");

            var user = _mapper.Map<User>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return Result<UserResponseDto>.Fail(result.Errors.Select(e => e.Description).ToArray());

            var response = _mapper.Map<UserResponseDto>(user);
            response.Token = GenerateJwtToken(user);

            return Result<UserResponseDto>.Ok(response);
        }

        public async Task<Result<UserResponseDto>> LoginAsync(UserLoginDto dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return Result<UserResponseDto>.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());


            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Result<UserResponseDto>.Fail("E-mail ou senha inválidos.");

            var response = _mapper.Map<UserResponseDto>(user);
            response.Token = GenerateJwtToken(user);

            return Result<UserResponseDto>.Ok(response);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

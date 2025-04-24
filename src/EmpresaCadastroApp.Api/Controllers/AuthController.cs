using EmpresaCadastroApp.Application.DTOs.User;
using EmpresaCadastroApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaCadastroApp.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return FromResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login( UserLoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return FromResult(result);
        }
    }
}

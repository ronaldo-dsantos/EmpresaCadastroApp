using EmpresaCadastroApp.Application.DTOs.User;

namespace EmpresaCadastroApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(UserRegisterDto dto);
        Task<UserResponseDto> LoginAsync(UserLoginDto dto);
    }
}

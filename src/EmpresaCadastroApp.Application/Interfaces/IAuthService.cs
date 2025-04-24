using EmpresaCadastroApp.Application.DTOs.User;
using EmpresaCadastroApp.Application.Utils;

namespace EmpresaCadastroApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserResponseDto>> RegisterAsync(UserRegisterDto dto);
        Task<Result<UserResponseDto>> LoginAsync(UserLoginDto dto);
    }
}

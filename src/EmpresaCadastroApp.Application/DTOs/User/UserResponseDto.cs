namespace EmpresaCadastroApp.Application.DTOs.User
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
    }
}

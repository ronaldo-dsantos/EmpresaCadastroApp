using Microsoft.AspNetCore.Identity;

namespace EmpresaCadastroApp.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? Name { get; set; }

        // Navegação
        public ICollection<Company>? Companies { get; set; }
    }
}

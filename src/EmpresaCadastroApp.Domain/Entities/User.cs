using Microsoft.AspNetCore.Identity;

namespace EmpresaCadastroApp.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? Nome { get; set; }

        // Navegação
        public ICollection<Company>? Companies { get; set; }
    }
}

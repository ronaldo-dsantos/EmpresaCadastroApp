using EmpresaCadastroApp.Domain.Entities;

namespace EmpresaCadastroApp.Application.Interfaces
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company);
        Task<IEnumerable<Company>> GetByUserIdAsync(Guid userId);
        Task<Company?> GetByCnpjAndUserIdAsync(string cnpj, Guid userId);
    }
}

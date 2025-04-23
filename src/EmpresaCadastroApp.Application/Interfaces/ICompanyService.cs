using EmpresaCadastroApp.Application.DTOs.Company;

namespace EmpresaCadastroApp.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto> CreateCompanyAsync(string cnpj, Guid userId);
        Task<IEnumerable<CompanyResponseDto>> GetCompaniesByUserAsync(Guid userId);
    }
}

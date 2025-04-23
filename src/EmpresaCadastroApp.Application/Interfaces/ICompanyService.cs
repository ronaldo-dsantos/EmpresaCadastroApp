using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.Utils;

namespace EmpresaCadastroApp.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<Result<CompanyResponseDto>> CreateCompanyAsync(string cnpj, Guid userId);
        Task<Result<IEnumerable<CompanyResponseDto>>> GetCompaniesByUserAsync(Guid userId);
    }
}

using EmpresaCadastroApp.Application.Models;
using EmpresaCadastroApp.Application.Utils;

namespace EmpresaCadastroApp.Application.Interfaces
{
    public interface IReceitaWsService
    {
        Task<Result<ReceitaWsResponse>> ConsultarCnpjAsync(string cnpj);
    }
}

using EmpresaCadastroApp.Application.Models;

namespace EmpresaCadastroApp.Application.Interfaces
{
    public interface IReceitaWsService
    {
        Task<ReceitaWsResponse?> ConsultarCnpjAsync(string cnpj);
    }
}

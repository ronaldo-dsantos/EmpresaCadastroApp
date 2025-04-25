using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Models;
using EmpresaCadastroApp.Application.Utils;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EmpresaCadastroApp.Infrastructure.Services
{
    public class ReceitaWsService : IReceitaWsService
    {
        private readonly HttpClient _httpClient;

        public ReceitaWsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<ReceitaWsResponse>> ConsultarCnpjAsync(string cnpj)
        {
            try
            {
                cnpj = Regex.Replace(cnpj, "[^0-9]", "");

                // Consulta o CNPJ na API externa ReceitaWS
                var response = await _httpClient.GetAsync($"cnpj/{cnpj}");
                if (!response.IsSuccessStatusCode)
                {
                    return Result<ReceitaWsResponse>.Fail("Erro ao consultar o CNPJ. Tente novamente mais tarde.");
                }

                // Lê o conteúdo do response
                var json = await response.Content.ReadAsStringAsync();

                // Deserializa o JSON para o objeto ReceitaWsResponse
                var data = JsonSerializer.Deserialize<ReceitaWsResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Verifica se o objeto deserializado é nulo
                if (data == null)
                    return Result<ReceitaWsResponse>.Fail("Resposta inválida da ReceitaWS.");

                // Verifica se o objeto deserializado contém erro
                if (!string.IsNullOrEmpty(data.Status) && data.Status.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
                    return Result<ReceitaWsResponse>.Fail(data.Message ?? "CNPJ inválido ou não encontrado.");

                return Result<ReceitaWsResponse>.Ok(data);
            }
            catch (HttpRequestException)
            {
                return Result<ReceitaWsResponse>.Fail("Falha de comunicação com a ReceitaWS.");
            }
            catch (Exception)
            {
                return Result<ReceitaWsResponse>.Fail("Erro inesperado ao consultar a ReceitaWS.");
            }
        }
    }
}

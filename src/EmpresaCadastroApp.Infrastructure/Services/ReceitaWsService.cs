using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Models;
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

        public async Task<ReceitaWsResponse?> ConsultarCnpjAsync(string cnpj)
        {
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            var response = await _httpClient.GetAsync($"cnpj/{cnpj}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ReceitaWsResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}

using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Models;
using EmpresaCadastroApp.Domain.Entities;
using EmpresaCadastroApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EmpresaCadastroApp.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public CompanyService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }
        public async Task<CompanyResponseDto> CreateCompanyAsync(string cnpj, Guid userId)
        {
            // Limpar o CNPJ (caso venha com pontuação)
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            // Fazer requisição à API ReceitaWS
            var response = await _httpClient.GetAsync($"https://www.receitaws.com.br/v1/cnpj/{cnpj}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao consultar CNPJ na ReceitaWS.");

            var json = await response.Content.ReadAsStringAsync();
            var receitaData = JsonSerializer.Deserialize<ReceitaWsResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (receitaData == null || receitaData.Cnpj == null)
                throw new Exception("CNPJ inválido ou não encontrado.");

            // Mapear para entidade Company
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Cnpj = receitaData.Cnpj,
                NomeEmpresarial = receitaData.NomeEmpresarial,
                NomeFantasia = receitaData.NomeFantasia,
                Situacao = receitaData.Situacao,
                Abertura = receitaData.Abertura,
                Tipo = receitaData.Tipo,
                NaturezaJuridica = receitaData.NaturezaJuridica,
                AtividadePrincipal = receitaData.AtividadePrincipal?.FirstOrDefault()?.Text,
                Logradouro = receitaData.Logradouro,
                Numero = receitaData.Numero,
                Complemento = receitaData.Complemento,
                Bairro = receitaData.Bairro,
                Municipio = receitaData.Municipio,
                Uf = receitaData.Uf,
                Cep = receitaData.Cep,
                UserId = userId
            };

            // Salvar no banco
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            // Mapear para DTO de resposta
            return new CompanyResponseDto
            {
                Id = company.Id,
                NomeEmpresarial = company.NomeEmpresarial,
                NomeFantasia = company.NomeFantasia,
                Cnpj = company.Cnpj,
                Situacao = company.Situacao,
                Abertura = company.Abertura,
                Tipo = company.Tipo,
                NaturezaJuridica = company.NaturezaJuridica,
                AtividadePrincipal = company.AtividadePrincipal,
                Logradouro = company.Logradouro,
                Numero = company.Numero,
                Complemento = company.Complemento,
                Bairro = company.Bairro,
                Municipio = company.Municipio,
                Uf = company.Uf,
                Cep = company.Cep
            };
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetCompaniesByUserAsync(Guid userId)
        {
            return await _context.Companies
                .Where(c => c.UserId == userId)
                .Select(c => new CompanyResponseDto
                {
                    Id = c.Id,
                    NomeEmpresarial = c.NomeEmpresarial,
                    NomeFantasia = c.NomeFantasia,
                    Cnpj = c.Cnpj,
                    Situacao = c.Situacao,
                    Abertura = c.Abertura,
                    Tipo = c.Tipo,
                    NaturezaJuridica = c.NaturezaJuridica,
                    AtividadePrincipal = c.AtividadePrincipal,
                    Logradouro = c.Logradouro,
                    Numero = c.Numero,
                    Complemento = c.Complemento,
                    Bairro = c.Bairro,
                    Municipio = c.Municipio,
                    Uf = c.Uf,
                    Cep = c.Cep
                })
                .ToListAsync();
        }
    }
}

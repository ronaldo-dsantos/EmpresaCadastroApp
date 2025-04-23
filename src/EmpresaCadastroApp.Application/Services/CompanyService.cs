using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Models;
using EmpresaCadastroApp.Domain.Entities;
using EmpresaCadastroApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

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
            var response = await _httpClient.GetAsync($"https://www.receitaws.com.br/v1/cnpj/{cnpj}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao consultar a ReceitaWS.");

            var data = await response.Content.ReadFromJsonAsync<ReceitaWsResponse>();

            if (data == null || data.Status?.ToLower() == "error")
                throw new Exception("CNPJ inválido ou não encontrado.");

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Cnpj = data.Cnpj,
                NomeEmpresarial = data.Nome,
                NomeFantasia = data.Fantasia,
                Situacao = data.Situacao,
                Abertura = data.Abertura,
                Tipo = data.Tipo,
                NaturezaJuridica = data.NaturezaJuridica,
                AtividadePrincipal = data.AtividadePrincipal?.FirstOrDefault()?.Text,
                Logradouro = data.Logradouro,
                Numero = data.Numero,
                Complemento = data.Complemento,
                Bairro = data.Bairro,
                Municipio = data.Municipio,
                Uf = data.Uf,
                Cep = data.Cep,
                UserId = userId
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return new CompanyResponseDto
            {
                Cnpj = company.Cnpj,
                NomeEmpresarial = company.NomeEmpresarial,
                NomeFantasia = company.NomeFantasia,
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
                    Cnpj = c.Cnpj,
                    NomeEmpresarial = c.NomeEmpresarial,
                    NomeFantasia = c.NomeFantasia,
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

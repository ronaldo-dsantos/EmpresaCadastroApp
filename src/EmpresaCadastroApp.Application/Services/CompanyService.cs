using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Models;
using EmpresaCadastroApp.Application.Utils;
using EmpresaCadastroApp.Domain.Entities;
using EmpresaCadastroApp.Domain.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EmpresaCadastroApp.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(HttpClient httpClient, ICompanyRepository companyRepository)
        {
            _httpClient = httpClient;
            _companyRepository = companyRepository;
        }
        public async Task<Result<CompanyResponseDto>> CreateCompanyAsync(string cnpj, Guid userId)
        {
            try
            {
                cnpj = Regex.Replace(cnpj, "[^0-9]", "");

                var response = await _httpClient.GetAsync($"cnpj/{cnpj}");

                if (!response.IsSuccessStatusCode)
                    return Result<CompanyResponseDto>.Fail("Erro ao consultar a ReceitaWS");

                var json = await response.Content.ReadAsStringAsync();

                var receitaData = JsonSerializer.Deserialize<ReceitaWsResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (receitaData == null || receitaData.NomeEmpresarial == null)
                    return Result<CompanyResponseDto>.Fail("Dados da ReceitaWS inválidos.");

                var existing = await _companyRepository.GetByCnpjAndUserIdAsync(receitaData.Cnpj, userId);
                if (existing != null)
                    return Result<CompanyResponseDto>.Fail("Esta empresa já está cadastrada por este usuário.");

                var company = new Company
                {
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

                await _companyRepository.AddAsync(company);

                var responseDto = new CompanyResponseDto
                {
                    Id = company.Id,
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

                return Result<CompanyResponseDto>.Ok(responseDto);
            }
            catch (Exception ex)
            {
                return Result<CompanyResponseDto>.Fail("Erro interno: " + ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CompanyResponseDto>>> GetCompaniesByUserAsync(Guid userId)
        {
            try
            {
                var companies = await _companyRepository.GetByUserIdAsync(userId);

                if (companies == null || !companies.Any())
                    return Result<IEnumerable<CompanyResponseDto>>.Ok(new List<CompanyResponseDto>());

                var responseDto = companies.Select(company => new CompanyResponseDto
                {
                    Id = company.Id,
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
                }).ToList();

                return Result<IEnumerable<CompanyResponseDto>>.Ok(responseDto);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CompanyResponseDto>>.Fail("Erro ao buscar empresas: " + ex.Message);
            }
        }
    }
}

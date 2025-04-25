using AutoMapper;
using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Application.Utils;
using EmpresaCadastroApp.Domain.Entities;
using EmpresaCadastroApp.Domain.Interfaces;
using FluentValidation;

namespace EmpresaCadastroApp.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IReceitaWsService _receitaWsService;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CompanyCreateDto> _validator;

        public CompanyService(IReceitaWsService receitaWsService, 
                              ICompanyRepository companyRepository, 
                              IMapper mapper,
                              IValidator<CompanyCreateDto> validator)
        {
            _receitaWsService = receitaWsService;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<Result<CompanyResponseDto>> CreateCompanyAsync(CompanyCreateDto dto, Guid userId)
        {
            try
            {
                // Valida os dados de entrada
                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)                
                    return Result<CompanyResponseDto>.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());

                // Consulta o CNPJ informado na API externa ReceitaWS
                var receitaResult = await _receitaWsService.ConsultarCnpjAsync(dto.Cnpj);
                if (!receitaResult.Success || receitaResult.Data == null || string.IsNullOrWhiteSpace(receitaResult.Data.NomeEmpresarial))
                    return Result<CompanyResponseDto>.Fail(receitaResult.Errors.FirstOrDefault() ?? "Dados inválidos.");

                // Verifica se o CNPJ já está cadastrada por este usuário
                var existing = await _companyRepository.GetByCnpjAndUserIdAsync(receitaResult.Data.Cnpj, userId);
                if (existing != null)
                    return Result<CompanyResponseDto>.Fail("Esta empresa já está cadastrada por este usuário.");

                var company = _mapper.Map<Company>(receitaResult.Data);
                company.UserId = userId;

                await _companyRepository.AddAsync(company);

                var responseDto = _mapper.Map<CompanyResponseDto>(company);

                return Result<CompanyResponseDto>.Ok(responseDto);
            }
            catch (Exception ex)
            {
                return Result<CompanyResponseDto>.Fail("Erro interno ao cadastrar empresa.");
            }
        }

        public async Task<Result<IEnumerable<CompanyResponseDto>>> GetCompaniesByUserAsync(Guid userId)
        {
            try
            {
                var companies = await _companyRepository.GetByUserIdAsync(userId);

                if (companies == null || !companies.Any())
                    return Result<IEnumerable<CompanyResponseDto>>.Ok(new List<CompanyResponseDto>());

                var responseDto = _mapper.Map<IEnumerable<CompanyResponseDto>>(companies);

                return Result<IEnumerable<CompanyResponseDto>>.Ok(responseDto);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CompanyResponseDto>>.Fail("Erro ao buscar empresas.");
            }
        }
    }
}

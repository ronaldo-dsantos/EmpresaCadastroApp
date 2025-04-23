using EmpresaCadastroApp.Application.DTOs.Company;
using EmpresaCadastroApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmpresaCadastroApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/empresas")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var result = await _companyService.CreateCompanyAsync(dto.Cnpj, Guid.Parse(userId));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var companies = await _companyService.GetCompaniesByUserAsync(Guid.Parse(userId));
            return Ok(companies);
        }
    }
}

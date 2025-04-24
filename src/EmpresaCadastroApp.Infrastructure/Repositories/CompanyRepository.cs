using EmpresaCadastroApp.Domain.Entities;
using EmpresaCadastroApp.Infrastructure.Data;
using EmpresaCadastroApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmpresaCadastroApp.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Companies
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Company?> GetByCnpjAndUserIdAsync(string cnpj, Guid userId)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.Cnpj == cnpj && c.UserId == userId);
        }
    }
}

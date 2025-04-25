using EmpresaCadastroApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmpresaCadastroApp.Infrastructure.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(c => c.NomeEmpresarial)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(c => c.NomeFantasia)
                   .HasMaxLength(256);

            builder.Property(c => c.Cnpj)
                   .HasMaxLength(18)
                   .IsRequired();

            builder.Property(c => c.Situacao)
                   .HasMaxLength(100);

            builder.Property(c => c.Abertura)
                   .HasMaxLength(20);

            builder.Property(c => c.Tipo)
                   .HasMaxLength(100);

            builder.Property(c => c.NaturezaJuridica)
                   .HasMaxLength(200);

            builder.Property(c => c.AtividadePrincipal)
                   .HasMaxLength(500);

            builder.Property(c => c.Logradouro)
                   .HasMaxLength(200);

            builder.Property(c => c.Numero)
                   .HasMaxLength(20);

            builder.Property(c => c.Complemento)
                   .HasMaxLength(100);

            builder.Property(c => c.Bairro)
                   .HasMaxLength(100);

            builder.Property(c => c.Municipio)
                   .HasMaxLength(100);

            builder.Property(c => c.Uf)
                   .HasMaxLength(2);

            builder.Property(c => c.Cep)
                   .HasMaxLength(10);
        }
    }
}

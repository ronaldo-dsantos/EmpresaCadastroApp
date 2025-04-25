using EmpresaCadastroApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmpresaCadastroApp.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                   .HasMaxLength(256)
                   .IsRequired();

            // Relacionamento User -> Companies
            builder.HasMany(u => u.Companies)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId);
        }
    }
}

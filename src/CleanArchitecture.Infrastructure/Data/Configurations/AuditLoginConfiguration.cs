using CleanArchitecture.Core.Domain.Entities.AuditLogin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class AuditLoginConfiguration : IEntityTypeConfiguration<AuditLogin>
    {
        public void Configure(EntityTypeBuilder<AuditLogin> builder)
        {
            builder.ToTable("AuditLogins");
        }
    }
}

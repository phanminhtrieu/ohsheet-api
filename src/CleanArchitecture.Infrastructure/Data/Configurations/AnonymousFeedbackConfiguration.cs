using CleanArchitecture.Core.Domain.Entities.AnonymousFeedbackAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class AnonymousFeedbackConfiguration : IEntityTypeConfiguration<AnonymousFeedback>
    {
        public void Configure(EntityTypeBuilder<AnonymousFeedback> builder)
        {
            builder.ToTable("AnonymousFeedbacks");
        }
    }
}

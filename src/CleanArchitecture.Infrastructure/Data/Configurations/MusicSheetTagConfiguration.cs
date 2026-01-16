using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class MusicSheetTagConfiguration : IEntityTypeConfiguration<MusicSheetTag>
    {
        public void Configure(EntityTypeBuilder<MusicSheetTag> builder)
        {
            builder.ToTable("MusicSheetTags");
            
            builder.HasIndex(t => t.Name).IsUnique();
        }
    }
}

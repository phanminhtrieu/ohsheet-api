using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class MusicSheetConfiguration : IEntityTypeConfiguration<MusicSheet>
    {
        public void Configure(EntityTypeBuilder<MusicSheet> builder)
        {
            builder.ToTable("MusicSheets");

            builder
                .HasMany(s => s.Tags)
                .WithMany(t => t.MusicSheets)
                .UsingEntity(j => j.ToTable("MusicSheetTagMappings"));
        }
    }
}
